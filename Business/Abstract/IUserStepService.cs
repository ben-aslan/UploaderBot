using Entities.Concrete;

namespace Business.Abstract;

public interface IUserStepService
{
    UserStep Get(long chatId);
}
