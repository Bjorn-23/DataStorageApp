using System.Security.Cryptography;
using System.Text;

namespace Business.Utils;

public static class PasswordGenerator
{

    public static (string Password, string SecurityKey) GenerateSecurePasswordAndKey(string password)
    {
        using var hmac = new HMACSHA256();
        var securityKey = Convert.ToBase64String(hmac.Key);
        var generatedPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return (generatedPassword, securityKey);
    }

    public static bool VerifyPassword(string password, string storedSecurityKey, string storedPassword)
    {
        // Convert the stored security key from Base64 string to byte array
        byte[] keyBytes = Convert.FromBase64String(storedSecurityKey);

        // Create an instance of HMACSHA256 with the stored key
        using (var hmac = new HMACSHA256(keyBytes))
        {
            // Compute the hash of the input password
            var computedPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

            // Compare the computed hash with the stored hash
            return computedPassword == storedPassword;
        }
    }

    //public static string GenerateSecurePassword(string password)
    //{

    //    using var hmac = new HMACSHA256();
    //    var securityKey = Convert.ToBase64String(hmac.Key);
    //    var generatedPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

    //    return (generatedPassword);
    //}
}
