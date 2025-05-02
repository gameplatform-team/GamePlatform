namespace GamePlatform.Application.DTOs.Usuario;

public class UsuarioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;

    public UsuarioDto(Guid id, string nome, string email, string role)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Role = role;
    }
}
