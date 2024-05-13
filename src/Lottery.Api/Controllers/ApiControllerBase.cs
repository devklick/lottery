using System.Net;

using Lottery.Api.Models.Common;
using Lottery.Common.Models;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

public class ApiControllerBase : ControllerBase
{
    protected ActionResult<T> CreateActionResult<T>(Result<T> result) => result.Status switch
    {
        ResultStatus.BadRequest => BadRequest(new { result.Errors }),
        ResultStatus.NotFound => NotFound(new { result.Errors }),
        ResultStatus.NotAuthenticated => Unauthorized(new { result.Errors }),
        ResultStatus.Ok => Ok(result.Value),
        _ => StatusCode((int)HttpStatusCode.InternalServerError, new { result.Errors }),
    };
}