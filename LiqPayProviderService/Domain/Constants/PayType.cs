using System.Runtime.Serialization;
namespace LiqPayProviderService.Domain.Constants;

public enum PayType
{
    //=====Methods of payment=====
    //card payment
    [EnumMember(Value = "card")]
    Card,
    //with privat24 account
    [EnumMember(Value = "privat24")]
    Privat24,
    //with masterpass account
    [EnumMember(Value = "masterpass")]
    Masterpass,
    //installments
    [EnumMember(Value = "moment_part")]
    MomentPart,
    //cash
    [EnumMember(Value = "cash")]
    Cash,
    //to email
    [EnumMember(Value = "invoice")]
    Invoice,
    //qr code scanning
    [EnumMember(Value = "qr")]
    QR,
}