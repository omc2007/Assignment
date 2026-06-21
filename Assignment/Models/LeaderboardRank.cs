using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCAI.Models;

public class LeaderboardRank
{
    public int Rank { get; set; }

    public string UserNames { get; set; } = string.Empty;

    public int Score { get; set; }

    public string DisplayText => $"Place {Rank}: {UserNames} - {Score} points";
}
