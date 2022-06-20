namespace Interfaces.ViewModels.User
{
    public interface ILoginUserModel
    {

        string UserName { get; set; }

        string Password { get; set; }

        bool RememberMe { get; set; }

    }
}