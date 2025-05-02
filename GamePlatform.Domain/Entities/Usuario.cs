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

    public void Atualizar(string? nome, string? email, string? novaSenha)
    {
        if (!string.IsNullOrEmpty(nome))
            Nome = nome;

        if (!string.IsNullOrEmpty(email))
            Email = email;

        if (!string.IsNullOrEmpty(novaSenha))
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);
    }

    public void PromoverParaAdmin()
    {
        Role = "Admin";
    }
}
