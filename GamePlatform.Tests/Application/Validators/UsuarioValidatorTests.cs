using GamePlatform.Application.Validators;
using Xunit;

namespace GamePlatform.Tests.Application.Validators;

public class UsuarioValidatorTests
{
    [Theory]
    [InlineData("teste@dominio.com", true)]
    [InlineData("teste@dominio", false)]
    [InlineData("testedominio.com", false)]
    [InlineData("teste@dominio..com", false)]
    public void ValidarEmail_ValidaCorretamente(string email, bool esperado)
    {
        var resultado = UsuarioValidator.ValidarEmail(email);

        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("Senha@123", true)]
    [InlineData("senha123", false)]
    [InlineData("12345678", false)]
    [InlineData("Senha123", false)]
    public void ValidarSenha_ValidaCorretamente(string senha, bool esperado)
    {
        var resultado = UsuarioValidator.ValidarSenha(senha);

        Assert.Equal(esperado, resultado);
    }
}
