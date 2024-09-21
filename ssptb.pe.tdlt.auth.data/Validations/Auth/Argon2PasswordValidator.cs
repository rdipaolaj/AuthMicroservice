using Konscious.Security.Cryptography;
using System.Text;

namespace ssptb.pe.tdlt.auth.data.Validations.Auth;
public class Argon2PasswordValidator : IPasswordValidator
{
    public bool ValidatePassword(string inputPassword, string storedHashedPassword, string salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(inputPassword))
        {
            Salt = Convert.FromBase64String(salt),
            DegreeOfParallelism = 8,
            MemorySize = 8192,
            Iterations = 4
        };

        var hashedInputPassword = Convert.ToBase64String(argon2.GetBytes(16));
        return hashedInputPassword == storedHashedPassword;
    }
}
