using System;
using System.Collections.Generic;

namespace LudoSnakeAndLadder.Databases.Models;

public partial class GameRecord
{
    public int GameId { get; set; }

    public string Gamecode { get; set; } = null!;

    public string? FirstWinnerPlayerUid { get; set; }

    public bool? IsGameEnd { get; set; }

    public virtual PlayerCharacter? FirstWinnerPlayerU { get; set; }

    public virtual ICollection<GamePlayRecord> GamePlayRecords { get; set; } = new List<GamePlayRecord>();
}
