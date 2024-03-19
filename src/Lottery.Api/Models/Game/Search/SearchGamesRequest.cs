using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Game.Search;

public class SearchGamesRequest
{
    [Required, FromQuery, BindProperty(Name = "")]
    public required SearchGamesRequestQuery Query { get; set; }
}