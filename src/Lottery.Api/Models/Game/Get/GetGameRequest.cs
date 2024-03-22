using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Game.Get;

public class GetGameRequest
{
    [Required, FromRoute, BindProperty(Name = "")]
    public required GetGameRequestRoute Route { get; set; }
}