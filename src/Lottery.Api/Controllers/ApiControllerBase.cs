using System.Net;

using Lottery.Api.Models.Common;
using Lottery.Common.Models;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

public class ApiControllerBase : ControllerBase
{
    protected ActionResult<T> CreateActionResult<T>(Result<T> result) => result.Status switch
    {
        ResultStatus.BadRequest => BadRequest(result),
        ResultStatus.NotFound => NotFound(result),
        ResultStatus.NotAuthenticated => Unauthorized(result),
        ResultStatus.Ok => Ok(result),
        _ => StatusCode((int)HttpStatusCode.InternalServerError, result),
    };
}