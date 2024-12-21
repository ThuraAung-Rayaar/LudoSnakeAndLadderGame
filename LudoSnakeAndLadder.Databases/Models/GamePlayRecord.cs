using System;
using System.Collections.Generic;

namespace LudoSnakeAndLadder.Databases.Models;

public partial class GamePlayRecord
{
    public int MoveId { get; set; }

    public string? PlayerId { get; set; }

    public int? RolledDice { get; set; }

    public int? OldPosition { get; set; }

    public int? NewPosition { get; set; }

    public string? MoveType { get; set; }

    public string? GameCode { get; set; }

    public virtual GameRecord? GameCodeNavigation { get; set; }

    public virtual SnakeBoard? NewPositionNavigation { get; set; }

    public virtual SnakeBoard? OldPositionNavigation { get; set; }

    public virtual PlayerCharacter? Player { get; set; }
}
