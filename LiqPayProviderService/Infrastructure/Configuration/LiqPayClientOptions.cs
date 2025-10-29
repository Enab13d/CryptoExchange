namespace LiqPayProviderService.Infrastructure.Configuration;

public class LiqPayClientOptions
{
    public string PublicKey { get; set; } = string.Empty;

    public string PrivateKey { get; set; } = string.Empty;

    public int LiqPayAPIVersion { get; set; }
}