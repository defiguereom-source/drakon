using Drakon.Api.Extensions;
using Drakon.Application.DTOs;
using Drakon.Application.Interfaces;
using Drakon.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Drakon.Api.Controllers;

[ApiController]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _tickets;
    public TicketsController(ITicketService tickets) => _tickets = tickets;

    [HttpGet("api/projects/{projectId:int}/tickets")]
    public async Task<ActionResult<List<TicketDto>>> GetByProject(
        int projectId, [FromQuery] TicketStatus? status)
        => Ok(await _tickets.GetByProjectAsync(User.GetUserId(), projectId, status));

    [HttpPost("api/projects/{projectId:int}/tickets")]
    public async Task<ActionResult<TicketDto>> Create(int projectId, CreateTicketDto dto)
        => Ok(await _tickets.CreateAsync(User.GetUserId(), projectId, dto));

    [HttpGet("api/tickets/{id:int}")]
    public async Task<ActionResult<TicketDto>> Get(int id)
        => Ok(await _tickets.GetAsync(User.GetUserId(), id));

    [HttpPut("api/tickets/{id:int}")]
    public async Task<ActionResult<TicketDto>> Update(int id, UpdateTicketDto dto)
        => Ok(await _tickets.UpdateAsync(User.GetUserId(), id, dto));

    [HttpPatch("api/tickets/{id:int}/move")]
    public async Task<ActionResult<TicketDto>> Move(int id, MoveTicketDto dto)
        => Ok(await _tickets.MoveAsync(User.GetUserId(), id, dto));

    [HttpDelete("api/tickets/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _tickets.DeleteAsync(User.GetUserId(), id);
        return NoContent();
    }
}