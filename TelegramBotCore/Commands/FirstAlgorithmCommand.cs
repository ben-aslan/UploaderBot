using Business.Abstract;
using Business.LangService.Abstract;
using Entities.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotCore.Commands.Abstract;

namespace TelegramBotCore.Commands;

[Command(ChatType = ChatType.Supergroup, Name = "/firstalgorithm")]
public class FirstAlgorithmCommand : Command, ICommand
{
    IUserService _userService;
    IUserStepService _userStepService;

    public FirstAlgorithmCommand(IUserService userService, IUserStepService userStepService)
    {
        _userService = userService;
        _userStepService = userStepService;
    }

    public void Execute(Update update, ITelegramBotClient _client = null!, IMessageService _message = null!)
    {
        _userStepService.Set(update.Message!.From!.Id, EStep.GroupVideoFirstAlgorithm, EStepIndex.Home);

        if (_userService.HaveClaim(update.Message!.From!.Id, EOperationClaim.Admin))
            _client.SendTextMessageAsync(update.Message.Chat.Id, _message.Get(EMessage.UploadVideo1));
        return;
    }
}
