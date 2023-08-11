namespace Cbms.Kms.Domain.PosmInvestments
{
    public enum PosmInvestmentItemStatus
    {
        Request = 10,// Đề xuất
        AsmDeniedRequest = 20, // ASM từ chốt duyệt đề xuất
        AsmApprovedRequest = 30, // ASM duyệt đề xuất  
        RsmDeniedRequest = 40, // RSM từ chối duyệt đề xuất
        RsmApprovedRequest = 50, // RSM duyệt đề xuất
        TradeDeniedRequest = 60, // Trade từ chốt PYC
        TradeApprovedRequest = 70,// Trade xác nhận PYC hợp lệ
        DirectorDeniedRequest = 80, // Giám đốc từ chối duyệt
        DirectorApprovedRequest = 90, // Giám đốc duyệt PYC
        InvalidOrder = 100, //  Dơn hàng không hợp lệ
        SupSuggestedUpdateCost = 110, // SS đề xuất bổ sung chi phí
        AsmConfirmedUpdateCost = 120, // ASM xác nhận bổ sung
        RsmConfirmedUpdateCost = 130, // RSM xác nhận bổ sung
        TradeConfirmedUpdateCost = 140, // Trade xác nhận bổ sung
        ValidOrder = 150,
        ConfirmedProduce1 = 160, // Xác nhận sản xuất 1 (MKT)
        ConfirmedProduce2 = 170, // Xác nhận sản xuất 2 (SS)
        ConfirmedVendorProduce = 180, // Xác nhận cho NCC sản xuất
        Accepted = 190, // Nghiệm thu (SS) 
        ConfirmedAccept1 = 200, // Xác nhận nghiệm thu 1(ASM)
        ConfirmedAccept2 = 210 // Xác nhận nghiệm thu 2(Trade)
    }
}
