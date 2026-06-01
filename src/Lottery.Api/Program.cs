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
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddDotNetEnv("../../.env");

        builder.WebHost.ConfigureKestrel(options =>
        {
            var port = int.Parse(Environment.GetEnvironmentVariable("API_PORT") ?? "5000");
            options.ListenAnyIP(port);
        });

        // Add services to the container.

        if (builder.Environment.IsDevelopment())
        {
            // builder.Services.AddCors(options =>
            // {
            //     options.AddDefaultPolicy(policy =>
            //     {
            //         var port = int.Parse(Environment.GetEnvironmentVariable("UI_PORT") ?? "3000");
            //         policy.WithOrigins($"http://localhost:{port}").AllowAnyMethod();
            //     });
            // });
        }

        builder.ConfigureEntityFramework<LotteryDBContext>("API_DB_USER", "API_DB_PASSWORD");
        ConfigureIdentity(builder);
        ConfigureAutoMapper(builder);
        ConfigureServices(builder);

        builder.Services.AddControllers();
        // builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = context =>
        // {
        //     var problemDetails = new ProblemDetails
        //     {
        //         Status = StatusCodes.Status400BadRequest,
        //         Title = "Validation failed"
        //     };

        //     problemDetails.Extensions["errors"] = context.ModelState
        //         .Where(x => x.Value?.Errors.Count > 0)
        //         .ToDictionary(
        //             kvp => kvp.Key,
        //             kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
        //         );
        //     return new BadRequestObjectResult(problemDetails);
        // });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddLogging();

        // builder.Services.Configure<ApiBehaviorOptions>(options =>
        // {
        //     options.SuppressModelStateInvalidFilter = true;
        // });


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
            // app.UseCors();
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
