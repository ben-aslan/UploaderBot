using TelegramBotAPI.BotConfiguration.Abstract;
using TelegramBotAPI.BotConfiguration;
using TelegramBotAPI.DependencyResolvers.Abstract;
using TelegramBotAPI.DependencyResolvers;
using TelegramBotAPI.Handle.Abstract;
using TelegramBotAPI.Handle;
using Core.DependencyResolvers;
using Core.Utilities;
using Core.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Core.Utilities.Security.Jwt;
using Microsoft.IdentityModel.Tokens;
using Core.Utilities.Security.Encryption;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Mvc;
using TelegramBotWebApi.Models;

var builder = WebApplication.CreateSlimBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBotConfiguration, BotConfiguration>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

builder.Services.AddSingleton<IDependencyResolver, AutofacDR>();

builder.Services.AddScoped<IHandle, UpdateHandle>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });
builder.Services.AddDependencyResolvers(new ICoreModule[] { new CoreModule() });


var app = builder.Build();

var api = app.MapGroup("/api");

api.MapGet($"/setWebhook", async (string botToken, string url) =>
{
    await new TelegramBotClient(botToken).SetWebhook(url + "/api/update?botId=" + botToken.Split(':')[0]);
    return Results.Ok();
}).WithName("TelegramSetWebhook");

api.MapPost("/update", (
     [FromServices] IHandle _handle,
     NewtonsoftJsonUpdate update,
     [FromQuery] long botId
    ) =>
{
    if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
    {
        if (update.Message!.Type == Telegram.Bot.Types.Enums.MessageType.Video)
            _handle.HandleVideoMessage(update, botId);

        if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Voice)
        {
            _handle.HandleVoiceMessage(update);
        }
        else if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
        {
            if (_handle.HandleKeyboardButton(update))
            {
                return Results.Ok();
            }
            if (update.Message.Entities != null)
            {
                if (update.Message.Entities[0].Type == Telegram.Bot.Types.Enums.MessageEntityType.BotCommand)
                {
                    _handle.HandleCommand(update, botId);
                }
            }
            else
            {
                _handle.HandleMessage(update);
            }
        }
    }
    else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
    {
        //if (await new JoinValidator(_client, new ChannelManager(new EfChannelRepository())).IsJoin(update))
        //{
        if (update.CallbackQuery!.Message!.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
        {
            _handle.HandleCallBackQuery(update, botId);
        }
        //}
    }
    return Results.Ok();
})
.WithName("TelegramWebhook");

app.Run();


[JsonSerializable(typeof(NewtonsoftJsonUpdate))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
