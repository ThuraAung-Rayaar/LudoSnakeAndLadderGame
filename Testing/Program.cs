// See https://aka.ms/new-console-template for more information
using LudoSnakeAndLadder.Databases.Models;
using Microsoft.EntityFrameworkCore;



Console.WriteLine("Hello, World!");

SnakeDbContext _dbContext = new SnakeDbContext();
var exitGame = await _dbContext.GameRecords.AsNoTracking().ToListAsync();
var ooon = Ulid.NewUlid();

Console.WriteLine(ooon);
/*string GameUID, Uid;
do
{
    Uid = ooon.ToString();
    GameUID = Uid.Substring(0, 2) + Uid.Substring(Uid.Length - 7, 6);
    Console.WriteLine(GameUID+Uid);
} while (exitGame.Any(x => x.Gamecode == GameUID));
*/






