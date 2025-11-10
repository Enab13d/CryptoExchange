using System.Runtime.Serialization;

namespace LiqPayProviderService.Domain.Constants;

public enum Language
{
    [EnumMember(Value = "uk")]
    UK,
    [EnumMember(Value = "en")]
    EN,
}