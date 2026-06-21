using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;

using Resend;

namespace Lottery.Api.Services.Email;

// todo: build proper email templates
public class EmailSender(IResend resend) : IEmailSender<AppUser>
{
    public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
        => await resend.EmailSendAsync(new()
        {
            From = "noreply@devklick.net",
            To = email,
            Subject = "Confirm the setup of your account",
            TextBody = confirmationLink,
        });

    public async Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
        => await resend.EmailSendAsync(new()
        {
            From = "noreply.devklick.net",
            To = email,
            Subject = "Reset your password",
            TextBody = resetLink
        });

    public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
    {
        throw new NotImplementedException(nameof(SendPasswordResetCodeAsync));
    }
}