using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Usuario;
using GamePlatform.Application.Interfaces.Serivces;
using GamePlatform.Application.Validators;
using GamePlatform.Domain.Entities;
using GamePlatform.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamePlatform.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _config;
    private readonly ILogger<UsuarioService> _logger;

    public UsuarioService(IUsuarioRepository usuarioRepository, IConfiguration config, ILogger<UsuarioService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _config = config;
        _logger = logger;
    }

    public async Task<UsuarioDto?> ObterPorIdAsync(Guid id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);
        return usuario is null ? null : new UsuarioDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Role);
    }

    public async Task<BaseResponseDto> AtualizarAsync(Guid id, AtualizarUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario == null)
            {
                _logger.LogWarning("Tentativa de atualizar usuário com ID inválido: {UserId}", id);
                return new BaseResponseDto(false, "Usuário não encontrado.");
            }

            if (!string.IsNullOrEmpty(dto.Email) && await _usuarioRepository.EmailJaExisteAsync(dto.Email, id))
            {
                _logger.LogWarning("Email em uso por outro usuário: {Email}", dto.Email);
                return new BaseResponseDto(false, "Este e-mail já está em uso por outro usuário.");
            }

            usuario.Atualizar(dto.Nome, dto.Email, dto.NovaSenha);

            await _usuarioRepository.SalvarAsync();

            _logger.LogInformation("Usuário atualizado com sucesso: {UserId}", usuario.Id);

            return new BaseResponseDto(true, "Usuário atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário {UserId}", id);
            return new BaseResponseDto(false, "Erro ao atualizar o usuário.");
        }
    }

    public async Task<BaseResponseDto> ExcluirAsync(Guid id)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario is null)
            {
                _logger.LogWarning("Tentativa de exclusão de usuário inexistente: {UserId}", id);
                return new BaseResponseDto(false, "Usuário não encontrado.");
            }

            _usuarioRepository.Remover(usuario);

            await _usuarioRepository.SalvarAsync();

            _logger.LogInformation("Usuário excluído com sucesso: {UserId}", id);

            return new BaseResponseDto(true, "Usuário excluído com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir usuário {UserId}", id);
            return new BaseResponseDto(false, "Erro ao excluir o usuário.");
        }
    }

    public async Task<IEnumerable<UsuarioDto>> ListarTodosAsync()
    {
        var usuarios = await _usuarioRepository.ListarTodosAsync();

        return usuarios.Select(u => new UsuarioDto(u.Id, u.Nome, u.Email, u.Role));
    }

    public async Task<BaseResponseDto> PromoverParaAdminAsync(Guid id)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario is null)
            {
                _logger.LogWarning("Tentativa de promoção de usuário inexistente: {UserId}", id);
                return new BaseResponseDto(false, "Usuário não encontrado.");
            }

            usuario.PromoverParaAdmin();

            await _usuarioRepository.SalvarAsync();

            _logger.LogInformation("Usuário promovido para admin: {UserId}", id);

            return new BaseResponseDto(true, "Usuário promovido para administrador com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao promover usuário {UserId}", id);
            return new BaseResponseDto(false, "Erro ao promover o usuário.");
        }
    }

    public async Task<BaseResponseDto> RegistrarAsync(RegistrarUsuarioDto dto)
    {
        try
        {
            if (!UsuarioValidator.ValidarEmail(dto.Email))
            {
                _logger.LogWarning("Registro com e-mail inválido: {Email}", dto.Email);
                return new BaseResponseDto(false, "Formato de e-mail inválido.");
            }

            if (!string.IsNullOrEmpty(dto.Email) && await _usuarioRepository.EmailJaExisteAsync(dto.Email))
            {
                _logger.LogWarning("Email em uso por outro usuário: {Email}", dto.Email);
                return new BaseResponseDto(false, "Este e-mail já está em uso por outro usuário.");
            }

            if (!UsuarioValidator.ValidarSenha(dto.Senha))
            {
                _logger.LogWarning("Registro com senha fraca");
                return new BaseResponseDto(false, "A senha deve ter no mínimo 8 caracteres e conter letras, números e caracteres especiais.");
            }

            if (await _usuarioRepository.ExisteEmailAsync(dto.Email))
            {
                _logger.LogWarning("Tentativa de registro com e-mail já existente: {Email}", dto.Email);
                return new BaseResponseDto(false, "E-mail já cadastrado.");
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var usuario = new Usuario(dto.Nome, dto.Email, senhaHash, "Admin");

            await _usuarioRepository.AdicionarAsync(usuario);

            await _usuarioRepository.SalvarAsync();

            _logger.LogInformation("Usuário registrado com sucesso: {Email}", dto.Email);

            return new BaseResponseDto(true, "Usuário registrado com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário: {Email}", dto.Email);
            return new BaseResponseDto(false, "Erro ao registrar o usuário.");
        }
    }

    public async Task<(bool sucesso, string? token, string mensagem)> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
        {
            _logger.LogWarning("Tentativa de login falhou para o e-mail: {Email}", dto.Email);
            return (false, null, "Email ou senha inválidos.");
        }

        var token = GerarToken(usuario);

        _logger.LogInformation("Login realizado com sucesso: {UserId}", usuario.Id);

        return (true, token, "Login realizado com sucesso.");
    }

    private string GerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Role, usuario.Role),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

        var chave = _config["Jwt:Key"];

        if (string.IsNullOrEmpty(chave))
        {
            _logger.LogCritical("Chave JWT não configurada.");
            throw new Exception("Chave JWT não configurada!");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
