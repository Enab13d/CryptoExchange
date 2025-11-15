using System.Runtime.Serialization;

namespace LiqPayProviderService.Domain.Constants;

public enum CardPaymentPayType
{
    //=====Methods of payment=====
    [EnumMember(Value = "apay")]
    //оплата за допомогою зашифрованого токена Apple
    APAY,
    [EnumMember(Value = "gpay")]
    //оплата за допомогою зашифрованого токена Google
    GPAY,

    [EnumMember(Value = "apay_tavv")]
    //оплата за допомогою незашифрованого токена Apple
    APAY_TAVV,
    //installments
    [EnumMember(Value = "gpay_tavv")]
    //оплата за допомогою незашифрованого токена Google
    GPAY_TAVV,
    //cash
    [EnumMember(Value = "tavv")]
    //оплата за допомогою незашифрованого токена міжнародних платіжних систем (MasterCard, Visa)
    TAVV,
}