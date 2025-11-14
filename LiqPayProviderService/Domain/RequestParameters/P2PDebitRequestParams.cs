using LiqPayProviderService.Domain.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharedContracts.Constants;

namespace LiqPayProviderService.Domain.RequestParameters;

public class P2PDebitRequestParams : ILiqpayBasicApiParams
{

    [JsonProperty("action")]
    [JsonConverter(typeof(StringEnumConverter))]
    public OperationType Action { get; set; }
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("card")]
    public string Card { get; init; } = string.Empty;
    [JsonProperty("card_cvv")]
    public string CardCVV { get; init; } = string.Empty;
    [JsonProperty("card_exp_month")]
    public string CardExpirationMonth { get; set; } = string.Empty;

    [JsonProperty("card_exp_year")]
    public string CardExpirationYear { get; set; } = string.Empty;
    [JsonProperty("card_token")]
    //Токен картки платника. Наприклад: B5BВB0D00B88B00ED00A00D0D
    // (При використанні токену дані картки передавати не потрібно)
    public string? CardToken { get; set; }

    [JsonProperty("currency")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Currency Currency { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("order_id")]
    public string OrderId { get; set; } = string.Empty;
    [JsonProperty("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonProperty("language")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Language? Language { get; set; }
    [JsonProperty("prepare")]
    //Попередня підготовка платежу. 
    // Цей режим дозволяє визначити чи заповнені всі дані , чи потрібна 3DS перевірка картки, чи не перевищено ліміт.
    //  Гроші з картки платника не списуються. 
    // Для включення режиму необхідно передати значення 1, а для роботи з DCC (dynamic currеncy conversion) - значення tariffs
    public string? Prepare { get; set; }
    [JsonProperty("recurringbytoken")]
    //Цей параметр дозволяє генерувати card_token платника, який ви отримаєте в callback запиті
    //  на server_url. card_token дозволяє проводити платежі без введення реквізитів картки платника,
    //  використовуючи API paytoken. Для отримання card_token необхідно передати в запиті значення 1
    public string? RecurringByToken { get; set; }

    [JsonProperty("result_url")]
    // URL у Вашому магазині на який покупця буде переадресовано після завершення покупки.
    //  Максимальна довжина 510 символів
    public string? ResultUrl { get; set; }
    [JsonProperty("server_url")]
    //URL API в Вашому магазині для повідомлень про зміну статусу платежу (сервер -> сервер).
    //  Максимальна довжина 510 символів
    public string? ServerUrl { get; set; }



}