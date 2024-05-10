using System.Security.Cryptography;

using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;

namespace Lottery.Api.Utilities;

public class Hasher(PasswordHasher<AppUser> passwordHasher)
{
    private readonly PasswordHasher<AppUser> _passwordHasher = passwordHasher;
    private readonly string _alphabet = string.Join("",
        Enumerable.Range('a', 26).Select(x => (char)x)
        .Concat(Enumerable.Range('A', 26).Select(x => (char)x))
        .Concat(Enumerable.Range(1, 9).Select(x => x.ToString().First())));

    public string HashPassword(AppUser user, string password)
        => _passwordHasher.HashPassword(user, password);

    public string GetString(int length)
        => RandomNumberGenerator.GetString(_alphabet, length);
}