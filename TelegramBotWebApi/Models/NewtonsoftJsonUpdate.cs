﻿using Newtonsoft.Json;
using System.Reflection;
using Telegram.Bot.Types;

namespace TelegramBotWebApi.Models;

public class NewtonsoftJsonUpdate:Update
{
    public static async ValueTask<NewtonsoftJsonUpdate?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        using var streamReader = new StreamReader(context.Request.Body);
        var updateJsonString = await streamReader.ReadToEndAsync();

        return JsonConvert.DeserializeObject<NewtonsoftJsonUpdate>(updateJsonString);
    }
}
