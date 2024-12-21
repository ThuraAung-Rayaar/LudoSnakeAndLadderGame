using System;
using System.Collections.Generic;

namespace LudoSnakeAndLadder.Databases.Models;

public partial class SnakeBoard
{
    public int CellNum { get; set; }

    public string? CellType { get; set; }

    public int? Destination { get; set; }

    public virtual ICollection<GamePlayRecord> GamePlayRecordNewPositionNavigations { get; set; } = new List<GamePlayRecord>();

    public virtual ICollection<GamePlayRecord> GamePlayRecordOldPositionNavigations { get; set; } = new List<GamePlayRecord>();

    public virtual ICollection<PlayerCharacter> PlayerCharacters { get; set; } = new List<PlayerCharacter>();
}
