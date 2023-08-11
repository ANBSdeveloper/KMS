using Cbms.Authorization.Permissions;
using Cbms.Authorization.Roles;
using Cbms.Authorization.Roles.Actions;
using Cbms.Kms.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Infrastructure.EntityFramework.Seed
{
    public class PermissionsCreator
    {
        private readonly AppDbContext _context;

        public PermissionsCreator(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync()
        {
            var adminRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.AdminRole);
            var salesAdminRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.SalesAdminRole);
            var asmRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.AsmRole);
            var rsmRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.RsmRole);
            var salesSupRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.SalesSupervisorRole);
            var tradeAdminRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.TradeAdminRole);
            var shopRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.ShopRole);
            var customerDevelopmentmManagerRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.CustomerDevelopmentManagerRole);
            var leadCustomerDevelopmentRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.CustomerDevelopmentLeadRole);
            var customerDevelopmentAdminRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.CustomerDevelopmentAdminRole);
            var tradeMarketingRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.TradeMarketingRole);
            var salesDirectorRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.SalesDirector);
            var developmentDirector = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.DevelopmentDirector);
            var supplyRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.SupplyRole);
            var maretkingRole = _context.Roles.Include(p => p.Permissions).FirstOrDefault(p => p.RoleName == KmsConsts.MarketingRole);

            await AddPermission(null, "Users", "Người dùng");
            await AddPermission(null, "Roles", "Vai trò");
            await AddPermission(null, "Brands", "Nhãn hàng");
            await AddPermission(null, "ProductClasses", "Ngành hàng");
            await AddPermission(null, "SubProductClasses", "Ngành hàng con");
            await AddPermission(null, "Provinces", "Tỉnh thành");
            await AddPermission(null, "Districts", "Quận huyện");
            await AddPermission(null, "Customers", "Khách hàng");
            await AddPermission(null, "Customers.RegisterKeyShop", "Đăng ký Key Shop");
            await AddPermission(null, "Customers.ApproveKeyShop", "Duyệt Key Shop");
            await AddPermission(null, "CustomerSales", "Doanh số khách hàng");
            await AddPermission(null, "Products", "Sản phẩm");
            await AddPermission(null, "ProductPoints", "Cài đặt điểm sản phẩm");
            await AddPermission(null, "Branches", "Nhà phân phối");
            await AddPermission(null, "Zones", "Vùng");
            await AddPermission(null, "Areas", "Khu vực");
            await AddPermission(null, "Wards", "Phường xã");
            await AddPermission(null, "CustomerLocations", "Vị trí cửa hàng");
            await AddPermission(null, "RewardPackages", "Gói quà");
            await AddPermission(null, "Vendors", "Nhà cung cấp");
            await AddPermission(null, "PosmClasses", "Nhóm POSM");
            await AddPermission(null, "PosmTypes", "Loại POSM");
            await AddPermission(null, "PosmItems", "POSM");
            await AddPermission(null, "PosmPrices", "Bảng giá POSM");

            var staffPermission = await AddPermission(null, "Staffs", "Nhân viên");
            await AddPermission(staffPermission.Id, "Staffs.UpdateSsCreditPoint", "Cập nhập điểm tín nhiệm của SS");
            await AddPermission(staffPermission.Id, "Staffs.UpdateAsmCreditPoint", "Cập nhập điểm tín nhiệm của ASM");
            await AddPermission(staffPermission.Id, "Staffs.UpdateRsmCreditPoint", "Cập nhập điểm tín nhiệm của RSM");

            await AddPermission(null, "InvestmentSettings", "Cấu hình đầu tư");
            var dashboardPermission = await AddPermission(null, "Dashboards", "Dashboards");
            await AddPermission(dashboardPermission.Id, "Dashboards.Admin", "Admin dashboard");
            await AddPermission(null, "Dashboards.Sales", "Sales dashboard");
            var budgetPerrmission = await AddPermission(null, "Budgets", "Ngân sách");
            await AddPermission(budgetPerrmission.Id, "Budgets.AllocateArea", "Phân bổ NS cho khu vực");
            await AddPermission(budgetPerrmission.Id, "Budgets.AllocateBranch", "Phân bổ NS cho npp");
            await AddPermission(null, "ProductUnits", "Đơn vị sản phẩm");
            await AddPermission(null, "Materials", "Vật tư");
            await AddPermission(null, "Cycles", "Kỳ làm việc");
            await AddPermission(null, "Channels", "Kênh");
            await AddPermission(null, "Tickets", "In phiếu BTTT");
            await AddPermission(null, "AppSettings", "Cài đặt hệ thống");

            var orderPermission = await AddPermission(null, "Orders", "Đơn hàng");
            await AddPermission(orderPermission.Id, "Orders.Create", "Tạo đơn hàng");

            var ticketPermission = await AddPermission(null, "TicketInvestments", "Đầu tư BTTT");
            // Đăng ký
            await AddPermission(ticketPermission.Id, "TicketInvestments.Register", "Đăng ký đầu tư BTTT");
            // Xác nhận yêu cầu đầu tư
            await AddPermission(ticketPermission.Id, "TicketInvestments.ApproveRequest", "Xác nhận YCDT");
            // Từ chối yêu cầu đầu tư
            await AddPermission(ticketPermission.Id, "TicketInvestments.DenyRequest", "Từ chối YCDT");
            //Xác nhận PYC hơp lệ 1
            await AddPermission(ticketPermission.Id, "TicketInvestments.ConfirmValid1", "Xác nhận PYC hơp lệ 1");
            //Từ chối PYC hơp lệ 1
            await AddPermission(ticketPermission.Id, "TicketInvestments.DenyValid1", "Từ chối PYC hơp lệ 1");
            //Xác nhận PYC hơp lệ 2
            await AddPermission(ticketPermission.Id, "TicketInvestments.ConfirmValid2", "Xác nhận PYC hơp lệ 2");
            //Từ chối PYC hơp lệ 2
            await AddPermission(ticketPermission.Id, "TicketInvestments.DenyValid2", "Từ chối PYC hơp lệ 2");
            //Xác nhận đầu tư
            await AddPermission(ticketPermission.Id, "TicketInvestments.ConfirmInvestment", "Xác nhận đầu tư");
            //Từ chối đầu tư
            await AddPermission(ticketPermission.Id, "TicketInvestments.DenyInvestmentConfirmation", "Từ chối đầu tư");
            //Duyệt đầu tư
            await AddPermission(ticketPermission.Id, "TicketInvestments.ApproveInvestment1", "Duyệt đầu tư 1");
            //Từ chối duyệt đầu tư
            await AddPermission(ticketPermission.Id, "TicketInvestments.DenyInvestment1", "Từ chối duyệt đầu tư 1");
            //Duyệt đầu tư
            await AddPermission(ticketPermission.Id, "TicketInvestments.ApproveInvestment2", "Duyệt đầu Tư 2");
            //Từ chối duyệt đầu tư
            await AddPermission(ticketPermission.Id, "TicketInvestments.DenyInvestment2", "Từ chối duyệt đầu tư 2");
            //Cập nhật tiến độ
            await AddPermission(ticketPermission.Id, "TicketInvestments.UpdateProgress", "Cập nhật tiến độ");
            //Tổ chức sự kiện
            await AddPermission(ticketPermission.Id, "TicketInvestments.Operate", "Tổ chức");
            //Nghiệm thu
            await AddPermission(ticketPermission.Id, "TicketInvestments.Accept", "Nghiệm thu");
            //Quyết toán
            await AddPermission(ticketPermission.Id, "TicketInvestments.FinalSettlement", "Quyết toán");
            //Chấm điểm của Sales
            await AddPermission(ticketPermission.Id, "TicketInvestments.SalesRemark", "Chấm điểm của Sales");
            //Chấm điểm của PTKH
            await AddPermission(ticketPermission.Id, "TicketInvestments.CustomerDevelopmentRemark", "Chấm điểm của PTKH");
            //Chấm điểm của công ty
            await AddPermission(ticketPermission.Id, "TicketInvestments.CompanyRemark", "Chấm điểm của CTY");

            var posmPermission = await AddPermission(null, "PosmInvestments", "Đầu tư POSM");
            // Đăng ký
            await AddPermission(posmPermission.Id, "PosmInvestments.Register", "Đăng ký đầu tư POSM");
            await AddPermission(posmPermission.Id, "PosmInvestments.AsmApprove", "Asm duyệt yêu cầu");
            await AddPermission(posmPermission.Id, "PosmInvestments.AsmDeny", "Asm từ chối yêu cầu");
            await AddPermission(posmPermission.Id, "PosmInvestments.RsmApprove", "Rsm duyệt yêu cầu");
            await AddPermission(posmPermission.Id, "PosmInvestments.RsmDeny", "Rsm từ chối yêu cầu");
            await AddPermission(posmPermission.Id, "PosmInvestments.TradeApprove", "Trade duyệt yêu cầu");
            await AddPermission(posmPermission.Id, "PosmInvestments.TradeDeny", "Trade từ chối yêu cầu");
            await AddPermission(posmPermission.Id, "PosmInvestments.DirectorApprove", "Giám đốc duyệt yêu cầu");
            await AddPermission(posmPermission.Id, "PosmInvestments.DirectorDeny", "Giám đốc từ chối yêu cầu");
            await AddPermission(posmPermission.Id, "PosmInvestments.SupplyConfirmRequest", "Cung ứng xác nhận hợp lệ");
            await AddPermission(posmPermission.Id, "PosmInvestments.SupplyDenyRequest", "Cũng ứng xác nhận không hợp lệ");
            await AddPermission(posmPermission.Id, "PosmInvestments.SuggestBudget", "Đề xuất bổ sung ngân sách");
            await AddPermission(posmPermission.Id, "PosmInvestments.AsmConfirmSuggest", "Asm đuyệt đề xuất ngân sách");
            await AddPermission(posmPermission.Id, "PosmInvestments.RsmConfirmSuggest", "Rsm đuyệt đề xuất ngân sách");
            await AddPermission(posmPermission.Id, "PosmInvestments.TradeConfirmSuggest", "Trade đuyệt đề xuất ngân sách");
            await AddPermission(posmPermission.Id, "PosmInvestments.MarketingConfirmProduce", "Xác nhận sản xuất 1");
            await AddPermission(posmPermission.Id, "PosmInvestments.SupConfirmProduce", "Xác nhận sản xuất 2");
            await AddPermission(posmPermission.Id, "PosmInvestments.SupplyConfirmProduce", "Xác nhận NCC sản xuất");
            await AddPermission(posmPermission.Id, "PosmInvestments.Accept", "Nghiệm thu");
            await AddPermission(posmPermission.Id, "PosmInvestments.ConfirmAccept1", "Xác nhận nghiệm thu lần 1");
            await AddPermission(posmPermission.Id, "PosmInvestments.ConfirmAccept2", "Xác nhận nghiệm thu lần 2");
            await AddPermission(posmPermission.Id, "PosmInvestments.ImportHistory", "Import lịch sử đầu tư");
            //Chấm điểm của Sales
            await AddPermission(posmPermission.Id, "PosmInvestments.SalesRemark", "Chấm điểm của Sales");
            //Chấm điểm của công ty
            await AddPermission(posmPermission.Id, "PosmInvestments.CompanyRemark", "Chấm điểm của CTY");
            // Report
            await AddPermission(null, "Notifications", "Thông báo");
            var reportPermission = await AddPermission(null, "Reports", "Báo cáo");
            await AddPermission(reportPermission.Id, "Reports.TicketInvestments.Result", "Kết quả thực hiện chương trình");
            await AddPermission(reportPermission.Id, "Reports.TicketInvestments.Ticket", "Danh sách mã BTTT");
            await AddPermission(reportPermission.Id, "Reports.TicketInvestments.Remark", "Tổng hợp đánh giá chương trình");
            await AddPermission(reportPermission.Id, "Reports.TicketInvestments.ScanQrCode", "Theo dõi quét QRCode tại Shop");

            await AddPermission(reportPermission.Id, "Reports.PosmInvestments.Register", "Danh sách Shop đề xuất POSM");
            await AddPermission(reportPermission.Id, "Reports.PosmInvestments.Order", "Danh sách đầu tư POSM");
            await AddPermission(reportPermission.Id, "Reports.PosmInvestments.Progress", "Theo dõi tiến độ đầu tư");
            await AddPermission(reportPermission.Id, "Reports.PosmInvestments.Budget", "Theo dõi ngân sách");
            await AddPermission(reportPermission.Id, "Reports.PosmInvestments.Produce", "Danh sách Shop sản xuất POSM");
            await _context.OrignalSaveAsync();

            // Assignment
            await AssignPermissionToRoles("Users", adminRole);
            await AssignPermissionToRoles("Roles", adminRole);
            await AssignPermissionToRoles("Brands", adminRole, salesAdminRole);
            await AssignPermissionToRoles("ProductClasses", adminRole, salesAdminRole);
            await AssignPermissionToRoles("SubProductClasses", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Provinces", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Districts", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Customers", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Customers.RegisterKeyShop", adminRole, salesSupRole, asmRole, rsmRole);
            await AssignPermissionToRoles("Customers.ApproveKeyShop", adminRole, salesAdminRole, asmRole);

            await AssignPermissionToRoles("Products", adminRole, salesAdminRole);
            await AssignPermissionToRoles("ProductPoints", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Branches", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Zones", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Areas", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Wards", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Staffs", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Staffs.UpdateSsCreditPoint", asmRole, rsmRole, tradeAdminRole, salesDirectorRole);
            await AssignPermissionToRoles("Staffs.UpdateAsmCreditPoint", rsmRole, tradeAdminRole, salesDirectorRole);
            await AssignPermissionToRoles("Staffs.UpdateRsmCreditPoint", tradeAdminRole, salesDirectorRole);

            await AssignPermissionToRoles("InvestmentSettings", adminRole, salesAdminRole);
            await AssignPermissionToRoles("RewardPackages", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Budgets", adminRole, tradeAdminRole);
            await AssignPermissionToRoles("Budgets.AllocateArea", rsmRole);
            await AssignPermissionToRoles("Budgets.AllocateBranch", asmRole);
            await AssignPermissionToRoles("ProductUnits", adminRole);
            await AssignPermissionToRoles("Dashboards", adminRole);
            await AssignPermissionToRoles("Materials", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Cycles", adminRole);
            await AssignPermissionToRoles("Channels", adminRole);
            await AssignPermissionToRoles("Reports", adminRole);
            await AssignPermissionToRoles("CustomerLocations", adminRole);
            await AssignPermissionToRoles("Vendors", adminRole);
            await AssignPermissionToRoles("PosmClasses", adminRole);
            await AssignPermissionToRoles("PosmTypes", adminRole);
            await AssignPermissionToRoles("PosmItems", adminRole);
            await AssignPermissionToRoles("PosmPrices", adminRole);

            await AssignPermissionToRoles("TicketInvestments", adminRole, salesAdminRole);
            await AssignPermissionToRoles("TicketInvestments.Register", adminRole, salesAdminRole, rsmRole, asmRole, salesSupRole);

            await AssignPermissionToRoles("TicketInvestments.ApproveRequest", adminRole, salesAdminRole, asmRole);
            await AssignPermissionToRoles("TicketInvestments.DenyRequest", adminRole, salesAdminRole, asmRole);

            await AssignPermissionToRoles("TicketInvestments.ConfirmValid1", adminRole, salesAdminRole, tradeAdminRole);
            await AssignPermissionToRoles("TicketInvestments.DenyValid1", adminRole, salesAdminRole, tradeAdminRole);

            await AssignPermissionToRoles("TicketInvestments.ConfirmValid2", adminRole, salesAdminRole, customerDevelopmentmManagerRole);
            await AssignPermissionToRoles("TicketInvestments.DenyValid2", adminRole, salesAdminRole, customerDevelopmentmManagerRole);

            await AssignPermissionToRoles("TicketInvestments.ConfirmInvestment", adminRole, salesAdminRole, rsmRole);
            await AssignPermissionToRoles("TicketInvestments.DenyInvestmentConfirmation", adminRole, salesAdminRole, rsmRole);

            await AssignPermissionToRoles("TicketInvestments.ApproveInvestment1", adminRole, salesAdminRole, tradeMarketingRole);
            await AssignPermissionToRoles("TicketInvestments.DenyInvestment1", adminRole, salesAdminRole, tradeMarketingRole);

            await AssignPermissionToRoles("TicketInvestments.ApproveInvestment2", adminRole, salesAdminRole, salesDirectorRole);
            await AssignPermissionToRoles("TicketInvestments.DenyInvestment2", adminRole, salesAdminRole, salesDirectorRole);

            await AssignPermissionToRoles("TicketInvestments.UpdateProgress", adminRole, salesAdminRole, salesSupRole);
            await AssignPermissionToRoles("TicketInvestments.Operate", adminRole, salesAdminRole, salesSupRole);
            await AssignPermissionToRoles("TicketInvestments.Accept", adminRole, salesAdminRole, leadCustomerDevelopmentRole);
            await AssignPermissionToRoles("TicketInvestments.FinalSettlement", adminRole, salesAdminRole, customerDevelopmentAdminRole);

            await AssignPermissionToRoles("TicketInvestments.SalesRemark", adminRole, asmRole);
            await AssignPermissionToRoles("TicketInvestments.CustomerDevelopmentRemark", adminRole, customerDevelopmentmManagerRole);
            await AssignPermissionToRoles("TicketInvestments.CompanyRemark", adminRole, tradeAdminRole);

            await AssignPermissionToRoles("Notifications", adminRole, salesAdminRole);

            await AssignPermissionToRoles("Orders", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Orders.Create", shopRole);
            await AssignPermissionToRoles("Tickets", adminRole);
            await AssignPermissionToRoles("AppSettings", adminRole, salesAdminRole);

            await AssignPermissionToRoles("Reports", adminRole, salesAdminRole);
            await AssignPermissionToRoles("Reports.TicketInvestments.Result", adminRole, salesAdminRole, asmRole, rsmRole, tradeAdminRole, tradeMarketingRole, salesDirectorRole, customerDevelopmentAdminRole, customerDevelopmentmManagerRole);
            await AssignPermissionToRoles("Reports.TicketInvestments.Ticket", adminRole, salesAdminRole, tradeAdminRole, tradeMarketingRole, customerDevelopmentAdminRole, customerDevelopmentmManagerRole);
            await AssignPermissionToRoles("Reports.TicketInvestments.Remark", adminRole, salesAdminRole, tradeAdminRole, tradeMarketingRole, customerDevelopmentAdminRole, customerDevelopmentmManagerRole);
            await AssignPermissionToRoles("Reports.TicketInvestments.ScanQrCode", adminRole, salesAdminRole, asmRole, rsmRole, tradeAdminRole, tradeMarketingRole, salesDirectorRole, customerDevelopmentAdminRole, customerDevelopmentmManagerRole);


            await AssignPermissionToRoles("PosmInvestments", adminRole, salesAdminRole);
            await AssignPermissionToRoles("PosmInvestments.Register", adminRole, salesAdminRole, rsmRole, asmRole, salesSupRole);
            await AssignPermissionToRoles("PosmInvestments.AsmApprove", adminRole, asmRole);
            await AssignPermissionToRoles("PosmInvestments.AsmDeny", adminRole, asmRole);
            await AssignPermissionToRoles("PosmInvestments.RsmApprove", adminRole, rsmRole);
            await AssignPermissionToRoles("PosmInvestments.RsmDeny", adminRole, rsmRole);
            await AssignPermissionToRoles("PosmInvestments.TradeApprove", adminRole, tradeMarketingRole);
            await AssignPermissionToRoles("PosmInvestments.TradeDeny", adminRole, tradeMarketingRole);
            await AssignPermissionToRoles("PosmInvestments.DirectorApprove", adminRole, developmentDirector);
            await AssignPermissionToRoles("PosmInvestments.DirectorDeny", adminRole, developmentDirector);
            await AssignPermissionToRoles("PosmInvestments.SupplyConfirmRequest", adminRole, supplyRole);
            await AssignPermissionToRoles("PosmInvestments.SupplyDenyRequest", adminRole, supplyRole);
            await AssignPermissionToRoles("PosmInvestments.SuggestBudget", adminRole, salesSupRole);
            await AssignPermissionToRoles("PosmInvestments.AsmConfirmSuggest", adminRole, asmRole);
            await AssignPermissionToRoles("PosmInvestments.RsmConfirmSuggest", adminRole, rsmRole);
            await AssignPermissionToRoles("PosmInvestments.TradeConfirmSuggest", adminRole, tradeMarketingRole);
            await AssignPermissionToRoles("PosmInvestments.MarketingConfirmProduce", adminRole, maretkingRole);
            await AssignPermissionToRoles("PosmInvestments.SupConfirmProduce", adminRole, salesSupRole);
            await AssignPermissionToRoles("PosmInvestments.SupplyConfirmProduce", adminRole, supplyRole);
            await AssignPermissionToRoles("PosmInvestments.Accept", adminRole, salesSupRole);
            await AssignPermissionToRoles("PosmInvestments.ConfirmAccept1", adminRole, asmRole, rsmRole);
            await AssignPermissionToRoles("PosmInvestments.ConfirmAccept2", adminRole, tradeMarketingRole);
            await AssignPermissionToRoles("PosmInvestments.SalesRemark", adminRole, asmRole);
            await AssignPermissionToRoles("PosmInvestments.CompanyRemark", adminRole, tradeAdminRole);
            await AssignPermissionToRoles("PosmInvestments.ImportHistory", adminRole, tradeAdminRole,tradeMarketingRole);

            await AssignPermissionToRoles("Reports.PosmInvestments.Register", adminRole, tradeAdminRole, supplyRole);
            await AssignPermissionToRoles("Reports.PosmInvestments.Order", adminRole, tradeAdminRole);
            await AssignPermissionToRoles("Reports.PosmInvestments.Progress", adminRole, tradeAdminRole);
            await AssignPermissionToRoles("Reports.PosmInvestments.Budget", adminRole, tradeAdminRole);
            await AssignPermissionToRoles("Reports.PosmInvestments.Produce", adminRole, tradeAdminRole, supplyRole, maretkingRole);
            await _context.OrignalSaveAsync();
        }

        private async Task<Permission> AddPermission(int? parentId, string code, string name)
        {
            var permissionEntity = _context.Permissions.FirstOrDefault(p => p.Code == code);
            if (permissionEntity == null)
            {
                permissionEntity = new Permission();
                await permissionEntity.ApplyActionAsync(new UpsertPermissionAction(parentId, code, name, "", true));
                await _context.Permissions.AddAsync(permissionEntity);
            }
            return permissionEntity;
        }

        private async Task AssignPermissionToRoles(string code, params Role[] roles)
        {
            var permissionEntity = _context.Permissions.FirstOrDefault(p => p.Code == code);
            foreach (var role in roles)
            {
                var rolePermission = new RolePermission();
                await rolePermission.ApplyActionAsync(new UpsertRolePermissionAction(permissionEntity.Id));
                await role.ApplyActionAsync(new CrudPermissionToRoleAction(
                    new System.Collections.Generic.List<RolePermission>
                    {
                        rolePermission
                    },
                    new System.Collections.Generic.List<RolePermission>()
                ));
            }
        }
    }
}