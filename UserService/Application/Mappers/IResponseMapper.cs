using UserService.Application.DTO.Responses;
using UserService.Application.Services.DTO;

namespace UserService.Application.Mappers;


public interface IResponseMapper
{
    public TokenDTO ToTokenDTO(KcLoginResponseDTO response);

    public TokenDTO ToTokenDTO(KcRefreshTokenResponseDTO response);
};