namespace Lottery.Common.Models;

public enum ResultStatus
{
    ServerError,
    BadRequest,
    NotFound,
    NotAuthenticated,
    NotAuthorized,
    Ok
}