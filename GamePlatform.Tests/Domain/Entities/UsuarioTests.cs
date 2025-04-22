using GamePlatform.Domain.Entities;
using Xunit;

namespace GamePlatform.Tests.Domain.Entities;

public class UsuarioTests
{
    [Fact]
    public void Deve_Criar_Usuario_Com_Dados_Corretos()
    {
        var nome = "Maria";
        var email = "maria@email.com";
        var senhaHash = "hashSeguro123";
        var role = "Admin";

        var usuario = new Usuario(nome, email, senhaHash, role);

        Assert.Equal(nome, usuario.Nome);
        Assert.Equal(email, usuario.Email);
        Assert.Equal(senhaHash, usuario.SenhaHash);
        Assert.Equal(role, usuario.Role);
    }

    [Fact]
    public void Deve_Criar_Usuario_Com_Role_Padrao_Usuario()
    {
        var nome = "João";
        var email = "joao@email.com";
        var senhaHash = "hash456";

        var usuario = new Usuario(nome, email, senhaHash); 
   
        Assert.Equal("Usuario", usuario.Role);
    }
}
