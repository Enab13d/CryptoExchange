namespace UserService.Application.DTO.Responses;


public record TokenDTO
{
    public string Access { get; set; } = string.Empty;
    public string Refresh { get; set; } = string.Empty;
}