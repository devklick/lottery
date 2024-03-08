namespace Lottery.Api.Models.Common;

public class PagedResponse<TItem>
{
    public required IEnumerable<TItem> Items { get; set; }
    public required int Page { get; set; }
    public required int Limit { get; set; }
    public required bool HasMore { get; set; }
}