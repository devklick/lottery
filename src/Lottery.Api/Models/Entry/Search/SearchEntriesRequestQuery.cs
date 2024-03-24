using Lottery.Api.Models.Common;

namespace Lottery.Api.Models.Entry.Search;

public class SearchEntriesRequestQuery : PagedRequest
{
    public Guid? GameId { get; set; }
}