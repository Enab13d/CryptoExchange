using System.Runtime.Serialization;

namespace LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;

public enum Language
{
    [EnumMember(Value = "uk")]
    UK,
    [EnumMember(Value = "en")]
    EN,
}