using Microsoft.AspNetCore.Identity;

using Lottery.Api.Services;

using Lottery.DB.Context;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Extensions;
using Lottery.Api.Services.Options;
using Lottery.Api.Repositories.Game;
using Microsoft.AspNetCore.Authentication.Cookies;
using Lottery.Api.Repositories.Entry;
using Lottery.Resulting;
using Lottery.Api.Utilities;
using Lottery.Api.Repositories.User;
using DotNetEnv.Configuration;
using Resend;
using Lottery.Common.Helpers;
using Lottery.Api.Services.Email;
using Lottery.Api.Mappings;

namespace Lottery.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddDotNetEnv("../../.env");

        builder.WebHost.ConfigureKestrel(options =>
        {
            var port = int.Parse(Environment.GetEnvironmentVariable("LOTTERY_API_PORT") ?? "5000");
            options.ListenAnyIP(port);
        });

        // Add services to the container.

        builder.ConfigureEntityFramework<LotteryDBContext>("LOTTERY_API_DB_USER", "LOTTERY_API_DB_PASSWORD");
        ConfigureIdentity(builder);
        ConfigureAutoMapper(builder);
        ConfigureServices(builder);

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddLogging();

        builder.Services.Configure<UserServiceOptions>(
            builder.Configuration.GetSection(UserServiceOptions.Name));

        builder.Services.PostConfigure<UserServiceOptions>(options =>
        {
            options.EmailConfirmationDomain =
                $"http://localhost:{Env.GetRequiredEnvVar("LOTTERY_UI_PORT")}";
        });

        builder.Services.AddTransient<IResend, ResendClient>();

        builder.Services.AddHttpClient<ResendClient>();
        builder.Services.Configure<ResendClientOptions>(o =>
            o.ApiToken = Env.GetRequiredEnvVar("EMAIL_API_KEY"));
        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services.AddScoped<ResultMapper>();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/account/signIn";
            // TODO: Implement a sliding expiration to keep auth fresh
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void ConfigureIdentity(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<LotteryDBContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(
            builder.Configuration.GetSection("IdentityOptions"));

        builder.Services.AddTransient<PasswordHasher<AppUser>>();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<GameService>();
        builder.Services.AddScoped<EntryService>();
        builder.Services.AddScoped<ResultService>();
        builder.Services.AddScoped<Hasher>();

        builder.Services.AddScoped<GameRepository>();
        builder.Services.AddScoped<EntryRepository>();
        builder.Services.AddScoped<ResultRepository>();
        builder.Services.AddScoped<UserRepository>();
    }

    private static void ConfigureAutoMapper(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(cfg => { }, typeof(Program));
    }
}
