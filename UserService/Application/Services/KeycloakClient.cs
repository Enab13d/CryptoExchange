
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Infrastructure.Configuration;

namespace UserService.Application.Services;

public class KeycloakClient(HttpClient httpClient, IOptions<KeycloakOptions> options, ILogger<KeycloakClient> logger) : IKeycloakClient
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly KeycloakOptions _options = options.Value;

    private readonly ILogger<KeycloakClient> _logger = logger;

    private async Task<KCLoginResponseDTO> GetAdminToken(CancellationToken cancellationToken = default)
    {

        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"},
            {"client_id", _options.AdminClientId},
            {"client_secret", _options.AdminClientSecret}
        });
        HttpResponseMessage responseMessage = await _httpClient.PostAsync($"/realms/{_options.RealmName}/protocol/openid-connect/token", body, cancellationToken);
        responseMessage.EnsureSuccessStatusCode();
        _logger.LogInformation("GetAdminToken response received. Status code {status}", responseMessage.StatusCode);
        string json = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
        KCLoginResponseDTO? dto = JsonConvert.DeserializeObject<KCLoginResponseDTO>(json)
        ?? throw new Exception("Login response is null");
        return dto;
    }

    public async Task<KCLoginResponseDTO> Login(KCLoginRequestDTO request, CancellationToken cancellationToken)
    {
        FormUrlEncodedContent body = new(new Dictionary<string, string>()
        {
            {"grant_type", request.GrantType},
            {"client_id", request.ClientId},
            {"client_secret", request.ClientSecret},
            {"username", request.Username },
            {"password", request.Password },
            {"scope", "openid profile email"}
        });
        HttpResponseMessage response = await _httpClient.PostAsync($"/realms/{_options.RealmName}/protocol/openid-connect/token", body, cancellationToken);
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        KCLoginResponseDTO? dto = JsonConvert.DeserializeObject<KCLoginResponseDTO>(json)
        ?? throw new Exception("Login response is null");

        return dto;


    }
    public async Task<KCRefreshTokenResponseDTO> RefreshToken(KCRefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        FormUrlEncodedContent body = new(new Dictionary<string, string>()
        {
            {"grant_type", request.GrantType},
            {"client_id", request.ClientId},
            {"client_secret", request.ClientSecret},
            {"refresh_token", request.RefreshToken},
        });

        HttpResponseMessage response = await _httpClient.PostAsync($"/realms/{_options.RealmName}/protocol/openid-connect/token", body, cancellationToken);
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        KCRefreshTokenResponseDTO? dto = JsonConvert.DeserializeObject<KCRefreshTokenResponseDTO>(json)
        ?? throw new Exception("Refresh token repsonse is null");
        return dto;
    }
    public async Task Logout(KCLogoutRequestDTO request, CancellationToken cancellationToken = default)
    {
        FormUrlEncodedContent body = new(new Dictionary<string, string>()
        {
            {"client_id", request.ClientId},
            {"client_secret", request.ClientSecret},
            {"refresh_token", request.RefreshToken},
        });
        HttpResponseMessage response = await _httpClient.PostAsync($"/realms/{_options.RealmName}/protocol/openid-connect/logout", body, cancellationToken);
        response.EnsureSuccessStatusCode();
        _logger.LogInformation("Logout sucess");
        return;
    }
    public async Task Register(KCRegisterRequestDTO request, CancellationToken cancellationToken)
    {
        KCLoginResponseDTO admin = await GetAdminToken(cancellationToken);
        string json = JsonConvert.SerializeObject(request);
        StringContent content = new(json, Encoding.UTF8, "application/json");
        using HttpRequestMessage message = new(HttpMethod.Post, $"/admin/realms/{_options.RealmName}/users")
        {
            Content = content
        };

        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admin.AcessToken);
        HttpResponseMessage response = await _httpClient.SendAsync(message, cancellationToken);
        response.EnsureSuccessStatusCode();
        _logger.LogInformation("Register user response received. Status code {status}", response.StatusCode);
        return;

    }

    public async Task<UserInfoResponseDTO> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage message = new(HttpMethod.Get, $"/realms/{_options.RealmName}/protocol/openid-connect/userinfo");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        HttpResponseMessage response = await _httpClient.SendAsync(message, cancellationToken);
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        UserInfoResponseDTO? userInfo = JsonConvert.DeserializeObject<UserInfoResponseDTO>(json)
        ?? throw new Exception("userInfo is null");
        return userInfo;
    }
}