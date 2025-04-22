using GamePlatform.Application.DTOs;
using GamePlatform.Application.Services;
using GamePlatform.Domain.Entities;
using GamePlatform.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace GamePlatform.Tests.Application.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly UsuarioService _usuarioService;

    public UsuarioServiceTests()
    {
        _usuarioRepoMock = new Mock<IUsuarioRepository>();
        _configMock = new Mock<IConfiguration>();

        _usuarioService = new UsuarioService(_usuarioRepoMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRetornarErro_SeEmailInvalido()
    {
        var dto = new RegistrarUsuarioDto { Email = "emailinvalido", Senha = "Senha123!", Nome = "João" };

        var resultado = await _usuarioService.RegistrarAsync(dto);

        Assert.False(resultado.sucesso);
        Assert.Equal("Formato de e-mail inválido.", resultado.mensagem);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRetornarErro_SeSenhaInvalida()
    {
        var dto = new RegistrarUsuarioDto { Email = "teste@email.com", Senha = "abc", Nome = "João" };

        var resultado = await _usuarioService.RegistrarAsync(dto);

        Assert.False(resultado.sucesso);
        Assert.Equal("A senha deve ter no mínimo 8 caracteres e conter letras, números e caracteres especiais.", resultado.mensagem);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRetornarErro_SeEmailJaExiste()
    {
        var dto = new RegistrarUsuarioDto { Email = "teste@email.com", Senha = "Senha123!", Nome = "João" };

        _usuarioRepoMock.Setup(x => x.ExisteEmailAsync(dto.Email)).ReturnsAsync(true);

        var resultado = await _usuarioService.RegistrarAsync(dto);

        Assert.False(resultado.sucesso);
        Assert.Equal("E-mail já cadastrado.", resultado.mensagem);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRegistrarUsuario_ComSucesso()
    {
        var dto = new RegistrarUsuarioDto { Email = "teste@email.com", Senha = "Senha123!", Nome = "João" };

        _usuarioRepoMock.Setup(x => x.ExisteEmailAsync(dto.Email)).ReturnsAsync(false);
        _usuarioRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
        _usuarioRepoMock.Setup(x => x.SalvarAsync()).Returns(Task.CompletedTask);

        var resultado = await _usuarioService.RegistrarAsync(dto);

        Assert.True(resultado.sucesso);
        Assert.Equal("Usuário registrado com sucesso.", resultado.mensagem);
    }
}
