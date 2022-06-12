using System.Threading.Tasks;

namespace IDS.Interfaces
{
    public interface ITokenManager
    {
        Task<bool> VerifyToken(string token);

        Task<string> GetUserRoles(string token);
    }
}