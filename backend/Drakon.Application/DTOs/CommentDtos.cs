using System;
using System.Collections.Generic;
using System.Text;

namespace Drakon.Application.DTOs;

public record CreateCommentDto(string Content);
public record CommentDto(int Id, string Content, int AuthorId, string AuthorName, DateTime CreatedAt);