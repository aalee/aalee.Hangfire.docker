namespace Engine.Services;

public class SqlServices : ISqlServices
{
    public SqlServices()
    {
        
    }

    public bool ChangePassword(string username, string password)
    {
        Console.WriteLine($"Changing password for {username}");
        return true;
    }
}