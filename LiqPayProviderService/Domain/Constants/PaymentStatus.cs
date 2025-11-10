using System.Runtime.Serialization;

namespace LiqPayProviderService.Domain.Constants;

public enum PaymentStatus
{
    // ===== FINAL PAYMENT STATUSES =====

    /// <summary>
    /// Failed payment. Data is incorrect.
    /// </summary>
    [EnumMember(Value = "error")]
    Error,

    /// <summary>
    /// Failed payment.
    /// </summary>
    [EnumMember(Value = "failure")]
    Failure,

    /// <summary>
    /// Payment refunded.
    /// </summary>
    [EnumMember(Value = "reversed")]
    Reversed,

    /// <summary>
    /// Subscribed successfully framed.
    /// </summary>
    [EnumMember(Value = "subscribed")]
    Subscribed,

    /// <summary>
    /// Successful payment.
    /// </summary>
    [EnumMember(Value = "success")]
    Success,

    /// <summary>
    /// Subscribed successfully deactivated.
    /// </summary>
    [EnumMember(Value = "unsubscribed")]
    Unsubscribed,


    // ===== STATUSES THAT REQUIRE PAYMENT CONFIRMATION =====

    /// <summary>
    /// 3DS verification is required to finish the payment.
    /// </summary>
    [EnumMember(Value = "3ds_verify")]
    Verify3DS,

    /// <summary>
    /// Waiting for customer to confirm with captcha.
    /// </summary>
    [EnumMember(Value = "captcha_verify")]
    CaptchaVerify,

    /// <summary>
    /// Sender's card CVV is required to finish the payment.
    /// </summary>
    [EnumMember(Value = "cvv_verify")]
    CVVVerify,

    /// <summary>
    /// Waiting for customer to confirm with IVR.
    /// </summary>
    [EnumMember(Value = "ivr_verify")]
    IVRVerify,

    /// <summary>
    /// OTP confirmation is required. OTP sent to customer's phone.
    /// </summary>
    [EnumMember(Value = "otp_verify")]
    OTPVerify,

    /// <summary>
    /// Waiting for customer to confirm with Privat24 password.
    /// </summary>
    [EnumMember(Value = "password_verify")]
    PasswordVerify,

    /// <summary>
    /// Waiting for customer to enter a phone number.
    /// </summary>
    [EnumMember(Value = "phone_verify")]
    PhoneVerify,

    /// <summary>
    /// Waiting for customer to confirm with PIN code.
    /// </summary>
    [EnumMember(Value = "pin_verify")]
    PinVerify,

    /// <summary>
    /// Receiver additional data is required to finish payment.
    /// </summary>
    [EnumMember(Value = "receiver_verify")]
    ReceiverVerify,

    /// <summary>
    /// Sender additional data is required to finish payment.
    /// </summary>
    [EnumMember(Value = "sender_verify")]
    SenderVerify,

    /// <summary>
    /// Waiting for customer to confirm with Privat24 sender app.
    /// </summary>
    [EnumMember(Value = "senderapp_verify")]
    SenderAppVerify,

    /// <summary>
    /// Waiting for customer to scan QR-code.
    /// </summary>
    [EnumMember(Value = "wait_qr")]
    WaitQr,

    /// <summary>
    /// Waiting for customer to confirm in mobile Privat24/SENDER app.
    /// </summary>
    [EnumMember(Value = "wait_sender")]
    WaitSender,


    // ===== OTHER PAYMENT STATUSES =====

    /// <summary>
    /// Waiting for payment in self-service terminal.
    /// </summary>
    [EnumMember(Value = "cash_wait")]
    CashWait,

    /// <summary>
    /// Amount successfully blocked on the sender's account.
    /// </summary>
    [EnumMember(Value = "hold_wait")]
    HoldWait,

    /// <summary>
    /// Invoice is created successfully, waiting for payment.
    /// </summary>
    [EnumMember(Value = "invoice_wait")]
    InvoiceWait,

    /// <summary>
    /// Payment is created, waiting for customer to finish it.
    /// </summary>
    [EnumMember(Value = "prepared")]
    Prepared,

    /// <summary>
    /// Payment is processing.
    /// </summary>
    [EnumMember(Value = "processing")]
    Processing,

    /// <summary>
    /// Money withdrawn from client but store is not verified yet.
    /// </summary>
    [EnumMember(Value = "wait_accept")]
    WaitAccept,

    /// <summary>
    /// Recipient hasn't set compensation method.
    /// </summary>
    [EnumMember(Value = "wait_card")]
    WaitCard,

    /// <summary>
    /// Payment successful, will be transferred in daily settlement.
    /// </summary>
    [EnumMember(Value = "wait_compensation")]
    WaitCompensation,

    /// <summary>
    /// Protected payment — waiting for receipt of goods confirmation.
    /// </summary>
    [EnumMember(Value = "wait_lc")]
    WaitLc,

    /// <summary>
    /// Funds reserved to make a refund per refund request.
    /// </summary>
    [EnumMember(Value = "wait_reserve")]
    WaitReserve,

    /// <summary>
    /// Payment verified.
    /// </summary>
    [EnumMember(Value = "wait_secure")]
    WaitSecure,

    /// <summary>
    /// Payment performed in sandbox mode.
    /// </summary>
    [EnumMember(Value = "sandbox")]
    Sandbox
}
