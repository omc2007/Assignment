using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCAI.Models;



public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Mobile { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string Password { get; set; } = "";
    public bool IsAdmin { get; set; } = false;

    public string FullName => $"{FirstName} {LastName}";
    public string CreatedAtShort => CreatedAt.ToString("dd/MM/yyyy");
}


