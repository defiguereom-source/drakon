using System;
using System.Collections.Generic;
using System.Text;

namespace Drakon.Application.DTOs;

public record RegisterDto(string Email, string FullName, string Password);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, int UserId, string FullName, string Email);