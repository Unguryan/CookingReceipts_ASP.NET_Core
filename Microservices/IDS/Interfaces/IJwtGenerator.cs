using Interfaces.Models;

namespace IDS.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(IUserIDS user, bool rememberMe);
    }
}
