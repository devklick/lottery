using Lottery.Api.Mappings;
using Lottery.Api.Services;
using Lottery.Common.Models;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Lottery.Api.Filters.Auth;

/// <summary>
/// Filters requests to ensure a recently-authenticated session is present. 
/// If not, a suitable response is returned to the caller and the request does not continue.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireRecentAuthAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userService = context.HttpContext.RequestServices.GetRequiredService<UserService>();
        var mapper = context.HttpContext.RequestServices.GetRequiredService<ResultMapper>();

        var userResult = await userService.GetCurrentUser();
        if (!userResult.Success)
        {
            context.Result = mapper.Map(userResult);
            return;
        }

        var recentAuth = await userService.HasRecentAuth(userResult.Value);

        if (recentAuth) return;

        context.Result = mapper.Map(Result<object>.Error(
            ResultStatus.NotAuthorized,
            new Message
            {
                Code = MessageCode.RecentAuthRequired,
                Value = "This action requires a session that was recently authenticated"
            }));
    }
}