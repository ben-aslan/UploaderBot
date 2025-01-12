using Business.Abstract;
using DataAccess.Abstract;

namespace Business.Concrete;

public class VideoManager : IVideoService
{
    IVideoDal _videoDal;

    public VideoManager(IVideoDal videoDal)
    {
        _videoDal = videoDal;
    }
}
