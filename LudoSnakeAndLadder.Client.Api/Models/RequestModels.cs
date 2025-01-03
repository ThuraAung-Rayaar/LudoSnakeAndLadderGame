using System.ComponentModel.DataAnnotations;

namespace LudoSnakeAndLadder.Client.Api.Models;

public class StartGameRequest
{
    [Required]
    public string Player1 { get; set; }
    [Required]
    public string Player2 { get; set; }
    [Required]
    public string Player3 { get; set; }
    [Required]
    public string Player4 { get; set; }


}


public class PlayGameRequest
{
    [Required]
    public string GameCode { get; set; }
    [Required]
    public string Player1 { get; set; }
    [Required]
    public string Player2 { get; set; }
    [Required]
    public string Player3 { get; set; }
    [Required]
    public string Player4 { get; set; }


}