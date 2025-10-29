using System.Runtime.Serialization;

namespace LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;

public enum Currency
{
    [EnumMember(Value = "USD")]
    USD,
    [EnumMember(Value = "EUR")]
    EUR,
    [EnumMember(Value = "UAH")]
    UAH
}