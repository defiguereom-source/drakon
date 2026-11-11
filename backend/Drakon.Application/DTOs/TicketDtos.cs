using Drakon.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drakon.Application.DTOs;

public record CreateTicketDto(string Title, string? Description, TicketPriority Priority, int? AssigneeId);
public record UpdateTicketDto(string Title, string? Description, TicketPriority Priority, int? AssigneeId);
public record MoveTicketDto(TicketStatus Status, int Position);

public record TicketDto(
    int Id,
    string Code,
    string Title,
    string? Description,
    TicketStatus Status,
    TicketPriority Priority,
    int Position,
    int? AssigneeId,
    string? AssigneeName,
    string ReporterName,
    DateTime CreatedAt);