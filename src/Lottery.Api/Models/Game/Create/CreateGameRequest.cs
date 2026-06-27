using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Game.Create;

public class CreateGameRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required CreateGameRequestBody Body { get; set; }

}