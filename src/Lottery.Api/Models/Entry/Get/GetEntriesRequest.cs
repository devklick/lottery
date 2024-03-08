using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Entry.Get;

public class GetEntriesRequest
{
    [Required, FromQuery(Name = "")]
    public required GetEntriesRequestQuery Query { get; set; }
}