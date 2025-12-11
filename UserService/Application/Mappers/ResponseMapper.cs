
using UserService.Application.DTO.Responses;

namespace UserService.Application.Mappers;

public class ResponseMapper() : IResponseMapper
{


    public TokenDTO ToTokenDTO(KCLoginResponseDTO response)
    {
        return new TokenDTO()
        {
            Access = response.AcessToken,
            Refresh = response.RefreshToken
        };
    }



    public TokenDTO ToTokenDTO(KCRefreshTokenResponseDTO response)
    {
        return new TokenDTO()
        {
            Access = response.AcessToken,
            Refresh = response.RefreshToken
        };
    }
}