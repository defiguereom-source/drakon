using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Application.DTOs;
using Drakon.Application.Interfaces;
using Drakon.Domain;
using Drakon.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Drakon.Application.Services;

public class TicketService : ITicketService
{
    private readonly IAppDbContext _db;
    private readonly IProjectService _projects;

    public TicketService(IAppDbContext db, IProjectService projects)
    {
        _db = db;
        _projects = projects;
    }

    private static TicketDto ToDto(Ticket t) => new(
        t.Id, $"{t.Project.Key}-{t.Number}", t.Title, t.Description,
        t.Status, t.Priority, t.Position,
        t.AssigneeId, t.Assignee?.FullName, t.Reporter.FullName, t.CreatedAt);

    private IQueryable<Ticket> Query() => _db.Tickets
        .Include(t => t.Project)
        .Include(t => t.Assignee)
        .Include(t => t.Reporter);

    public async Task<List<TicketDto>> GetByProjectAsync(int userId, int projectId, TicketStatus? status)
    {
        await _projects.EnsureMemberAsync(userId, projectId);

        var q = Query().Where(t => t.ProjectId == projectId);
        if (status.HasValue) q = q.Where(t => t.Status == status);

        var list = await q.OrderBy(t => t.Status).ThenBy(t => t.Position).ToListAsync();
        return list.Select(ToDto).ToList();
    }

    public async Task<TicketDto> GetAsync(int userId, int ticketId)
    {
        var ticket = await Query().FirstOrDefaultAsync(t => t.Id == ticketId)
            ?? throw new KeyNotFoundException("Ticket no encontrado.");
        await _projects.EnsureMemberAsync(userId, ticket.ProjectId);
        return ToDto(ticket);
    }

    public async Task<TicketDto> CreateAsync(int userId, int projectId, CreateTicketDto dto)
    {
        await _projects.EnsureMemberAsync(userId, projectId);

        if (dto.AssigneeId.HasValue)
            await _projects.EnsureMemberAsync(dto.AssigneeId.Value, projectId);

        var nextNumber = (await _db.Tickets
            .Where(t => t.ProjectId == projectId)
            .MaxAsync(t => (int?)t.Number) ?? 0) + 1;

        var nextPosition = (await _db.Tickets
            .Where(t => t.ProjectId == projectId && t.Status == TicketStatus.Todo)
            .MaxAsync(t => (int?)t.Position) ?? -1) + 1;

        var ticket = new Ticket
        {
            ProjectId = projectId,
            Number = nextNumber,
            Title = dto.Title.Trim(),
            Description = dto.Description,
            Priority = dto.Priority,
            AssigneeId = dto.AssigneeId,
            ReporterId = userId,
            Position = nextPosition
        };

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();

        return await GetAsync(userId, ticket.Id);
    }

    public async Task<TicketDto> UpdateAsync(int userId, int ticketId, UpdateTicketDto dto)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId)
            ?? throw new KeyNotFoundException("Ticket no encontrado.");
        await _projects.EnsureMemberAsync(userId, ticket.ProjectId);

        if (dto.AssigneeId.HasValue)
            await _projects.EnsureMemberAsync(dto.AssigneeId.Value, ticket.ProjectId);

        ticket.Title = dto.Title.Trim();
        ticket.Description = dto.Description;
        ticket.Priority = dto.Priority;
        ticket.AssigneeId = dto.AssigneeId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return await GetAsync(userId, ticketId);
    }

    public async Task<TicketDto> MoveAsync(int userId, int ticketId, MoveTicketDto dto)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId)
            ?? throw new KeyNotFoundException("Ticket no encontrado.");
        await _projects.EnsureMemberAsync(userId, ticket.ProjectId);

        var oldStatus = ticket.Status;

        // columna destino (sin el ticket que se mueve)
        var target = await _db.Tickets
            .Where(t => t.ProjectId == ticket.ProjectId && t.Status == dto.Status && t.Id != ticket.Id)
            .OrderBy(t => t.Position)
            .ToListAsync();

        var index = Math.Clamp(dto.Position, 0, target.Count);
        ticket.Status = dto.Status;
        ticket.UpdatedAt = DateTime.UtcNow;
        target.Insert(index, ticket);

        for (var i = 0; i < target.Count; i++)
            target[i].Position = i;

        // si cambió de columna, reordenar la columna origen
        if (oldStatus != dto.Status)
        {
            var origin = await _db.Tickets
                .Where(t => t.ProjectId == ticket.ProjectId && t.Status == oldStatus && t.Id != ticket.Id)
                .OrderBy(t => t.Position)
                .ToListAsync();

            for (var i = 0; i < origin.Count; i++)
                origin[i].Position = i;
        }

        await _db.SaveChangesAsync();
        return await GetAsync(userId, ticketId);
    }

    public async Task DeleteAsync(int userId, int ticketId)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId)
            ?? throw new KeyNotFoundException("Ticket no encontrado.");
        await _projects.EnsureMemberAsync(userId, ticket.ProjectId);

        _db.Tickets.Remove(ticket);
        await _db.SaveChangesAsync();
    }
}