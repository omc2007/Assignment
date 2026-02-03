using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Services;

public interface ISessionService
{
    string? CurrentUserId { get; set; }
    bool IsAdmin { get; set; }
    void SignOut();
}

