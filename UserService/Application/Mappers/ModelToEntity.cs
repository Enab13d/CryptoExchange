using UserService.Application.DTO.Requests;
using UserService.Domain.Entities;

namespace UserService.Application.Mappers;


public static class ModelToEnity
{
    public static User UserFromRegisterRequest(RegisterRequestDTO dto)
    {
        return new User()
        {
            Username = dto.Username,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
    }
}