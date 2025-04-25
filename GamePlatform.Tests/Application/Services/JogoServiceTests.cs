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
        var jogoDto = new CadastrarJogoDto { Titulo = "Novo Jogo", Preco = 99.99m };

        _jogoRepoMock.Setup(x => x.ExisteTituloAsync(jogoDto.Titulo)).ReturnsAsync(false);
        _jogoRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<Jogo>())).Returns(Task.CompletedTask);

        var resultado = await _jogoService.CadastrarAsync(jogoDto);

        _jogoRepoMock.Verify(x => x.ExisteTituloAsync(jogoDto.Titulo), Times.Once);
        _jogoRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<Jogo>()), Times.Once);
        Assert.True(resultado.Sucesso);
        Assert.Equal("Jogo cadastrado com sucesso", resultado.Mensagem);
    }

    [Fact]
    public async Task CadastrarAsync_DeveRetornarErro_QuandoJogoJaExiste()
    {
        var jogoDto = new CadastrarJogoDto { Titulo = "Jogo Existente", Preco = 99.99m };

        _jogoRepoMock.Setup(x => x.ExisteTituloAsync(jogoDto.Titulo)).ReturnsAsync(true);

        var resultado = await _jogoService.CadastrarAsync(jogoDto);

        _jogoRepoMock.Verify(x => x.ExisteTituloAsync(jogoDto.Titulo), Times.Once);
        _jogoRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<Jogo>()), Times.Never);
        Assert.False(resultado.Sucesso);
        Assert.Equal("Jogo já cadastrado", resultado.Mensagem);
    }
}