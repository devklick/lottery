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

namespace Lottery.Api;

public class Program
{
    public static void Main(string[] args)
    {
        // TODO: Load values from env rather than user secrets. 
        // User secrets are great, but since the entire workflow is built around docker
        // and docker doesnt natively support user secrets, it seems better to use env vars.
        DotNetEnv.Env.Load("../../.env");

        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(options =>
        {
            var port = int.Parse(Environment.GetEnvironmentVariable("API_PORT") ?? "5000");
            options.ListenAnyIP(port);
        });

        // Add services to the container.

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var port = int.Parse(Environment.GetEnvironmentVariable("UI_PORT") ?? "3000");
                    policy.WithOrigins($"http://localhost:{port}").AllowAnyMethod();
                });
            });
        }

        builder.ConfigureEntityFramework<LotteryDBContext>("API_DB_USER", "API_DB_PASSWORD");
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

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/account/signIn";
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors();
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
        builder.Services.AddSingleton(TimeProvider.System);
    }

    private static void ConfigureAutoMapper(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(cfg => { }, typeof(Program));
    }
}
