using LudoSnakeAndLadder.Client.Api.Models;
using LudoSnakeAndLadder.Domain.AppModel;
using LudoSnakeAndLadder.Domain.ResponseMOdel;
using RestSharp;

namespace LudoSnakeAndLadder.Client.Api.Rest;

public class SnakeGameClient
{
    private readonly RestClient _client;
    private readonly string _path;

    public SnakeGameClient(RestClient client, string path)
    {
        _client = client;
        _path = path;

    }

    public async Task<RestResponse> StartGameClient(StartGameRequest ReqModel)
    {
        var request = new RestRequest(_path + "/StartGame", Method.Post);
        request.AddJsonBody(ReqModel);

        var respone = await _client.ExecuteAsync<Result<StartGameResponseModel>>(request);
        return respone;
    }

    public async Task<RestResponse> PlayGameClient(PlayGameRequest ReqModel)
    {
        var request = new RestRequest(_path + "/PlayGame", Method.Post);
        request.AddJsonBody(ReqModel);

        var respone = await _client.ExecuteAsync<Result<PlayGameResponse>>(request);
        return respone;
    }
}