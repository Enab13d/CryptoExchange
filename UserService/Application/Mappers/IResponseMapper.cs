using UserService.Application.DTO.Responses;

namespace UserService.Application.Mappers;


public interface IResponseMapper
{
    public TokenDTO ToTokenDTO(KCLoginResponseDTO response);

    public TokenDTO ToTokenDTO(KCRefreshTokenResponseDTO response);
};