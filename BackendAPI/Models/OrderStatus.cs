namespace BackendAPI.Models
{
    public enum OrderStatus
    {
        WaitingForPayment, // รอการชำระเงิน
        PendingApproval, // รอการอนุมัติจากแอดมิน
        SuccessfulPayment, // ชำระเงินสำเร็จ
        SuccessfulPaymentforcreditCard, // ชำระเงินสำเร็จสำหรับเคดิตการ์ด
        Refunded, คืนเงิน
    }
}
