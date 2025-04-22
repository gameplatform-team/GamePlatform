using System.ComponentModel.DataAnnotations;

namespace GamePlatform.Application.DTOs;

public class RegistrarUsuarioDto
{
    [Required]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Senha { get; set; }
}
