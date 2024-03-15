using Microsoft.AspNetCore.Identity;

using Lottery.Api.Services;
using Lottery.Api.Repositories;

using Lottery.DB.Context;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Extensions;
using Lottery.Api.Services.Options;

namespace Lottery.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:3000").AllowAnyMethod();
                });
            });
        }

        builder.ConfigureEntityFramework<LotteryDBContext>();
        ConfigureIdentity(builder);
        ConfigureAutoMapper(builder);
        ConfigureServices(builder);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<UserServiceOptions>(
            builder.Configuration.GetSection(UserServiceOptions.Name));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors();
        }

        app.UseHttpsRedirection();

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

        builder.Services.AddScoped<GameRepository>();
        builder.Services.AddScoped<EntryRepository>();
    }

    private static void ConfigureAutoMapper(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program));
    }
}
