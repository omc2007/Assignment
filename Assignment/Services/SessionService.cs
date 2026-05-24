using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCAI.Services;

public class SessionService : ISessionService
{
    public string? CurrentUserId { get; set; }
    public bool IsAdmin { get; set; }

    public void SignOut()
    {
        CurrentUserId = null;
        IsAdmin = false;
    }
}

