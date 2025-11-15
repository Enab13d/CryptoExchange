using System.Runtime.Serialization;

namespace LiqPayProviderService.Domain.Constants;

public enum BonusType
{
    [EnumMember(Value = "bonusplus")]
    BonusPlus,
    [EnumMember(Value = "discount_club")]
    DiscountClub,
    [EnumMember(Value = "personal")]
    Personal,
    [EnumMember(Value = "promo")]
    Promo
}