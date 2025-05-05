using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Usuario;
using GamePlatform.Application.Services;
using GamePlatform.Domain.Entities;
using GamePlatform.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GamePlatform.Tests.Application.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly Mock<ILogger<UsuarioService>> _logger;
    private readonly UsuarioService _usuarioService;

    public UsuarioServiceTests()
    {
        _usuarioRepoMock = new Mock<IUsuarioRepository>();
        _configMock = new Mock<IConfiguration>();
        _logger = new Mock<ILogger<UsuarioService>>();
        _usuarioService = new UsuarioService(_usuarioRepoMock.Object, _configMock.Object, _logger.Object);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRetornarErro_SeEmailInvalido()
    {
        var dto = new RegistrarUsuarioDto { Email = "emailinvalido", Senha = "Senha123!", Nome = "João" };

        var resultado = await _usuarioService.RegistrarAsync(dto);

        Assert.False(resultado.Sucesso);
        Assert.Equal("Formato de e-mail inválido.", resultado.Mensagem);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRetornarErro_SeSenhaInvalida()
    {
        var dto = new RegistrarUsuarioDto { Email = "teste@email.com", Senha = "abc", Nome = "João" };

        var resultado = await _usuarioService.RegistrarAsync(dto);

        Assert.False(resultado.Sucesso);
        Assert.Equal("A senha deve ter no mínimo 8 caracteres e conter letras, números e caracteres especiais.", resultado.Mensagem);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRetornarErro_SeEmailJaExiste()
    {
        var dto = new RegistrarUsuarioDto { Email = "teste@email.com", Senha = "Senha123!", Nome = "João" };

        _usuarioRepoMock.Setup(x => x.ExisteEmailAsync(dto.Email)).ReturnsAsync(true);

        var resultado = await _usuarioService.RegistrarAsync(dto);

        Assert.False(resultado.Sucesso);
        Assert.Equal("E-mail já cadastrado.", resultado.Mensagem);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRegistrarUsuario_ComSucesso()
    {
        var dto = new RegistrarUsuarioDto { Email = "teste@email.com", Senha = "Senha123!", Nome = "João" };

        _usuarioRepoMock.Setup(x => x.ExisteEmailAsync(dto.Email)).ReturnsAsync(false);
        _usuarioRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
        _usuarioRepoMock.Setup(x => x.SalvarAsync()).Returns(Task.CompletedTask);

        var resultado = await _usuarioService.RegistrarAsync(dto);

        Assert.True(resultado.Sucesso);
        Assert.Equal("Usuário registrado com sucesso.", resultado.Mensagem);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarUsuario_SeExistir()
    {
        var id = Guid.NewGuid();

        var usuario = new Usuario("João", "joao@email.com", "senhaHash");
        usuario.Id = id;

        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync(usuario);

        var resultado = await _usuarioService.ObterPorIdAsync(id);

        Assert.NotNull(resultado);
        Assert.Equal(id, resultado!.Id);
        Assert.Equal("João", resultado.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_SeNaoEncontrado()
    {
        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);

        var resultado = await _usuarioService.ObterPorIdAsync(Guid.NewGuid());

        Assert.Null(resultado);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarDados_SeUsuarioExistir()
    {
        var id = Guid.NewGuid();

        var usuario = new Usuario("João", "joao@email.com", "senhaHash")
        {
            Id = id
        };

        var dto = new AtualizarUsuarioDto
        {
            Nome = "Novo Nome",
            Email = "novo@email.com",
            NovaSenha = "NovaSenha123!"
        };

        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync(usuario);

        var resultado = await _usuarioService.AtualizarAsync(id, dto);

        Assert.True(resultado.Sucesso);
        _usuarioRepoMock.Verify(r => r.SalvarAsync(), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveRetornarFalse_SeUsuarioNaoExistir()
    {
        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);

        var resultado = await _usuarioService.AtualizarAsync(Guid.NewGuid(), new AtualizarUsuarioDto());

        Assert.False(resultado.Sucesso);
    }

    [Fact]
    public async Task ExcluirAsync_DeveRemoverUsuario_SeExistir()
    {
        var usuario = new Usuario("João", "joao@email.com", "senhaHash");
        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(usuario);

        var resultado = await _usuarioService.ExcluirAsync(Guid.NewGuid());

        Assert.True(resultado.Sucesso);
        _usuarioRepoMock.Verify(r => r.Remover(usuario), Times.Once);
        _usuarioRepoMock.Verify(r => r.SalvarAsync(), Times.Once);
    }

    [Fact]
    public async Task ExcluirAsync_DeveRetornarFalse_SeNaoEncontrado()
    {
        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);

        var resultado = await _usuarioService.ExcluirAsync(Guid.NewGuid());

        Assert.False(resultado.Sucesso);
    }

    [Fact]
    public async Task ListarTodosAsync_DeveRetornarListaDeUsuarios()
    {
        var usuarios = new List<Usuario>
        {
            new Usuario("João", "joao@email.com", "hash"),
            new Usuario("Maria", "maria@email.com", "hash")
        };

        _usuarioRepoMock.Setup(r => r.ListarTodosAsync()).ReturnsAsync(usuarios);

        var resultado = await _usuarioService.ListarTodosAsync();

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task PromoverParaAdminAsync_DevePromover_SeUsuarioExistir()
    {
        var usuario = new Usuario("João", "joao@email.com", "hash");
        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(usuario);

        var resultado = await _usuarioService.PromoverParaAdminAsync(Guid.NewGuid());

        Assert.True(resultado.Sucesso);
        _usuarioRepoMock.Verify(r => r.SalvarAsync(), Times.Once);
    }

    [Fact]
    public async Task PromoverParaAdminAsync_DeveRetornarFalse_SeNaoEncontrado()
    {
        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);

        var resultado = await _usuarioService.PromoverParaAdminAsync(Guid.NewGuid());

        Assert.False(resultado.Sucesso);
    }
}
