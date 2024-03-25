using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Game.Edit;

public class EditGameRequest
{
    [Required, FromRoute, BindProperty(Name = "")]
    public required EditGameRequestRoute Route { get; set; }

    [Required, FromBody, BindProperty(Name = "")]
    public required EditGameRequestBody Body { get; set; }

}