using Drakon.Domain.Enums;

namespace Drakon.Domain;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProjectMember
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public UserRole Role { get; set; }
}

public class Comment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Ticket
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public int Number { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public int? AssigneeId { get; set; }
    public User? Assignee { get; set; }
    public int ReporterId { get; set; }
    public User Reporter { get; set; }
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public List<Comment> Comments { get; set; } = new();
}

public class Project
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int OwnerId { get; set; }
    public User Owner { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<ProjectMember> Members { get; set; } = new();
    public List<Ticket> Tickets { get; set; } = new();
}