using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.RequestParameters;
using SharedContracts;

namespace LiqPayProviderService.Api.Extensions;

public static class ToLiqpayRequest
{
    public static CardPaymentRequest ToCardPaymentRequest(this DepositDTO dto)
    {
        return new CardPaymentRequest
        {
            Amount = (decimal)dto.Amount,
            Currency = Enum.TryParse<Currency>(dto.Currency, true, out var currency)
                ? currency
                : Currency.UAH, // fallback if unknown
            Description = dto.Description,
            OrderId = dto.OrderId,
            Phone = dto.Phone,
            Action = OperationType.Pay
            // Fields not in DTO are left default (Version, PublicKey, etc.)
        };
    }
}
