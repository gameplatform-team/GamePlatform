namespace GamePlatform.Application.DTOs;

public class BaseResponseDto
{
    public BaseResponseDto() {}

    public BaseResponseDto(bool sucesso, string mensagem)
    {
        Sucesso = sucesso;
        Mensagem = mensagem;
    }
    
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
}