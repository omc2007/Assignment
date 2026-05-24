using ReCAI.Models;
using System.Net.Http.Json;

namespace ReCAI.Services;

public class FirebaseUserService
{
    private readonly HttpClient client = new();

    private const string BaseUrl =
        "https://recai-fdb65-default-rtdb.europe-west1.firebasedatabase.app/users.json";

    public async Task AddUser(AppUser user)
    {
        await client.PostAsJsonAsync(BaseUrl, user);
    }

    public async Task<List<AppUser>> GetUsers()
    {
        var result =
            await client.GetFromJsonAsync<Dictionary<string, AppUser>>(BaseUrl);

        return result?.Values.ToList() ?? new List<AppUser>();
    }
}
