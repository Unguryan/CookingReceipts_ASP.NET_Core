using Interfaces.ViewModels.User;

namespace IDS.Models
{
    public class RegisterUserModel : IRegisterUserModel
    {

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

    }
}
