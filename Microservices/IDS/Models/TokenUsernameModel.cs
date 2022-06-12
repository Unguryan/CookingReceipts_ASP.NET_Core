using Interfaces.ViewModels.User;

namespace IDS.Models
{
    public class TokenUsernameModel : ITokenUsernameModel
    {

        public string Username { get; set; }

        public string Token { get; set; }

    }
}
