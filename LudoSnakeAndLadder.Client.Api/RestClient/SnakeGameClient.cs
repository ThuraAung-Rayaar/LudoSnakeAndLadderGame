using RestSharp;

namespace LudoSnakeAndLadder.Client.Api.Rest
{
    public  class SnakeGameClient
    {
        private readonly RestClient _client;

        public SnakeGameClient(RestClient client)
        {
            _client = client;
        }

      public async Task StartGame() {


            var request = new RestRequest("/StartGame", Method.Post);
            var respone =await _client.ExecutePostAsync(request);

            

        
        
        }



    }
}
