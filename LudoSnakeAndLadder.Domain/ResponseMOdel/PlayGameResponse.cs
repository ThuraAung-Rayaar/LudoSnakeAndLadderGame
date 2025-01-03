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
    public string PlayerName { get; set; } = "";
    public string PlayerColor { get; set; } = "";
    public List<PlayedCell> PlayRecord { get; set; } = new List<PlayedCell>();

   

    public class PlayedCell
    {
        public int OldPosition { get; set; }
        public int NewPosition { get; set; }
        public string CellType { get; set; }

       
        public PlayedCell(int oldPosition, int newPosition, string cellType)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
            CellType = cellType;
        }

       
        public static PlayedCell CallPlayRecord(int oldPosition, int newPosition, string cellType)
        {
            return new PlayedCell(oldPosition, newPosition, cellType);
        }
    }

   
    public PlayedCell CreatePlayRecord(int oldPosition, int newPosition, string cellType)
    {
        PlayedCell newRecord = PlayedCell.CallPlayRecord(oldPosition, newPosition, cellType);
        return newRecord;
    }

   
    
}

