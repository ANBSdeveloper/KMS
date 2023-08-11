namespace Cbms.Kms.Domain.PosmInvestments
{
    public enum PosmInvestmentStatus
    {
        Request = 10,// Đề xuất
        ASMDeniedRequest = 20, // ASM từ chốt duyệt đề xuất
        AsmApprovedRequest = 30, // ASM duyệt đề xuất  
        RSMDeniedRequest = 40, // RSM từ chối duyệt đề xuất
        RsmApprovedRequest = 50, // RSM duyệt đề xuất
        TradeDeniedRequest = 60, // Trade từ chốt PYC
        TradeApprovedRequest = 70,// Trade xác nhận PYC hợp lệ
        DirectorDeniedRequest = 80, // Giám đốc từ chối duyệt
        DirectorApprovedRequest = 90, // Giám đọc duyệt PYC
        InvalidOrder = 100, //  Dơn hàng không hợp lệ
        ValidOrder = 150,
        ConfirmedProduce1 = 160, // Xác nhận sản xuất 1 (MKT)
        ConfirmedProduce2 = 170, // Xác nhận sản xuất 2 (SS)
        ConfirmedVendorProduce = 180, // Xác nhận cho NCC sản xuất
        Accepted = 190, // Nghiệm thu (SS)
        ConfirmAccept1 = 200, // Xác nhận nghiệm thu 1(ASM)
        ConfirmedAccept2 = 210 // Xác nhận nghiệm thu 2(Trade)
    }
}
