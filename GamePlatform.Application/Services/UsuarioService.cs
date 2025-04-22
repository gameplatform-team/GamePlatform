using GamePlatform.Application.DTOs;
using GamePlatform.Application.Interfaces.Serivces;
using GamePlatform.Application.Validators;
using GamePlatform.Domain.Entities;
using GamePlatform.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamePlatform.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _config;

    public UsuarioService(IUsuarioRepository usuarioRepository, IConfiguration config)
    {
        _usuarioRepository = usuarioRepository;
        _config = config;
    }

    public async Task<(bool sucesso, string mensagem)> RegistrarAsync(RegistrarUsuarioDto dto)
    {
        if (!UsuarioValidator.ValidarEmail(dto.Email))
            return (false, "Formato de e-mail inválido.");

        if (!UsuarioValidator.ValidarSenha(dto.Senha))
            return (false, "A senha deve ter no mínimo 8 caracteres e conter letras, números e caracteres especiais.");

        if (await _usuarioRepository.ExisteEmailAsync(dto.Email))
            return (false, "E-mail já cadastrado.");

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

        var usuario = new Usuario(dto.Nome, dto.Email, senhaHash, "Admin");

        await _usuarioRepository.AdicionarAsync(usuario);
        await _usuarioRepository.SalvarAsync();

        return (true, "Usuário registrado com sucesso.");
    }

    public async Task<(bool sucesso, string? token, string mensagem)> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            return (false, null, "Email ou senha inválidos.");

        var token = GerarToken(usuario);
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
            throw new Exception("Chave JWT não configurada!");

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