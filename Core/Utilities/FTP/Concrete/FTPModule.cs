namespace Core.Utilities.FTP.Concrete;

public class FTPModule : IFTPModule
{
    public FTPCredential GetCredential()
    {
        return new FTPCredential() { Host = "ftp://host", UserName = "usrname", Password = "password" };
    }

    public static string Domain => "host";
}
