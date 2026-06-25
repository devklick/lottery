using Lottery.DB.Entities.Idt;

using Resend;

namespace Lottery.Api.Services.Email;

// todo: build proper email templates
public class EmailService(IResend resend) : IEmailService
{
    const string NO_REPLY_EMAIL = "noreply@devklick.net";
    public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
        => await resend.EmailSendAsync(new()
        {
            From = NO_REPLY_EMAIL,
            To = email,
            Subject = "Confirm the setup of your account",
            TextBody = confirmationLink,
        });

    public async Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
        => await resend.EmailSendAsync(new()
        {
            From = NO_REPLY_EMAIL,
            To = email,
            Subject = "Reset your password",
            TextBody = resetLink
        });



    public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
    {
        throw new NotImplementedException(nameof(SendPasswordResetCodeAsync));
    }

    public async Task SendAccountDeletionConfirmation(string email)
        => await resend.EmailSendAsync(new()
        {
            From = NO_REPLY_EMAIL,
            To = email,
            Subject = "Your account has been deleted",
            TextBody =
                "You have recently requested your account to be deleted.\n"
                + "This email confirms that your account has successfully been deleted.\n"
                + "You will no longer be able to access our site.\n"
                + "You will receive no further communication from us.\n"
        });
}