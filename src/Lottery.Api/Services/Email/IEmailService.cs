using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;

namespace Lottery.Api.Services.Email;

public interface IEmailService : IEmailSender<AppUser>
{
    Task SendAccountDeletionConfirmation(string email);
}