
using UserService.Application.DTO.Responses;
using UserService.Application.Services.DTO;

namespace UserService.Application.Mappers;

public class ResponseMapper() : IResponseMapper
{


    public TokenDTO ToTokenDTO(KcLoginResponseDTO response)
    {
        return new TokenDTO()
        {
            Access = response.AcessToken,
            Refresh = response.RefreshToken
        };
    }



    public TokenDTO ToTokenDTO(KcRefreshTokenResponseDTO response)
    {
        return new TokenDTO()
        {
            Access = response.AcessToken,
            Refresh = response.RefreshToken
        };
    }
}