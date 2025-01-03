namespace LudoSnakeAndLadder.Client.Api.Models;

public class StartGameClientResponse
{

    public string GameCode {  get; set; }
    public string Player1Code { get; set; }
    public string Player2Code { get; set; }
    public string Player3Code { get; set; }
    public string Player4Code { get; set; }

}

public class PlayGameClientResponse
{

    public int DiceNum { get; set; }
    public string PlayerAction { get; set; }

}