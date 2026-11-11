using Drakon.Application.DTOs;
using Drakon.Domain.Enums;

namespace Drakon.Application.Interfaces;

public interface ITicketService
{
    Task<List<TicketDto>> GetByProjectAsync(int userId, int projectId, TicketStatus? status);
    Task<TicketDto> GetAsync(int userId, int ticketId);
    Task<TicketDto> CreateAsync(int userId, int projectId, CreateTicketDto dto);
    Task<TicketDto> UpdateAsync(int userId, int ticketId, UpdateTicketDto dto);
    Task<TicketDto> MoveAsync(int userId, int ticketId, MoveTicketDto dto);
    Task DeleteAsync(int userId, int ticketId);
}