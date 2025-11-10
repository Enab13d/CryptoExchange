using System.Runtime.Serialization;

namespace LiqPayProviderService.Domain.Constants;

public enum Currency
{
    [EnumMember(Value = "USD")]
    USD,
    [EnumMember(Value = "EUR")]
    EUR,
    [EnumMember(Value = "UAH")]
    UAH
}