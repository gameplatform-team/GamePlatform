using GamePlatform.Application.DTOs.Jogo;
using GamePlatform.Application.Services;
using GamePlatform.Domain.Entities;
using GamePlatform.Domain.Interfaces;
using Moq;
using Xunit;

namespace GamePlatform.Tests.Application.Services;

public class JogoServiceTests
{
    private readonly Mock<IJogoRepository> _jogoRepoMock;
    private readonly JogoService _jogoService;

    public JogoServiceTests()
    {
        _jogoRepoMock = new Mock<IJogoRepository>();
        _jogoService = new JogoService(_jogoRepoMock.Object);
    }

    [Fact]
    public async Task CadastrarAsync_DeveRetornarSucesso_QuandoJogoNaoExiste()
    {
        // Arrange
        var jogoDto = new CadastrarJogoDto { Titulo = "Novo Jogo", Preco = 99.99m };

        _jogoRepoMock.Setup(x => x.ExisteTituloAsync(jogoDto.Titulo)).ReturnsAsync(false);
        _jogoRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

        // Act
        var resultado = await _jogoService.CadastrarAsync(jogoDto);

        // Assert
        _jogoRepoMock.Verify(x => x.ExisteTituloAsync(jogoDto.Titulo), Times.Once);
        _jogoRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<Jogo>()), Times.Once);
        Assert.True(resultado.Sucesso);
        Assert.Equal("Jogo cadastrado com sucesso", resultado.Mensagem);
    }

    [Fact]
    public async Task CadastrarAsync_DeveRetornarErro_QuandoJogoJaExiste()
    {
        // Arrange
        var jogoDto = new CadastrarJogoDto { Titulo = "Jogo Existente", Preco = 99.99m };

        _jogoRepoMock.Setup(x => x.ExisteTituloAsync(jogoDto.Titulo)).ReturnsAsync(true);

        // Act
        var resultado = await _jogoService.CadastrarAsync(jogoDto);

        // Assert
        _jogoRepoMock.Verify(x => x.ExisteTituloAsync(jogoDto.Titulo), Times.Once);
        _jogoRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<Jogo>()), Times.Never);
        Assert.False(resultado.Sucesso);
        Assert.Equal("Jogo já cadastrado", resultado.Mensagem);
    }
    
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarJogo_QuandoExistir()
    {
        // Arrange
        var jogo = new Jogo("Jogo Existente", 99.99m);
        
        _jogoRepoMock.Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(jogo);
        
        // Act
        var resultado = await _jogoService.ObterPorIdAsync(Guid.NewGuid());
        
        // Assert
        _jogoRepoMock.Verify(x => x.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);
        Assert.Equivalent(jogo, resultado.Jogo, true);
        Assert.True(resultado.Sucesso);
        Assert.Equal("Jogo encontrado", resultado.Mensagem);
    }
    
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarErro_QuandoNaoExistir()
    {
        // Arrange
        _jogoRepoMock.Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Jogo?)null);
        
        // Act
        var resultado = await _jogoService.ObterPorIdAsync(Guid.NewGuid());
        
        // Assert
        _jogoRepoMock.Verify(x => x.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);
        Assert.Null(resultado.Jogo);
        Assert.False(resultado.Sucesso);
        Assert.Equal("Jogo não encontrado", resultado.Mensagem);
    }
}