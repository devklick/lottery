using System.Net;

using Lottery.Api.Mappings;
using Lottery.Common.Models;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

public class ApiControllerBase(ResultMapper resultMapper) : ControllerBase
{
    protected ObjectResult CreateObjectResult<T>(Result<T> result) => resultMapper.Map(result);
}