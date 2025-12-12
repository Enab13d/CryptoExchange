
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedContracts;
using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Infrastructure.Configuration;
using UserService.Application.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

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
    public async Task<string> Register(KCRegisterRequestDTO request, CancellationToken cancellationToken)
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
        // Extract the id from Location header
        var location = response.Headers.Location?.ToString()
            ?? throw new Exception("No Location header from Keycloak");

        // Last segment is the user ID (same as sub claim)
        string userId = location.Split('/').Last();
        return userId;

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

    public async Task SendResetPasswordEmailAsync(string username, CancellationToken cancellationToken)
    {
        //obtain admin acess token
        KCLoginResponseDTO admin = await GetAdminToken(cancellationToken);
        //prepare and send request to check if user exists in KC
        using HttpRequestMessage request = new(HttpMethod.Get, $"/admin/realms/{_options.RealmName}/users?username={Uri.EscapeDataString(username)}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admin.AcessToken);
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        //check if user exist
        // var users = await response.Content.ReadFromJsonAsync<List<JsonElement>>(cancellationToken);
        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        List<UserInfoResponseDTO>? users = JsonConvert.DeserializeObject<List<UserInfoResponseDTO>>(json);

        if (users is null || users.Count == 0)
        {
            throw new KeyNotFoundException($"User with username {username} not found");
        }
        //extract user id from response
        UserInfoResponseDTO user = users.First();
        //prepare and send reset password request
        var resetUrl = $"/admin/realms/{_options.RealmName}/users/{user.Sub}/execute-actions-email" +
                   $"?client_id={Uri.EscapeDataString(_options.ClientId)}" +
                   $"&redirect_uri={Uri.EscapeDataString("http://localhost:4200")}";

        using HttpRequestMessage resetPasswordRequest = new(HttpMethod.Put, resetUrl)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new List<string> { "UPDATE_PASSWORD" }), Encoding.UTF8, "application/json")
        };
        resetPasswordRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admin.AcessToken);

        var resetPasswordResponse = await _httpClient.SendAsync(resetPasswordRequest, cancellationToken);
        resetPasswordResponse.EnsureSuccessStatusCode();

    }


}