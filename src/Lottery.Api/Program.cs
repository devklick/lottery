using Microsoft.EntityFrameworkCore;

using Lottery.Repository.Configuration;
using Lottery.Repository.Context;
using Lottery.Repository.Entities.Dbo;
using Microsoft.AspNetCore.Identity;
using Lottery.Repository.Entities.Idt;
using Lottery.Api.Services;
using Lottery.Repository;

namespace Lottery.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        ConfigureEntityFramework(builder);
        ConfigureAutoMapper(builder);
        ConfigureServices(builder);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
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

    private static void ConfigureEntityFramework(WebApplicationBuilder builder)
    {
        builder.Services.Configure<EFMigrationSettings>(
            builder.Configuration.GetSection(nameof(EFMigrationSettings)));

        builder.Services.AddDbContext<LotteryDBContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Default")
                ?? throw new Exception("No default connection string found");

            var dbPassword = builder.Configuration["ConnectionStrings:Default:Password"];

            if (!string.IsNullOrWhiteSpace(dbPassword))
            {
                connectionString = connectionString.TrimEnd();
                connectionString += $";Password={dbPassword};";
            }
            options.UseNpgsql(connectionString, sqlOps =>
            {
                var settings = builder.Configuration.GetSection(nameof(EFMigrationSettings)).Get<EFMigrationSettings>()
                    ?? throw new Exception("No EFMigrationSettings found");

                sqlOps.MigrationsHistoryTable(tableName: settings.TableName, schema: settings.SchemaName);
            });
        });

        builder.Services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<LotteryDBContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(
            builder.Configuration.GetSection("IdentityOptions"));
    }
}
