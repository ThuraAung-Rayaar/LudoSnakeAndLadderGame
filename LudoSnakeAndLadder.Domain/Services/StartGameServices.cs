using LudoSnakeAndLadder.Databases.Models;
using LudoSnakeAndLadder.Domain.AppModel;
using LudoSnakeAndLadder.Domain.ResponseMOdel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoSnakeAndLadder.Domain.Services;

public class StartGameServices
{
    private readonly SnakeDbContext _dbContext;

    public StartGameServices(SnakeDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Result<StartGameResponseModel>> StartGame(string name1,string name2,string name3,string name4) {

        /*
         
        Start Game will only be called one time
        
        1.setup Gameboard
        2. Setup Game Room
        3.set up Player

        Return GameCode and 4 Player Code

         
         */



        // Game Board Setup
        await ChangeGameBoard();

       
        Result<StartGameResponseModel> result = null;
        StartGameResponseModel response = null;

        //Game Room setup
        string gameCode = await GameSetUp();
        if (gameCode is null) goto GameErrorReturn;


        #region  Player Set up
        var P1 = await PlayerSetUp(name1,"red");
        if (P1 is null) goto PlayerErrorReturn;


        var P2 = await PlayerSetUp(name2,"green");
        if (P2 is null) goto PlayerErrorReturn;

        var P3 = await PlayerSetUp(name3,"orange");
        if(P3 is null) goto PlayerErrorReturn;

        var P4 = await PlayerSetUp(name4,"blue");
        if( P4 is null) goto PlayerErrorReturn;

        #endregion

        response = new StartGameResponseModel() { 
        GameCode = gameCode,
        Player1_Id = P1.PlayerUid,
        Player2_Id = P2.PlayerUid,
        Player3_Id = P3.PlayerUid,
        Player4_Id = P4.PlayerUid
        

        
        };


       
         result =   Result<StartGameResponseModel>.Success(response);
         return result;

        //Error Response 
        PlayerErrorReturn: { result = Result<StartGameResponseModel>.PlayerNotFoundError(); return result; }

        GameErrorReturn: { result = Result<StartGameResponseModel>.GameSetUpError(); return result; }
    }

    public async Task<string?> GameSetUp() {

        var exitGame = await _dbContext.GameRecords.AsNoTracking().ToListAsync();
        string GameUID, Uid;
        do
        {
            Uid = Ulid.NewUlid().ToString();
            GameUID = Uid.Substring(0, 2) + Uid.Substring(Uid.Length - 7, 6);
        } while (exitGame.Any(x=>x.Gamecode == GameUID));
       

        GameRecord game = new GameRecord() { 
        
        
        FirstWinnerPlayerUid = null,
        Gamecode = GameUID,
        IsGameEnd = false,
        
        
        };


        await _dbContext.GameRecords.AddAsync(game);
        int res =   await _dbContext.SaveChangesAsync();

        return res >0 ? game.Gamecode:null;



    }

    public async Task ChangeGameBoard()
    {
        await SetGameBoard();

        using var transaction =await _dbContext.Database.BeginTransactionAsync();

        var cells =await _dbContext.SnakeBoards.OrderBy(x=>x.CellNum).ToListAsync();

        Random rand = new Random();
        int ladderNum = 7;
        int snakeNum = 9;
        HashSet<int> Usedcell = new HashSet<int>();

        //Random adding for Snake | start cell: 11 , end cell : 99
        for (int i = 1; i <= snakeNum; i++)
        {
            int start, end;
            do { start = rand.Next(13, 100); } while (Usedcell.Contains(start));
            Usedcell.Add(start);

            do { end = rand.Next(2, start - 9); } while (Usedcell.Contains(end));
            Usedcell.Add(end);


            var EachCell = cells.FirstOrDefault(x => x.CellNum == start);
            if (EachCell is not null)
            {
                EachCell.Destination = end;
                EachCell.CellType = "snake";
               // _dbContext.Entry(EachCell).State = EntityState.Modified;
            }


        }

        //Random adding for Ladder | start cell: 2 , end cell : 98
        for (int i = 1; i <= ladderNum; i++) {

            int start, end;


            do{    start = rand.Next(2,79);   } while (Usedcell.Contains(start));
            Usedcell.Add(start);

            do {  end = rand.Next(start + 10, 96);  } while (Usedcell.Contains(end));
            Usedcell.Add(end);


            var EachCell = cells.FirstOrDefault(x => x.CellNum == start);
            if (EachCell is not null)
            {
                EachCell.Destination = end;
                EachCell.CellType = "ladder";
               // _dbContext.Entry(EachCell).State = EntityState.Modified;
            }

          
           
        }

      

        var wincell = cells.Where(x => x.CellNum == 100).FirstOrDefault();
        wincell.CellType = "win";
          //_dbContext.Entry(wincell).State = EntityState.Modified;

      int result =   await  _dbContext.SaveChangesAsync();
        if (result == 0)
        {
            transaction.Rollback();

        }
        transaction.Commit();


    }
    public async Task SetGameBoard() { 
    
        

        var cells = await   _dbContext.SnakeBoards.AsNoTracking().ToListAsync();

        foreach (var cell in cells) { 
            cell.CellType = null;
            cell.Destination = null;
            _dbContext.Entry(cell).State = EntityState.Modified;
        }
        
       await _dbContext.SaveChangesAsync();
    
    }

    public async Task<PlayerResponseModel> PlayerSetUp(string name,string color) {

        color = color.ToLower();

        
        
        PlayerResponseModel? response = null;
        PlayerCharacter? newplayer = null;
        
        //set up ULID
        string Uid = Ulid.NewUlid().ToString();
        string PUID = Uid.Substring(0,2)+Uid.Substring(10,2)+Uid.Substring(Uid.Length/2,2)+Uid.Substring(Uid.Length-3,2);

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        //get player with color
        var player = await _dbContext.PlayerCharacters.FirstOrDefaultAsync(x => x.CharacterColor == color);

        // if no player is called in new game and change the player info in subsequent game
        if (player is null)
        {

             newplayer = new PlayerCharacter
            {
                PlayerUid = PUID,
                CharacterColor = color,
                CharacterName = name,
                IsPlayerWin = false,
                PlayerCurrentPosition = 1

            };

            _dbContext.PlayerCharacters.Add(newplayer);

        }
        else {
            // Needed to Add a PlayerID identity primary key To make PlayerUID dynamicall changable
            //player.PlayerUid = PUID;
            player.CharacterName = name;
            player.IsPlayerWin = false;
            player.PlayerCurrentPosition = 1;
           
           // _dbContext.Entry(player).State = EntityState.Modified;

        
        
        }
        

        int result =    await _dbContext.SaveChangesAsync();


       

        //rollback if no changes takeplace in databases
        if (result == 0) {

            transaction.Rollback();
            return response;  
        }


        //commit and return response model
        transaction.Commit();
        response = new PlayerResponseModel()
        {
            PlayerUid = (player is null) ? newplayer!.PlayerUid : player.PlayerUid,
            CharacterColor = (player is null) ? newplayer!.CharacterColor : player.CharacterColor,
            CharacterName = (player is null) ? newplayer!.CharacterName : player.CharacterName,

            PlayerCurrentPosition = 1
        };

        return response;

    }

}



