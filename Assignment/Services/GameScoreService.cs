using Firebase.Database;
using Firebase.Database.Query;
using ReCAI.Models;

namespace ReCAI.Services;

public class GameScoreService
{
    private const string FirebaseUrl = "https://recai-fdb65-default-rtdb.europe-west1.firebasedatabase.app/";

    private readonly FirebaseClient firebaseClient;

    public GameScoreService()
    {
        firebaseClient = new FirebaseClient(FirebaseUrl);
    }

    public async Task SaveScoreAsync(GameScore newScore)
    {
        if (string.IsNullOrWhiteSpace(newScore.UserId))
            return;

        var existingScore = await GetScoreByUserIdAsync(newScore.UserId);

        if (existingScore == null)
        {
            newScore.Id = Guid.NewGuid().ToString();
            newScore.BestScore = newScore.LastScore;
            newScore.UpdatedAt = DateTime.Now;

            await firebaseClient
                .Child("GameScores")
                .Child(newScore.Id)
                .PutAsync(newScore);

            return;
        }

        existingScore.UserEmail = newScore.UserEmail;
        existingScore.UserName = newScore.UserName;
        existingScore.LastScore = newScore.LastScore;
        existingScore.TotalQuestions = newScore.TotalQuestions;
        existingScore.UpdatedAt = DateTime.Now;

        if (newScore.LastScore > existingScore.BestScore)
        {
            existingScore.BestScore = newScore.LastScore;
        }

        await firebaseClient
            .Child("GameScores")
            .Child(existingScore.Id)
            .PutAsync(existingScore);
    }

    public async Task<GameScore?> GetScoreByUserIdAsync(string userId)
    {
        var scores = await firebaseClient
            .Child("GameScores")
            .OnceAsync<GameScore>();

        return scores
            .Select(item => item.Object)
            .FirstOrDefault(score => score.UserId == userId);
    }

    public async Task UpdateUserDetailsAsync(string userId, string newEmail, string newUserName)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return;

        var existingScore = await GetScoreByUserIdAsync(userId);

        if (existingScore == null)
            return;

        existingScore.UserEmail = newEmail;
        existingScore.UserName = string.IsNullOrWhiteSpace(newUserName) ? newEmail : newUserName;
        existingScore.UpdatedAt = DateTime.Now;

        await firebaseClient
            .Child("GameScores")
            .Child(existingScore.Id)
            .PutAsync(existingScore);
    }

    public async Task DeleteScoreByUserIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return;

        var existingScore = await GetScoreByUserIdAsync(userId);

        if (existingScore == null)
            return;

        await firebaseClient
            .Child("GameScores")
            .Child(existingScore.Id)
            .DeleteAsync();
    }

    public async Task<List<LeaderboardRank>> GetTop3RanksAsync()
    {
        var scores = await firebaseClient
            .Child("GameScores")
            .OnceAsync<GameScore>();

        var allScores = scores
            .Select(item => item.Object)
            .Where(score => !string.IsNullOrWhiteSpace(score.UserId))
            .GroupBy(score => score.UserId)
            .Select(group => group.OrderByDescending(score => score.BestScore).First())
            .OrderByDescending(score => score.BestScore)
            .ToList();

        var topRanks = allScores
            .GroupBy(score => score.BestScore)
            .OrderByDescending(group => group.Key)
            .Take(3)
            .Select((group, index) => new LeaderboardRank
            {
                Rank = index + 1,
                Score = group.Key,
                UserNames = string.Join(", ", group.Select(score =>
                    string.IsNullOrWhiteSpace(score.UserName) ? score.UserEmail : score.UserName))
            })
            .ToList();

        return topRanks;
    }
}