using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Domain.Enums;

namespace Drakon.Application.DTOs;

public record CreateProjectDto(string Name, string Key, string? Description);
public record ProjectDto(int Id, string Name, string Key, string? Description, int OwnerId, int TicketCount);
public record AddMemberDto(int UserId, UserRole Role);