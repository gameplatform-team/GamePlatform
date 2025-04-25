namespace GamePlatform.Application.DTOs.Jogo;

public class JogoResponseDto : BaseResponseDto
{
    public JogoResponseDto(bool sucesso, string mensagem) : base(sucesso, mensagem)
    {
    }

    public JogoResponseDto(bool sucesso, string mensagem, Domain.Entities.Jogo jogo) : base(sucesso, mensagem)
    {
        Jogo = jogo;
    }
    
    public Domain.Entities.Jogo? Jogo { get; set; }
}