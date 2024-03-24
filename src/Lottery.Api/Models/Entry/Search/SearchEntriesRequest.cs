using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Entry.Search;

public class SearchEntriesRequest
{
    [Required, FromQuery(Name = "")]
    public required SearchEntriesRequestQuery Query { get; set; }
}