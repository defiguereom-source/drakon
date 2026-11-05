using System.Security.Cryptography.X509Certificates;

namespace Drakon.Domain;

public enum UserRole { Member = 0, Admin = 1};
public enum TicketStatus { Todo = 0, InProgress = 1, Done = 2 };
public enum TicketPriority { Low = 0, Medium = 1, High = 2 };

public class User
{
    public int Id { get; set; }
    public string Name { get; set;  }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProjectManager
{
    public int Id { get; set; }
    public Project project { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

}

public class Comment
{
    public int Id { get; set; }
    public int TicketId { get; set;  }
    public Ticket Ticket { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public string Content { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}

public class Ticket
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public int? AssignedId { get; set; }
    public User? Assigned { get; set; }
    public int Position { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public List<Comment> Comments { get; set; } = new();
}


public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int OwnerId { get; set;  }
    public User Owner { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public List<ProjectManager> Members { get; set; } = new();
    public List<Ticket> Tickets { get; set; } = new();

}