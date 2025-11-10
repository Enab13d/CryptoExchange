using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.RequestParameters;
using SharedContracts;

namespace LiqPayProviderService.Api.Extensions;

public static class ToLiqpayRequest
{
    public static CardPaymentRequest ToCardPaymentRequest(this DepositDTO dto)
    {
        bool isValidCurrency = Enum.TryParse<Currency>(dto.Currency, true, out var currency);
        if (!isValidCurrency) throw new ArgumentException($"Provided invalid currency: {dto.Currency}", nameof(dto.Currency));

        return new CardPaymentRequest
        {
            Amount = dto.Amount,
            Currency = currency,
            Description = dto.Description,
            OrderId = dto.OrderId,
            Phone = dto.Phone,
            Action = OperationType.Pay
            // Fields not in DTO are left default (Version, PublicKey, etc.)
        };
    }
}
