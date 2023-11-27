using System.Security.Cryptography;
using System.Text;

namespace CodeFighters.WebApi.Utilities
{
    public class Encryption
    {
        public static string PBKDF2(string password, string salt)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000, hashAlgorithm);
            var hash = pbkdf2.GetBytes(20);
            return Convert.ToBase64String(hash);
        }
    }
}
