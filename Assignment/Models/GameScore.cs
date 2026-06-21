using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCAI.Models;

public class GameScore
{
    public string Id { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string UserEmail { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public int BestScore { get; set; }

    public int LastScore { get; set; }

    public int TotalQuestions { get; set; }

    public DateTime UpdatedAt { get; set; }
}
