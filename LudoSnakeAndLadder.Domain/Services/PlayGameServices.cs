using Azure;
using LudoSnakeAndLadder.Databases.Models;
using LudoSnakeAndLadder.Domain.AppModel;
using LudoSnakeAndLadder.Domain.ResponseMOdel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LudoSnakeAndLadder.Domain.Services;

public class PlayGameServices
{
    private readonly SnakeDbContext _dbContext;

    public PlayGameServices(SnakeDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Result<PlayGameResponse>> PlayGame(string GameCode,params string[] playerCode) {

        #region Check parameters
        if (!await GameCode.isGameExist(_dbContext)) return Result<PlayGameResponse>.NotFoundError("GAME Not Found");

        //calling playrecord for player turn
        var LastGamePlay = await _dbContext.GamePlayRecords.MaxAsync(x => x.MoveId);
        int counter = LastGamePlay == 0 ? 0 : LastGamePlay;

        if (playerCode.Count() > 4)
        {

            return Result<PlayGameResponse>.ValidationError("Player COunt More Than 4");

        }
        // Add checker for duplicat player Code

        #endregion

        int oldPosition,newPosition;

        #region Play the Dice
        Random rand = new Random();
        int diceNum = rand.Next(1, 7);
        #endregion


        using var transaction = _dbContext.Database.BeginTransaction();// begin Transaction

        #region Player Check
        int Playerturn = (counter % 4) ;// check playerturn

        var player =await _dbContext.PlayerCharacters.AsNoTracking().FirstOrDefaultAsync(x => x.PlayerUid == playerCode[Playerturn]);//to update
        // check a null error
        if (player is null) return Result<PlayGameResponse>.NotFoundError("Player Not Found"); 

        oldPosition =Convert.ToInt32(player.PlayerCurrentPosition);
        newPosition = oldPosition + diceNum;

        if (newPosition <= 100) { player.PlayerCurrentPosition = newPosition; }// If Dice Num Exceed max Dont move
        // Need to check for backward regress if newPosition is over 100
        else { newPosition = oldPosition; }
        #endregion

        

        #region Add PlayRecord 

        GamePlayRecord gamePlay = new GamePlayRecord() { 
        PlayerId = player.PlayerUid,
        GameCode = GameCode,
        OldPosition= oldPosition,
        NewPosition= player.PlayerCurrentPosition,
        RolledDice = diceNum,
        MoveType = "forward"
        
        
        };

        await _dbContext.GamePlayRecords.AddAsync(gamePlay);


        #endregion
        

        GamePlayRecord? gamePlay2 = null;
        string CellCheckResult="";

        #region GameBoard cell Check

        //readonly from snakeboard
        var cell =await _dbContext.SnakeBoards.FirstOrDefaultAsync(x => x.CellNum == newPosition);
        if (cell is null || newPosition>100) { transaction.Rollback(); Result<PlayGameResponse>.GameSetUpError("Cell Record Not Found"); }


        // need to improve switch
        switch (cell.CellType) {

            case "snake":
            case "ladder"://Change player position and add new play record
                             {   
                                player.PlayerCurrentPosition = cell.Destination;

                                // preparation to add New Play Record
                                int oldPosition2 = Convert.ToInt32(cell.CellNum);
                                int newPosition2 = Convert.ToInt32(player.PlayerCurrentPosition);

                    #region Add PlayRecord 

                    gamePlay2 = new GamePlayRecord()
                    {
                        PlayerId = player.PlayerUid,
                        GameCode = GameCode,
                        OldPosition = oldPosition2,
                        NewPosition = newPosition2,
                        RolledDice = diceNum,
                        MoveType = cell.CellType

                    };

                                await _dbContext.GamePlayRecords.AddAsync(gamePlay2);
                    #endregion

                                CellCheckResult = $"Player Meet {cell.CellType}: player change position to {player.PlayerCurrentPosition}";
                                } 
                            break; 
            default:break;
        
        
        }

        #endregion

        #region Game Winner check

        if (cell.CellType == "win") {
            
            var gameRecord =await _dbContext.GameRecords.AsNoTracking().FirstOrDefaultAsync(x=>x.Gamecode == GameCode);

            gameRecord.FirstWinnerPlayerUid = player.PlayerUid;
            gameRecord.IsGameEnd = true;

            _dbContext.Entry(gameRecord).State = EntityState.Modified;

            CellCheckResult = $"Winner {player.CharacterName} - Color:{player.CharacterColor} - Game Code: {gameRecord.GetHashCode}";
        }
        #endregion


        #region Save to Database
        _dbContext.Entry(player).State = EntityState.Modified;
      
        int result =  await _dbContext.SaveChangesAsync();

        #endregion

        // roll back occur
        if (result == 0) { transaction.Rollback(); return Result<PlayGameResponse>.SystemError("Error Saving Game Record Rolling Back Data"); }


        
        transaction.Commit(); // commit if the above update  are made successfully

        string FirstMoveResult = $"Player {player.CharacterName} move from {gamePlay.OldPosition} to {gamePlay.NewPosition}\n";
        

        PlayGameResponse gameResponse = new PlayGameResponse(counter, FirstMoveResult + CellCheckResult);

        return Result<PlayGameResponse>.Success(gameResponse);
        
        
    
         

        
       

    }

   



}

public static class ServiceHelper {


    public static async Task<bool> isGameExist(this string code,SnakeDbContext dbContext) { 
    
        var isExist = dbContext.GameRecords.Any(x=>x.Gamecode == code && x.IsGameEnd == false);
        return isExist;
    }



}
