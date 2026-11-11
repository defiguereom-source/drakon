using Drakon.Api.Extensions;
using Drakon.Application.DTOs;
using Drakon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Drakon.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/tickets/{ticketId:int}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _comments;
    public CommentsController(ICommentService comments) => _comments = comments;

    [HttpGet]
    public async Task<ActionResult<List<CommentDto>>> Get(int ticketId)
        => Ok(await _comments.GetByTicketAsync(User.GetUserId(), ticketId));

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(int ticketId, CreateCommentDto dto)
        => Ok(await _comments.CreateAsync(User.GetUserId(), ticketId, dto));
}