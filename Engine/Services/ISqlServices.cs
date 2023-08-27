namespace Engine.Services;

public interface ISqlServices
{
    bool ChangePassword(string username, string password);
}