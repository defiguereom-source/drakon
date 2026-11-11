using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Application.Interfaces;
using Drakon.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Drakon.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ICommentService, CommentService>();
        return services;
    }
}