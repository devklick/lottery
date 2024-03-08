using System.Security.Claims;

using Lottery.Api.Models.Common;
using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;

namespace Lottery.Api.Services;

public class UserService(UserManager<AppUser> userManager)
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public Result<Guid> GetUserId(ClaimsPrincipal user)
    {
        var id = _userManager.GetUserId(user);

        if (id == null)
        {
            return new Result<Guid>
            {
                Status = ResultStatus.NotAuthenticated,
                Errors = [new() { Message = "Unable to locate user" }]
            };
        }

        return new Result<Guid>
        {
            Status = ResultStatus.Ok,
            Value = Guid.Parse(id),
        };
    }
}