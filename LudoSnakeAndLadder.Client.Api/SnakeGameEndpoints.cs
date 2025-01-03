using LudoSnakeAndLadder.Client.Api.Models;
using LudoSnakeAndLadder.Client.Api.Rest;
using LudoSnakeAndLadder.Domain.AppModel;
using LudoSnakeAndLadder.Domain.ResponseMOdel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LudoSnakeAndLadder.Client.Api;

public static class SnakeGameEndpoints
{
    public static void StartSnakeGameEndpoints(this IEndpointRouteBuilder app) 
    {
       
        string responseMessage = "Successfully Requested";
      

        app.MapPost("/Client/StartGame", async ([FromServices] SnakeGameClient clientService, [FromBody] StartGameRequest requestModel) =>
        {
            StartGameClientResponse? clientGameResponse = null;
            var response =await clientService.StartGameClient(requestModel);
            if(!response.IsSuccessful)
            {
                responseMessage = "Client Request Unsuccessful ";
                return Results. BadRequest(responseMessage);

            }
            var resultObject = JsonConvert.DeserializeObject<Result<StartGameResponseModel>>(response!.Content!.ToString());
            if (!resultObject.IsSuccess)
            {
                responseMessage = "Response From Api : " + resultObject.Message;
               return Results.NotFound(responseMessage); 

            }
            clientGameResponse = new StartGameClientResponse()
            {

                GameCode = resultObject.Data.GameCode,
                Player1Code = resultObject.Data.Player1_Id,
                Player2Code = resultObject.Data.Player2_Id,
                Player3Code = resultObject.Data.Player3_Id,
                Player4Code = resultObject.Data.Player4_Id

            };
            
            return Results.Ok(new { clientGameResponse, responseMessage });
        }).WithName("Start Snake Game").WithOpenApi();

        app.MapPost("/Client/PlayGame",async([FromServices] SnakeGameClient clientService, [FromBody] PlayGameRequest requestModel) =>
        {
            PlayGameClientResponse? clientPlayResponse = null;
            var response =await clientService.PlayGameClient(requestModel);
            if (!response.IsSuccessful) 
            {
                responseMessage = "Client Request Unsuccessful ";
                return Results.BadRequest(responseMessage);
            }

            var resultObject = JsonConvert.DeserializeObject<Result<PlayGameResponse>>(response!.Content!.ToString());
            if (!resultObject.IsSuccess)
            {
                responseMessage = "Response From Api : " + resultObject.Message;
                return Results.NotFound(responseMessage);

            }
            clientPlayResponse = new PlayGameClientResponse() { 
            DiceNum = resultObject.Data.DiceNum,
            PlayerAction = resultObject.Data.playerAction
            
            
            };
            return Results.Ok(new { clientPlayResponse, responseMessage });

        }).WithName("Play Snake Game").WithOpenApi();

    }


}
