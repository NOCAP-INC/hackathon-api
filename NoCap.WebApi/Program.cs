using System.Configuration;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NoCap.Configs;
using NoCap.Data;
using NoCap.Handlers;
using NoCap.Managers;
using NoCap.Service;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<Config>(provider => BindConfiguration(provider));

builder.Services.AddDbContext<IdentityContext>();
builder.Services.AddSingleton<SMTPConfig>();

builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ReportService>();
builder.Services.AddControllers();
builder.Services.AddTransient<RegisterUserHandler>();

builder.Services.AddTransient<LoginUserHandler>();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddMediatR(typeof(EmailHandler).Assembly);
builder.Services.AddAuthorization();
builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<IdentityContext>();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    Seed(serviceProvider);
}

var mediatr = new ServiceCollection();
builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "No Cap",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    // using System.Reflection;
    
});

var app = builder.Build();


app.UseSwagger();


app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllers();

app.UseCors(builder => builder
    .WithOrigins()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.Run();

Config? BindConfiguration(IServiceProvider provider)
{
    var envName = builder.Environment.EnvironmentName;

    var config = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.{envName}.json")
        .Build();

    var configService = config.Get<Config>();
    return configService;
}
 static async Task Seed(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }
}

