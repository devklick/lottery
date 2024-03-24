using System.Net;

using Lottery.Api.Models.Common;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

public class ApiControllerBase : ControllerBase
{
    protected ActionResult<T> CreateActionResult<T>(Result<T> result) => result.Status switch
    {
        ResultStatus.BadRequest => BadRequest(result.Errors),
        ResultStatus.NotFound => NotFound(result.Errors),
        ResultStatus.NotAuthenticated => Unauthorized(result.Errors),
        ResultStatus.Ok => Ok(result.Value),
        _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors),
    };
}