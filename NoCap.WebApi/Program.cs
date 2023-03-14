using System.Configuration;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using NoCap.Configs;
using NoCap.Handlers;
using NoCap.Managers;
using NoCap.Service;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<Config>(provider => BindConfiguration(provider));

builder.Services.AddDbContext<IdentityDbContext>();

builder.Services.AddDefaultIdentity<User>()
    .AddEntityFrameworkStores<IdentityDbContext>();
builder.Services.Configure<SMTPConfig>(builder.Configuration.GetSection(nameof(SMTPConfig)));
builder.Services.AddSingleton<SMTPConfig>();

builder.Services.AddTransient<CheckCodeHandler>();
builder.Services.AddTransient<EmailService>();

builder.Services.AddControllers();
builder.Services.AddTransient<RegisterUserHandler>();

builder.Services.AddTransient<AuthManager>();
builder.Services.AddTransient<LoginUserHandler>();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddMediatR(typeof(EmailHandler).Assembly);

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.SignInScheme = IdentityConstants.ExternalScheme;
    });
builder.Services.AddAuthorization();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllers();

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
