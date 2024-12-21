using System;
using System.Collections.Generic;

namespace LudoSnakeAndLadder.Databases.Models;

public partial class PlayerCharacter
{
    public string PlayerUid { get; set; } = null!;

    public string? CharacterName { get; set; }

    public string? CharacterColor { get; set; }

    public int? PlayerCurrentPosition { get; set; }

    public bool? IsPlayerWin { get; set; }

    public virtual ICollection<GamePlayRecord> GamePlayRecords { get; set; } = new List<GamePlayRecord>();

    public virtual ICollection<GameRecord> GameRecords { get; set; } = new List<GameRecord>();

    public virtual SnakeBoard? PlayerCurrentPositionNavigation { get; set; }
}
