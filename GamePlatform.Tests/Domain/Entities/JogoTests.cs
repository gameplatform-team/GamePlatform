using GamePlatform.Domain.Entities;
using Xunit;

namespace GamePlatform.Tests.Domain.Entities;

public class JogoTests
{
    [Fact]
    public void Deve_Criar_Jogo_Com_Dados_Corretos()
    {
        var titulo = "Super Mario";
        var preco = 59.99m;

        var jogo = new Jogo(titulo, preco);

        Assert.Equal(titulo, jogo.Titulo);
        Assert.Equal(preco, jogo.Preco);
        
        // Testing BaseEntity properties
        Assert.NotEqual(Guid.Empty, jogo.Id);
        Assert.NotEqual(default, jogo.CreatedAt);
        Assert.True(jogo.CreatedAt <= DateTime.UtcNow);
        Assert.Null(jogo.UpdatedAt);
    }
}