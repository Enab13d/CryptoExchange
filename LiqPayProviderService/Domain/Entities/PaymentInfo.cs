using LiqPayProviderService.Domain.Constants;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LiqPayProviderService.Domain.Entities;


public class PaymentInfo
{
    //has the shape of liqpay callback response data

    [JsonIgnore]
    public ObjectId Id { get; set; }
    [JsonProperty("acq_id")]
    public string? AcquirerID { get; set; }

    [JsonProperty("action")]
    [JsonConverter(typeof(StringEnumConverter))]
    public OperationType Action { get; set; }

    [JsonProperty("agent_commission")]
    public decimal? AgentCommission { get; set; }

    [JsonProperty("amount")]
    public decimal? Amount { get; set; }

    [JsonProperty("amount_bonus")]
    public decimal? AmountBonus { get; set; }

    [JsonProperty("amount_credit")]
    public decimal? AmountCredit { get; set; }

    [JsonProperty("amount_debit")]
    public decimal? AmountDebit { get; set; }

    [JsonProperty("authcode_credit")]
    public string? AuthCodeCredit { get; set; }

    [JsonProperty("authcode_debit")]
    public string? AuthCodeDebit { get; set; }

    [JsonProperty("card_token")]
    public string? CardToken { get; set; }

    [JsonProperty("commission_credit")]
    public decimal? CommissionCredit { get; set; }

    [JsonProperty("commission_debit")]
    public decimal? CommissionDebit { get; set; }

    [JsonProperty("completion_date")]
    public string? CompletionDate { get; set; }

    [JsonProperty("create_date")]
    public string? CreateDate { get; set; }

    [JsonProperty("currency")]
    public string? Currency { get; set; }

    [JsonProperty("currency_credit")]
    public string? CurrencyCredit { get; set; }

    [JsonProperty("currency_debit")]
    public string? CurrencyDebit { get; set; }

    [JsonProperty("customer")]
    public string? Customer { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("end_date")]
    public string? EndDate { get; set; }

    [JsonProperty("err_code")]
    public string? ErrorCode { get; set; }

    [JsonProperty("err_description")]
    public string? ErrorDescription { get; set; }

    [JsonProperty("info")]
    public string? Info { get; set; }

    [JsonProperty("ip")]
    public string? IP { get; set; }

    [JsonProperty("is_3ds")]
    public bool Is3Ds { get; set; }
    [JsonProperty("liqpay_order_id")]
    public string? LiqPayOrderId { get; set; }

    [JsonProperty("order_id")]
    public string? OrderId { get; set; }

    [JsonProperty("payment_id")]
    public string? PaymentId { get; set; }

    public PayType PayType { get; set; }

    [JsonProperty("public_key")]
    public string? PublicKey { get; set; }

    [JsonProperty("receiver_commission")]
    public decimal? ReceiverCommission { get; set; }

    [JsonProperty("redirect_to")]
    public string? RedirectTo { get; set; }

    [JsonProperty("refund_date_last")]
    public string? RefundDateLast { get; set; }

    [JsonProperty("rrn_credit")]
    public string? RrnCredit { get; set; }

    [JsonProperty("rrn_debit")]
    public string? RrnDebit { get; set; }

    [JsonProperty("sender_bonus")]
    public decimal? SenderBonus { get; set; }

    [JsonProperty("sender_card_bank")]
    public string? SenderCardBank { get; set; }

    [JsonProperty("sender_card_country")]
    public string? SenderCardCountry { get; set; }

    [JsonProperty("sender_card_mask2")]
    public string? SenderCardMask2 { get; set; }

    [JsonProperty("sender_card_type")]
    public string? SenderCardType { get; set; }

    [JsonProperty("sender_commission")]
    public decimal? SenderCommission { get; set; }

    [JsonProperty("sender_first_name")]
    public string? SenderFirstName { get; set; }

    [JsonProperty("sender_last_name")]
    public string? SenderLastName { get; set; }

    [JsonProperty("sender_phone")]
    public string? SenderPhone { get; set; }

    public PaymentStatus Status { get; set; }

    [JsonProperty("wait_reserve_status")]
    public string? WaitReserveStatus { get; set; }

    [JsonProperty("token")]
    public string? Token { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("version")]
    public int? Version { get; set; }

    [JsonProperty("err_erc")]
    public string? ErrorErc { get; set; }

    [JsonProperty("product_category")]
    public string? ProductCategory { get; set; }

    [JsonProperty("product_description")]
    public string? ProductDescription { get; set; }

    [JsonProperty("product_name")]
    public string? ProductName { get; set; }

    [JsonProperty("product_url")]
    public string? ProductUrl { get; set; }

    [JsonProperty("refund_amount")]
    public decimal? RefundAmount { get; set; }

    [JsonProperty("verifycode")]
    public string? VerifyCode { get; set; }

}
