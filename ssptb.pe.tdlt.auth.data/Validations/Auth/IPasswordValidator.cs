namespace ssptb.pe.tdlt.auth.data.Validations.Auth;
public interface IPasswordValidator
{
    bool ValidatePassword(string inputPassword, string storedHashedPassword, string salt);
}
