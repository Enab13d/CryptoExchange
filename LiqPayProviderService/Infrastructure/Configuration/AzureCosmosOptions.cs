namespace LiqPayProviderService.Infrastructure.Configuration;

public class AzureCosmosOptions
{
    public string AccountEndpoint { get; set; } = string.Empty;
    public string AccountKey { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;

}