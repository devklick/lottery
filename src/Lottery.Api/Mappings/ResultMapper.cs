using System.Net;

using Lottery.Common.Models;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Mappings;

public sealed class ResultMapper
{
    public ObjectResult Map<T>(Result<T> result)
    {
        return result.Status switch
        {
            ResultStatus.BadRequest => new BadRequestObjectResult(result),
            ResultStatus.NotFound => new NotFoundObjectResult(result),
            ResultStatus.NotAuthenticated => new UnauthorizedObjectResult(result),
            ResultStatus.NotAuthorized => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.Forbidden },
            ResultStatus.Ok => new OkObjectResult(result),
            _ => new ObjectResult(result)
            {
                StatusCode = 500,
                Value = result
            }
        };
    }
}