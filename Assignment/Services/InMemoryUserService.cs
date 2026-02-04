using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Assignment.Models;

namespace Assignment.Services;



using Microsoft.Maui.Storage;

public class InMemoryUserService : IUserService
{
    private List<AppUser> users = new();
    private const string UsersIdsKey = "UsersIds";

    public const string AdminEmail = "admin@gmail.com";
    public const string AdminPassword = "admin9";

    public InMemoryUserService()
    {
        // טען את כל המשתמשים מה-Preferences
        var idsCsv = Preferences.Get(UsersIdsKey, "");
        if (!string.IsNullOrWhiteSpace(idsCsv))
        {
            var ids = idsCsv.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in ids)
            {
                var user = LoadUser(id);
                if (user != null) users.Add(user);
            }
        }

        // אם אין משתמשים, צור Seed Data
        if (!users.Any())
        {
            SeedData();
            SaveAllUsers();
        }
    }

    private void SeedData()
    {
        Add(new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Admin",
            LastName = "User",
            Email = AdminEmail,
            Mobile = "0500000000",
            Password = AdminPassword,
            IsAdmin = true,
            CreatedAt = new DateTime(2025, 1, 1)
        });

        Add(new AppUser { Id = Guid.NewGuid().ToString(), FirstName = "r", LastName = "Lav", Email = "r@gmail.com", Mobile = "0500000001", Password = "8", CreatedAt = new DateTime(2025, 1, 29) });
        Add(new AppUser { Id = Guid.NewGuid().ToString(), FirstName = "Max", LastName = "Zab", Email = "max@gmail.com", Mobile = "0500000002", Password = "2222", CreatedAt = new DateTime(2025, 1, 30) });
        Add(new AppUser { Id = Guid.NewGuid().ToString(), FirstName = "K", LastName = "Z", Email = "k@gmail.com", Mobile = "0500000003", Password = "3333", CreatedAt = new DateTime(2025, 1, 29) });
    }

    private void SaveAllUsers()
    {
        // שמור את כל ה-Ids
        var idsCsv = string.Join(';', users.Select(u => u.Id));
        Preferences.Set(UsersIdsKey, idsCsv);

        // שמור כל משתמש בנפרד
        foreach (var user in users)
            SaveUser(user);
    }

    private void SaveUser(AppUser user)
    {
        Preferences.Set($"{user.Id}_FirstName", user.FirstName);
        Preferences.Set($"{user.Id}_LastName", user.LastName);
        Preferences.Set($"{user.Id}_Email", user.Email);
        Preferences.Set($"{user.Id}_Mobile", user.Mobile);
        Preferences.Set($"{user.Id}_Password", user.Password ?? "");
        Preferences.Set($"{user.Id}_IsAdmin", user.IsAdmin);
        Preferences.Set($"{user.Id}_CreatedAt", user.CreatedAt.ToBinary());
    }

    private AppUser? LoadUser(string id)
    {
        try
        {
            return new AppUser
            {
                Id = id,
                FirstName = Preferences.Get($"{id}_FirstName", ""),
                LastName = Preferences.Get($"{id}_LastName", ""),
                Email = Preferences.Get($"{id}_Email", ""),
                Mobile = Preferences.Get($"{id}_Mobile", ""),
                Password = Preferences.Get($"{id}_Password", ""),
                IsAdmin = Preferences.Get($"{id}_IsAdmin", false),
                CreatedAt = DateTime.FromBinary(Preferences.Get($"{id}_CreatedAt", DateTime.UtcNow.ToBinary()))
            };
        }
        catch
        {
            return null;
        }
    }

    private void DeleteUserFromPreferences(string id)
    {
        Preferences.Remove($"{id}_FirstName");
        Preferences.Remove($"{id}_LastName");
        Preferences.Remove($"{id}_Email");
        Preferences.Remove($"{id}_Mobile");
        Preferences.Remove($"{id}_Password");
        Preferences.Remove($"{id}_IsAdmin");
        Preferences.Remove($"{id}_CreatedAt");
    }

    public IReadOnlyList<AppUser> GetAll() => users;

    public AppUser? GetById(string id) => users.FirstOrDefault(x => x.Id == id);

    public AppUser? GetByEmail(string email) =>
        users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public bool ExistsByEmail(string email) => GetByEmail(email) != null;

    public void Add(AppUser user)
    {
        if (string.IsNullOrWhiteSpace(user.Id))
            user.Id = Guid.NewGuid().ToString();

        users.Add(user);
        SaveAllUsers();
    }

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
        existing.IsAdmin = user.IsAdmin;
        existing.CreatedAt = user.CreatedAt;

        SaveAllUsers();
    }

    public void Delete(string id)
    {
        var existing = GetById(id);
        if (existing == null) return;

        users.Remove(existing);
        DeleteUserFromPreferences(id);
        SaveAllUsers();
    }
}





