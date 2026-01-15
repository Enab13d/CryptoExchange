namespace UserService.Infrastructure.Configuration;


public class KeycloakOptions
{
    public string Authority { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string RealmName { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AdminClientId { get; set; } = string.Empty;
    public string AdminClientSecret { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
}