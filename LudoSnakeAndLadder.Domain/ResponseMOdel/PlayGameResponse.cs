using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoSnakeAndLadder.Domain.ResponseMOdel;

public class PlayGameResponse
{
    public PlayGameResponse(int diceNum, string playerAction)
    {
        DiceNum = diceNum;
        this.playerAction = playerAction;
    }

    public int DiceNum { get; set; }
    public string playerAction { get; set; }


}


public class PlayGameResponse2
{
    public string PlayerName { get; set; }
    public string PlayerColor { get; set; }
    public List<PlayedCell> PlayRecord { get; set; } = new List<PlayedCell>();

    public PlayGameResponse2()
    {
        
        PlayRecord = new List<PlayedCell>();
    }

    public class PlayedCell
    {
        public int OldPosition { get; set; }
        public int NewPosition { get; set; }
        public string CellType { get; set; }

        // Constructor to initialize PlayedCell
        public PlayedCell(int oldPosition, int newPosition, string cellType)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
            CellType = cellType;
        }

        // Static factory method for creating a PlayedCell
        public static PlayedCell CallPlayRecord(int oldPosition, int newPosition, string cellType)
        {
            return new PlayedCell(oldPosition, newPosition, cellType);
        }
    }

    // Method to create and add a play record
    public void CreatePlayRecord(int oldPosition, int newPosition, string cellType)
    {
        PlayedCell newRecord = PlayedCell.CallPlayRecord(oldPosition, newPosition, cellType);
        PlayRecord.Add(newRecord);
    }

    // Display all play records (for testing purposes)
    public void DisplayPlayRecords()
    {
        Console.WriteLine($"Player: {PlayerName}, Color: {PlayerColor}");
        foreach (var record in PlayRecord)
        {
            Console.WriteLine($"OldPosition: {record.OldPosition}, NewPosition: {record.NewPosition}, CellType: {record.CellType}");
        }
    }
}

