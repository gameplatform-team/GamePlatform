using GamePlatform.Domain.Entities;
using Xunit;

namespace GamePlatform.Tests.Domain.Entities;

public class JogoTests
{
    [Fact]
    public void Deve_Criar_Jogo_Com_Dados_Corretos()
    {
        // Arrange
        var titulo = "Super Mario";
        var preco = 59.99m;

        // Act
        var jogo = new Jogo(titulo, preco);

        // Assert
        Assert.Equal(titulo, jogo.Titulo);
        Assert.Equal(preco, jogo.Preco);
        
        Assert.NotEqual(Guid.Empty, jogo.Id);
        Assert.NotEqual(default, jogo.CreatedAt);
        Assert.True(jogo.CreatedAt <= DateTime.UtcNow);
        Assert.Null(jogo.UpdatedAt);
    }
    
    [Fact]
    public void Atualizar_DeveAtualizarDadosDoJogo()
    {
        // Arrange
        var titulo = "Super Mario";
        var preco = 59.99m;
        
        var jogo = new Jogo(titulo, preco);
        
        // Act
        jogo.Atualizar("Super Mario Bros", 89.99m);
        
        // Assert
        Assert.Equal("Super Mario Bros", jogo.Titulo);
        Assert.Equal(89.99m, jogo.Preco);
        Assert.NotNull(jogo.UpdatedAt);
        Assert.True(jogo.UpdatedAt <= DateTime.UtcNow);
    }
}