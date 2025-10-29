using System.Runtime.Serialization;
namespace LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;

public enum MpiEci
{
    /// <summary>
    /// The transaction passed with 3DS 
    /// (issuer and acquirer both support 3D Secure technology).
    /// </summary>
    [EnumMember(Value = "5")]
    ThreeDSecureSuccess = 5,

    /// <summary>
    /// The issuer of the payer card does not support 3D Secure technology.
    /// </summary>
    [EnumMember(Value = "6")]
    IssuerDoesNotSupport3DS = 6,

    /// <summary>
    /// The operation passed without 3D Secure.
    /// </summary>
    [EnumMember(Value = "7")]
    Without3DSecure = 7
}
