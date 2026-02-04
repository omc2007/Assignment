using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assignment.Models;

namespace Assignment.Services;



public class InMemoryUserService : IUserService
{
    private readonly List<AppUser> users = new();

    public const string AdminEmail = "admin@gmail.com";
    public const string AdminPassword = "admin9";

    public InMemoryUserService()
    {
        users.Add(new AppUser
        {
            FirstName = "Admin",
            LastName = "User",
            Email = AdminEmail,
            Mobile = "0500000000",
            Password = AdminPassword,
            IsAdmin = true,
            CreatedAt = new DateTime(2025, 1, 1)
        });

        users.Add(new AppUser { FirstName = "r", LastName = "Lav", Email = "r@gmail.com", Mobile = "0500000001", Password = "8", CreatedAt = new DateTime(2025, 1, 29) });
        users.Add(new AppUser { FirstName = "Max", LastName = "Zab", Email = "max@gmail.com", Mobile = "0500000002", Password = "2222", CreatedAt = new DateTime(2025, 1, 30) });
        users.Add(new AppUser { FirstName = "K", LastName = "Z", Email = "k@gmail.com", Mobile = "0500000003", Password = "3333", CreatedAt = new DateTime(2025, 1, 29) });
    }

    public IReadOnlyList<AppUser> GetAll() => users;

    public AppUser? GetById(string id) => users.FirstOrDefault(x => x.Id == id);

    public AppUser? GetByEmail(string email) =>
        users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public bool ExistsByEmail(string email) => GetByEmail(email) != null;

    public void Add(AppUser user) => users.Add(user);

    public void Update(AppUser user)
    {
        var existing = GetById(user.Id);
        if (existing == null) return;

        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;
        existing.Email = user.Email;
        existing.Mobile = user.Mobile;
        if (!string.IsNullOrWhiteSpace(user.Password))
            existing.Password = user.Password;
        
    }

    public void Delete(string id)
    {
        var existing = GetById(id);
        if (existing == null) return;
        users.Remove(existing);
    }
}




