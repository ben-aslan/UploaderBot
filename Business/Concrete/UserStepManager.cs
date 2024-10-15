using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class UserStepManager : IUserStepService
{
    IUserStepDal _userStepDal;

    public UserStepManager(IUserStepDal userStepDal)
    {
        _userStepDal = userStepDal;
    }

    public UserStep Get(long chatId)
    {
        return _userStepDal.First(x => x.User.ChatId == chatId);
    }
}
