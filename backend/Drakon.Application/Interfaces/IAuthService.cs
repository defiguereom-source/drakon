using Drakon.Application.DTOs;
using Drakon.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drakon.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);

    }
}
