using Drakon.Application.DTOs;

namespace Drakon.Application.Interfaces;

public interface ICommentService
{
    Task<List<CommentDto>> GetByTicketAsync(int userId, int ticketId);
    Task<CommentDto> CreateAsync(int userId, int ticketId, CreateCommentDto dto);
}