using Business.LangService.Abstract;
using Entities.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotCore.VideoMessages.Abstract;

namespace TelegramBotCore.VideoMessages;

[VideoMessage(StepId = (int)EStep.Video, StepIndexId = (int)EStepIndex.UploadVideoWithSameFileId)]
public class UploadVideoWithSameFileId : VideoMessage, IVideoMessage
{
    public void Execute(Update update, ITelegramBotClient _client = null, IMessageService _message = null)
    {
        throw new NotImplementedException();
    }
}
