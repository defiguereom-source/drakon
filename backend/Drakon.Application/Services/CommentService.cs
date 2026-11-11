using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Application.DTOs;
using Drakon.Application.Interfaces;
using Drakon.Domain;
using Microsoft.EntityFrameworkCore;

namespace Drakon.Application.Services;

public class CommentService : ICommentService
{
    private readonly IAppDbContext _db;
    private readonly IProjectService _projects;

    public CommentService(IAppDbContext db, IProjectService projects)
    {
        _db = db;
        _projects = projects;
    }

    public async Task<List<CommentDto>> GetByTicketAsync(int userId, int ticketId)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId)
            ?? throw new KeyNotFoundException("Ticket no encontrado.");
        await _projects.EnsureMemberAsync(userId, ticket.ProjectId);

        return await _db.Comments
            .Where(c => c.TicketId == ticketId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto(c.Id, c.Content, c.AuthorId, c.Author.FullName, c.CreatedAt))
            .ToListAsync();
    }

    public async Task<CommentDto> CreateAsync(int userId, int ticketId, CreateCommentDto dto)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId)
            ?? throw new KeyNotFoundException("Ticket no encontrado.");
        await _projects.EnsureMemberAsync(userId, ticket.ProjectId);

        var comment = new Comment
        {
            TicketId = ticketId,
            AuthorId = userId,
            Content = dto.Content.Trim()
        };
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();

        var author = await _db.Users.FirstAsync(u => u.Id == userId);
        return new CommentDto(comment.Id, comment.Content, userId, author.FullName, comment.CreatedAt);
    }
}