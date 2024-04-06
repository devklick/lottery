using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Game.Result;

public class ResultGameRequest
{
    [FromRoute, BindProperty(Name = "")]
    public required ResultGameRequestRoute Route { get; set; }

    [FromBody, BindProperty(Name = "")]
    public ResultGameRequestBody Body { get; set; } = new();
}