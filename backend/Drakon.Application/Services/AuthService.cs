using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Application.Common.Exceptions;
using Drakon.Application.DTOs;
using Drakon.Application.Interfaces;
using Drakon.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Drakon.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAppDbContext _db;
    private readonly IJwtTokenService _jwt;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(IAppDbContext db, IJwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        if (await _db.Users.AnyAsync(u => u.Email == email))
            throw new InvalidOperationException("El email ya está registrado.");

        var user = new User { Email = email, FullName = dto.FullName.Trim() };
        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new AuthResponseDto(_jwt.Generate(user), user.Id, user.FullName, user.Email);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new InvalidCredentialsException();

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new InvalidCredentialsException();

        return new AuthResponseDto(_jwt.Generate(user), user.Id, user.FullName, user.Email);
    }
}
