using LudoSnakeAndLadder.Api.Models;
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
        public async Task<IActionResult> StartGame(StartGameRequest model) {

            try{

             var response =  await  _GameStart.StartGame(model.Player1, model.Player2,model.Player3,model.Player4);
              
                return Ok(response);
            
            }
          catch (Exception ex) {
            
            return StatusCode(500, ex.Message);
            }
                
        
        }

        [HttpPost("PlayGame")]
        public async Task<IActionResult> PlayGame(PlayGameRequest model)
        {

            try
            {

                var response = await _GamePlay.PlayGame(model.GameCode,model.Player1,model.Player2,model.Player3,model.Player4);
                counter++;

                return Ok(response);

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }


        }

    }
}
