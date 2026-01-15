
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UserService.Application.DTO.Requests;
using UserService.Application.Services.DTO;
using UserService.Infrastructure.Configuration;


namespace UserService.Application.Services;

public class KeycloakClient(HttpClient httpClient, IOptions<KeycloakOptions> options, ILogger<KeycloakClient> logger) : IKeycloakClient
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly KeycloakOptions _options = options.Value;

    private readonly ILogger<KeycloakClient> _logger = logger;

    private const string ExecutionActionsEmailPath = "/admin/realms/{0}/users/{1}/execute-actions-email";
    private async Task<KcLoginResponseDTO> GetAdminToken(CancellationToken cancellationToken = default)
    {

        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"},
            {"client_id", _options.AdminClientId},
            {"client_secret", _options.AdminClientSecret}
        });
        HttpResponseMessage responseMessage = await _httpClient.PostAsync($"/realms/{_options.RealmName}/protocol/openid-connect/token", body, cancellationToken);
        _logger.LogInformation("GetAdminToken response received. Status code {status}", responseMessage.StatusCode);

        responseMessage.EnsureSuccessStatusCode();
        string json = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
        KcLoginResponseDTO? dto = JsonConvert.DeserializeObject<KcLoginResponseDTO>(json)
        ?? throw new Exception("Login response is null");
        return dto;
    }

    public async Task<KcLoginResponseDTO> Login(KcLoginRequestDTO request, CancellationToken cancellationToken)
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

        KcLoginResponseDTO? dto = JsonConvert.DeserializeObject<KcLoginResponseDTO>(json)
        ?? throw new Exception("Login response is null");

        return dto;


    }
    public async Task<KcRefreshTokenResponseDTO> RefreshToken(KcRefreshTokenRequest request, CancellationToken cancellationToken = default)
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
        KcRefreshTokenResponseDTO? dto = JsonConvert.DeserializeObject<KcRefreshTokenResponseDTO>(json)
        ?? throw new Exception("Refresh token repsonse is null");
        return dto;
    }
    public async Task Logout(KcLogoutRequestDTO request, CancellationToken cancellationToken = default)
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
    public async Task<string> Register(KcRegisterRequestDTO request, CancellationToken cancellationToken)
    {
        KcLoginResponseDTO admin = await GetAdminToken(cancellationToken);
        string json = JsonConvert.SerializeObject(request);
        StringContent content = new(json, Encoding.UTF8, "application/json");
        using HttpRequestMessage message = new(HttpMethod.Post, $"/admin/realms/{_options.RealmName}/users")
        {
            Content = content
        };

        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admin.AcessToken);
        HttpResponseMessage response = await _httpClient.SendAsync(message, cancellationToken);
        _logger.LogInformation("Login response received {code}", response.StatusCode);
        response.EnsureSuccessStatusCode();
        // Extract the id from Location header
        var location = response.Headers.Location?.ToString()
            ?? throw new Exception("No Location header from Keycloak");
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
        // Last segment is the user ID (same as sub claim)
        string userId = location.Split('/').Last();
        return userId;

    }

    public async Task<KcUserInfoResponseDTO> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage message = new(HttpMethod.Get, $"/realms/{_options.RealmName}/protocol/openid-connect/userinfo");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        HttpResponseMessage response = await _httpClient.SendAsync(message, cancellationToken);
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        KcUserInfoResponseDTO? userInfo = JsonConvert.DeserializeObject<KcUserInfoResponseDTO>(json)
        ?? throw new Exception("userInfo is null");
        return userInfo;
    }

    public async Task SendResetPasswordEmailAsync(string username, CancellationToken cancellationToken)
    {
        //obtain admin acess token
        KcLoginResponseDTO admin = await GetAdminToken(cancellationToken);
        //prepare and send request to check if user exists in Kc
        using HttpRequestMessage request = new(HttpMethod.Get, $"/admin/realms/{_options.RealmName}/users?username={Uri.EscapeDataString(username)}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admin.AcessToken);
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        //check if user exist
        // var users = await response.Content.ReadFromJsonAsync<List<JsonElement>>(cancellationToken);
        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        List<KcUserInfoResponseDTO>? users = JsonConvert.DeserializeObject<List<KcUserInfoResponseDTO>>(json);

        if (users is null || users.Count == 0)
        {
            throw new KeyNotFoundException($"User with username {username} not found");
        }
        //extract user id from response
        KcUserInfoResponseDTO user = users.First();
        //prepare and send reset password request
        var resetUrl = string.Format(ExecutionActionsEmailPath, _options.RealmName, user.Sub)
        + $"?client_id={Uri.EscapeDataString(_options.ClientId)}"
        + $"&redirect_uri={Uri.EscapeDataString(_options.RedirectUri)}";

        using HttpRequestMessage resetPasswordRequest = new(HttpMethod.Put, resetUrl)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new List<string> { "UPDATE_PASSWORD" }), Encoding.UTF8, "application/json")
        };
        resetPasswordRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admin.AcessToken);

        var resetPasswordResponse = await _httpClient.SendAsync(resetPasswordRequest, cancellationToken);
        resetPasswordResponse.EnsureSuccessStatusCode();

    }


}