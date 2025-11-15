using System.Runtime.Serialization;

namespace LiqPayProviderService.Domain.Constants;

public enum OperationType
{
    [EnumMember(Value = "pay")]
    Pay,
    [EnumMember(Value = "hold")]
    Hold,
    [EnumMember(Value = "subscribe")]
    Subscribe,
    [EnumMember(Value = "regular")]
    Regular,
    [EnumMember(Value = "paysplit")]
    PaySplit,
    [EnumMember(Value = "paydonate")]
    PayDonate,
    [EnumMember(Value = "auth")]
    Auth,
    [EnumMember(Value = "p2pdebit")]
    P2PDebit,

}