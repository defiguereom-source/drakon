using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Application.Interfaces;
using Drakon.Domain;
using Microsoft.EntityFrameworkCore;

namespace Drakon.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Comment> Comments { get; set; }
}