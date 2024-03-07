using System.Security.Claims;

using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;

namespace Lottery.Api.Services;

public class UserService(UserManager<AppUser> userManager)
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public Guid? GetUserId(ClaimsPrincipal user)
    {
        var id = _userManager.GetUserId(user);
        return id != null ? Guid.Parse(id) : null;
    }
}