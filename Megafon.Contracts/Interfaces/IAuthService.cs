namespace Megafon.Contracts.Interfaces;

public interface IAuthService
{
    public bool Challange(string claim);
    public void Login(string username, string password);
    public void Logout();
    public bool IsLoggedIn();
}
