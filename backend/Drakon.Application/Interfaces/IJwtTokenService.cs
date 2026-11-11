using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Domain;

namespace Drakon.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string Generate(User user);
    }
}
