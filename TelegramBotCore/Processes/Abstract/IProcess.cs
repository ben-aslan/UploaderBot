using Telegram.Bot.Types;

namespace TelegramBotCore.Processes.Abstract;

public interface IProcess
{
    Task Execute(Update update);
}
