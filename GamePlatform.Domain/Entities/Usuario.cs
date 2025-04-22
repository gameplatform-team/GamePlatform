namespace GamePlatform.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public string Role { get; private set; } // "Usuario" ou "Admin"

    public Usuario(string nome, string email, string senhaHash, string role = "Usuario")
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Role = role;
    }
}
