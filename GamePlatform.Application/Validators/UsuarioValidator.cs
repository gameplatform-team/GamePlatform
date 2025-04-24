using System.Text.RegularExpressions;

namespace GamePlatform.Application.Validators;

public static class UsuarioValidator
{
    public static bool ValidarEmail(string email)
    {
        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$");
        return regex.IsMatch(email);
    }

    public static bool ValidarSenha(string senha)
    {
        var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
        return regex.IsMatch(senha);
    }
}