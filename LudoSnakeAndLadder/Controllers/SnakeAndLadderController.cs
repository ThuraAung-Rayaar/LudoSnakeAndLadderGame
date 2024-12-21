using LudoSnakeAndLadder.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LudoSnakeAndLadder.MinimalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnakeAndLadderController : ControllerBase
    {
        private readonly StartGameServices _GameStart;
        private readonly PlayGameServices _GamePlay;
        private int counter;

        public SnakeAndLadderController(StartGameServices gameStart, PlayGameServices gamePlay)
        {
            _GameStart = gameStart;
            _GamePlay = gamePlay;
            counter = 0;
        }

        [HttpPost("StartGame")]
        public async Task<IActionResult> StartGame(string Player1Name, string Player2Name, string Player3Name, string Player4Name) {

            try{

             var response =  await  _GameStart.StartGame(Player1Name, Player2Name, Player3Name, Player4Name);
              
                return Ok(response);
            
            }
          catch (Exception ex) {
            
            return StatusCode(500, ex.Message);
            }
                
        
        }

        [HttpPost("PlayGame")]
        public async Task<IActionResult> PlayGame(string gameCode,string Player1Name, string Player2Name, string Player3Name, string Player4Name)
        {

            //try
            {

                var response = await _GamePlay.PlayGame(gameCode,Player1Name, Player2Name, Player3Name, Player4Name);
                counter++;

                return Ok(response);

            }
          /*  catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }*/


        }

    }
}
