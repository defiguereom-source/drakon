using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Domain;
using Microsoft.EntityFrameworkCore;

namespace Drakon.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectMember> ProjectMembers { get; set; }
        DbSet<Ticket> Tickets { get; set; }
        DbSet<Comment> Comments { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
