using LiqPayProviderService.Domain.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharedContracts.Constants;

namespace LiqPayProviderService.Domain.RequestParameters;

public class CardPaymentRequest : ILiqpayBasicApiParams
{


    [JsonProperty("action")]
    [JsonConverter(typeof(StringEnumConverter))]
    public OperationType Action { get; set; }
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("card")]
    public string Card { get; init; } = string.Empty;
    [JsonProperty("card_cvv")]
    public string? CardCVV { get; init; }
    [JsonProperty("card_exp_month")]
    public string? CardExpirationMonth { get; set; }
    //card_exp_year
    [JsonProperty("card_exp_year")]
    public string? CardExpirationYear { get; set; }

    [JsonProperty("currency")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Currency Currency { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
    [JsonProperty("ip")]
    public string IP { get; set; } = string.Empty;
    [JsonProperty("order_id")]
    public string OrderId { get; set; } = string.Empty;
    [JsonProperty("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonProperty("paytype")]
    [JsonConverter(typeof(StringEnumConverter))]
    //Параметр обов'язковий для оплат за допомогою зашифрованих і незашифрованих токенів
    public CardPaymentPayType? PayType { get; set; }

    [JsonProperty("tavv")]
    /*
    Криптограма — динамічний одноразовий код для кожної транзакції, що супроводжує токен.
    Параметр обов'язковий при здійсненні оплати за допомогою незашифрованих токенів Apple,
    Google, міжнародних платіжних систем (MasterCard, Visa)
    */
    public string? Tavv { get; set; }
    [JsonProperty("tid")]
    /*
    дентифікатор попередньої транзакції.
    Для рекурентних платежів за токенами міжнародної платіжної системи Visa
    */
    public string? TId { get; set; }

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

    [JsonProperty("recurring")]
    public bool? Recurring { get; set; }
    //Ознака рекурентної оплати за токеном. 
    // Використовується для оплат за токеном міжнародних платіжних систем (MasterCard, Visa).
    // Можливі значення true - операція виконується без участі клієнта, false - операція виконується клієнтом

    [JsonProperty("server_url")]
    //URL API в Вашому магазині для повідомлень про зміну статусу платежу (сервер -> сервер).
    //  Максимальна довжина 510 символів
    public string? ServerUrl { get; set; }

    [JsonProperty("eci")]
    // Electronic Commerce Indicator - код, який ідентифікує тип транзакції 
    // та факт проведення аутентифікації платника через 3D Secure або інший спосіб.
    // Можливі значення:
    // MasterCard: 02, 06
    // Visa: 05, 07
    public string? ECI { get; set; }
    [JsonProperty("cavv")]
    // Cardholder Authentication Verification Value - використовується 
    // для перевірки автентичності власника картки під час транзакцій, що проходять через 3D Secure
    public string? CAVV { get; set; }

    [JsonProperty("tdsv")]
    // Версія, за якою проходила перевірка 3ds
    public string? TDSV { get; set; }
    [JsonProperty("dsTransID")]
    // Ідентифікатор сесії перевірки 3ds
    public string? DsTransID { get; set; }

}