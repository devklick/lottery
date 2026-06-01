using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Game.Search;

public class SearchGamesRequest
{
    [FromQuery, BindProperty(Name = "")]
    public SearchGamesRequestQuery Query { get; set; } = new();
}