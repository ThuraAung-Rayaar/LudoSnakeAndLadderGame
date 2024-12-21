using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoSnakeAndLadder.Domain.ResponseMOdel;

public class StartGameResponseModel { 

    public string GameCode { get; set; }
    public string Player1_Id { get; set; }

    public string Player2_Id { get; set; }
    public string Player3_Id { get; set; }
    public string Player4_Id { get; set; }





}

public class PlayerResponseModel {
    public string PlayerUid { get; set; } = null!;

    public string? CharacterName { get; set; }

    public string? CharacterColor { get; set; }

    public int? PlayerCurrentPosition { get; set; }

   

}
