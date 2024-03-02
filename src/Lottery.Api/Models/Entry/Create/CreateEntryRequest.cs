using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Entry.Create;

public class CreateEntryRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required CreateEntryRequestBody Body { get; set; }

    public CreateEntryRequestUnbound Unbound { get; set; } = new();
}