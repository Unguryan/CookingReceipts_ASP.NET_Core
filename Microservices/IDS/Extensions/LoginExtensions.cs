using Microsoft.AspNetCore.DataProtection;

namespace IDS.Extensions
{
    public static class LoginExtensions
    {
        private const string keyPass = "Key_Pass_To_Ecrypte";

        public static string EncryptePassword(this string text, IDataProtectionProvider provider)
        {
            var protector = provider.CreateProtector(keyPass);
            return protector.Protect(text);
        }

        public static string DecryptePassword(this string text, IDataProtectionProvider provider)
        {
            var protector = provider.CreateProtector(keyPass);
            return protector.Unprotect(text);
        }

    }
}
