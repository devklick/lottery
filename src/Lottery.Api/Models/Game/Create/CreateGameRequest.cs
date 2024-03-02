using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lottery.Api.Models.Game.Create;

public class CreateGameRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required CreateGameRequestBody Body { get; set; }

    [BindNever]
    public CreateGameRequestUnbound Unbound { get; set; } = new();
}