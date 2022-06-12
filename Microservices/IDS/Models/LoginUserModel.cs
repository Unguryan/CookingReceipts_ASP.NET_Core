using Interfaces.ViewModels.User;

namespace IDS.Models
{
    public class LoginUserModel : ILoginUserModel
    {

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
