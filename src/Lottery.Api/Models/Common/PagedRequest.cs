namespace Lottery.Api.Models.Common;

public class PagedRequest
{
    public required int Page { get; set; }
    public required int Limit { get; set; }
}