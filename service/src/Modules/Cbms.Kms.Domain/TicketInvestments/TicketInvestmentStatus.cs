namespace Cbms.Kms.Domain.TicketInvestments
{
    public enum TicketInvestmentStatus
    {
        RequestInvestment = 10,// Đề Xuất
        DeniedRequestInvestment = 20, // Không Xác Nhận Yêu Cầu
        ConfirmedRequestInvestment = 30, // Xác Nhận Yêu Cầu
        ValidRequestInvestment1 = 40, // Xác Nhận PYC Hợp Lệ 1
        InValidRequestInvestment1 = 50, // PYC Không Hợp Lệ 1
        ValidRequestInvestment2 = 60,// Xác Nhận PYC Hợp Lệ 2
        InValidRequestInvestment2 = 70,// PYC Không Hợp Lệ 2
        ConfirmedInvestment = 80,// Xác Nhận Đầu Tư
        DeniedInvestmentConfirmation = 90,// Không Xác Nhận Đầu Tư
        ApproveInvestment = 100, // Trade Duyệt Đầu Tư
        DeniedInvestmentApproval = 110, // Trade Không Duyệt Đầu Tư
        Approved = 120, // Đã Duyệt
        Denied = 130, // Hủy Duyệt Đầu Tư
        Doing = 140,// Đang Thực Hiện
        Operated = 150,// Đã Tổ Chức
        Accepted = 160,// Đã Nghiệm Thu
        FinalSettlement = 170,// Đã Quyết Toán
    }
}
