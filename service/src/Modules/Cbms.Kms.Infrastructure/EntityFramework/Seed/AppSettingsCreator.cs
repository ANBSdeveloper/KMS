using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.AppSettings.Actions;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Infrastructure.EntityFramework.Seed
{
    public class AppSettingsCreator
    {
        private readonly AppDbContext _context;

        public AppSettingsCreator(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync()
        {
            await AddAsync("REGISTER_NEW_SALES", "1", "Đăng ký nhân viên mới trên Sales App");
            await AddAsync("REGISTER_NEW_SHOP", "1", "Đăng ký shop mới trên Shop App");
            await AddAsync("REWARD_APP_LINK", "https//google.com", "Đường link tải Reward APP trong tin nhắn SMS");
            await AddAsync("HOTLINE_SHOP", "1900", "Hotline tổng đài trong thông báo gửi SMS");
            await AddAsync("SMS_ENABLE", "1", "Bật tắt tính năng gửi SMS");
            await AddAsync("SPOON_SAMPLE_MODE", "0", "Sử dụng mã muỗng ảo để test hệ thống không gọi qua Reward App");
            await AddAsync("TICKET_SAMPLE_MODE", "0", "Dùng để test tạo phiếu không cần kiểm tra thời gian bắt đầu và kết thúc phát phiếu");
            await AddAsync("EFFICIENT_MONTHS", "12", "Số tháng lấy chương trình đầu tư để tính hiệu quả đầu tư");
            await AddAsync("CYCLE_MONTHS", "12", "Số tháng lấy số liệu kỳ làm việc");
            await AddAsync("DMS_SELLOUT_MONTHS", "6", "Số tháng lấy dữ liệu sellout từ DMS, tối đa 12 tháng");
            await AddAsync("QRCODE_LINK", "http://micrositevitadairy.com", "Đường link truy suất thông tin của hàng qua QRCode");
            await AddAsync("BTTT_NOTIFY_TEMPLATE", "Chương trình BTTT {0} của shop {1}: đã {2} bởi {3}", "Mẫu nội dung gửi thông báo khi đề xuất và xét duyệt chương trình BTTT");
            await AddAsync("BTTT_NOTIFY_SUBJECT_TEMPLATE", "Chương trình BTTT {0}", "Mẫu tiều đề gửi thông báo khi đề xuất và xét duyệt chương trình BTTT");
            await AddAsync("BTTT_NOTIFY_SHOP_HOLDING_TEMPLATE", "Chương trình BTTT {0} đang chờ xét duyệt", "Mẫu nội dung gửi thông báo đề xuất chương trình BTTT của cửa hàng");
            await AddAsync("BTTT_NOTIFY_SHOP_APPROVED_TEMPLATE", "Chương trình BTTT {0} đã được duyệt", "Mẫu nội dung gửi thông báo chương trình BTTT của cửa hàng đã được duyệt");
            await AddAsync("BTTT_NOTIFY_SHOP_DENIED_TEMPLATE", "Chương trình BTTT {0} đã bị từ chối", "Mẫu nội dung gửi thông báo chương trình BTTT của cửa hàng đã bị từ chối");
            await AddAsync("BTTT_NOTIFY_SHOP_SUBJECT_TEMPLATE", "Chương trình BTTT {0}","Mẫu tiều đề gửi thông báo khi đề xuất và xét duyệt chương trình BTTT cho cửa hàng");
            await AddAsync("ENABLE_CREATE_TICKET_FROM_SHOP", "0", "Cho phép tạo phiếu BTTT trên Shop App");
            await AddAsync("SHOP_REGISTER_OTP_SMS_TEMPLATE", "Ban dang dang ky tai khoan he thong KMS cua VitaDairy. Ma OTP xac thuc cua ban la {0}", "Mẫu nhận OTP khi cửa hàng đăng ký");
            await AddAsync("SHOP_RESET_PASSWORD_OTP_SMS_TEMPLATE", "Ban đang khoi phuc mat khau tai khoan KMS cua VitaDairy. Ma OTP khoi phuc cua ban la {0}", "Mẫu nhận OTP khi cửa hàng khôi phục mật khẩu");
            await AddAsync("TICKET1_SMS_TEMPLATE", "Ban da tham gia chuong trinh BTTT tai shop {0} va nhan duoc {1} ma BTTT. Ma tham du BTTT cua ban la: {2}", "Thỏa điều kiện tích điểm & nhận mã BTTT: Tự động gen mã BTTT và gởi SMS đến cho người dùng");
            await AddAsync("TICKET2_SMS_TEMPLATE", "Ban da tham gia chuong trinh BTTT tai shop {0} va nhan duoc {1} ma BTTT. Mã tham dự BTTT cua ban la: {2}. So diem tich luy la {3}, vui long mua them san pham de nhan ve ma tham du BTTT.", "Thỏa điều kiện tích điểm & nhận mã BTTT & có số điểm tích lũy chưa gen mã BTTT: Tự động gen mã BTTT và gởi SMS đến cho người dùng");
            await AddAsync("TICKET3_SMS_TEMPLATE", "Ban da tham gia chuong trinh BTTT tai Shop {0}. So diem tich luy la: {3}, vui long mua them san pham de nhan ve ma tham du BTTT.", "Thỏa điều kiện tích điểm & chưa đủ điều kiện nhận mã BTTT");
            await _context.OrignalSaveAsync();
        }

        private async Task AddAsync(string code, string data, string description)
        {
            var entity = _context.AppSettings.FirstOrDefault(p => p.Code == code);
            if (entity == null)
            {
                entity = new AppSetting();
                await entity.ApplyActionAsync(new AppSettingUpsertAction(code, data, description));
                await _context.AppSettings.AddAsync(entity);
            }
        }
    }
}