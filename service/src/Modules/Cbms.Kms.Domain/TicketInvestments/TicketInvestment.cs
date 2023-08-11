using Cbms.Authorization;
using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Extensions;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Kms.Domain.Materials;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Localization;
using Cbms.Localization.Sources;
using Cbms.Runtime.Session;
using Cbms.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketInvestment : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public int CustomerId { get; private set; }
        public int RegisterStaffId { get; private set; }
        public int StockQuantity { get; private set; }
        public int RewardPackageId { get; private set; }
        public int BudgetId { get; private set; }
        public int CycleId { get; private set; }

        /// Tổng số lượng quà
        public int TicketQuantity { get; private set; }

        // Số điểm để được 1 phiếu
        public decimal PointsForTicket { get; private set; }

        // Doanh số bán dự kiến
        public decimal SalesPlanAmount { get; private set; }

        public decimal CommitmentAmount { get; private set; }
        public decimal RewardAmount { get; private set; }
        public decimal MaterialAmount { get; private set; }
        public decimal InvestmentAmount { get; private set; }
        public int PrintTicketQuantity { get; private set; }
        public int SmsTicketQuantity { get; private set; }
        public decimal ActualSalesAmount { get; private set; }

        // Ngày bắt đầu mua hàng
        public DateTime BuyBeginDate { get; private set; }

        // Ngày kết thúc mua hàng
        public DateTime BuyEndDate { get; private set; }

        // Ngày phát phiếu
        public DateTime IssueTicketBeginDate { get; private set; }

        public DateTime IssueTicketEndDate { get; private set; }

        public TicketInvestmentStatus Status { get; private set; }
        public bool IsActive { get; private set; }

        // Ngày tổ chức
        public DateTime OperationDate { get; private set; }

        public string RegisterNote { get; private set; }
        public string SurveyPhoto1 { get; private set; }
        public string SurveyPhoto2 { get; private set; }
        public string SurveyPhoto3 { get; private set; }
        public string SurveyPhoto4 { get; private set; }
        public string SurveyPhoto5 { get; private set; }
        public TicketOperation TicketOperation { get; private set; }
        public TicketAcceptance TicketAcceptance { get; private set; }
        public TicketFinalSettlement TicketFinalSettlement { get; private set; }

        public IReadOnlyCollection<TicketMaterial> Materials => _materials;
        public List<TicketMaterial> _materials = new List<TicketMaterial>();
        public IReadOnlyCollection<TicketSalesCommitment> SalesCommitments => _salesCommitments;
        public List<TicketSalesCommitment> _salesCommitments = new List<TicketSalesCommitment>();
        public IReadOnlyCollection<TicketRewardItem> RewardItems => _rewardItems;
        public List<TicketRewardItem> _rewardItems = new List<TicketRewardItem>();
        public IReadOnlyCollection<TicketProgress> Progresses => _progresses;
        public List<TicketProgress> _progresses = new List<TicketProgress>();
        public IReadOnlyCollection<TicketInvestmentHistory> Histories => _histories;
        public List<TicketInvestmentHistory> _histories = new List<TicketInvestmentHistory>();
        public IReadOnlyCollection<TicketConsumerReward> ConsumerRewards => _consumerRewards;
        public List<TicketConsumerReward> _consumerRewards = new List<TicketConsumerReward>();
        public IReadOnlyCollection<Ticket> Tickets => _tickets;
        public List<Ticket> _tickets = new List<Ticket>();

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketInvestmentUpdateAction updateAction:
                    await UpdateAsync(updateAction);
                    break;

                case TicketInvestmentRegisterAction registerAction:
                    await RegisterAsync(registerAction);
                    break;

                case TicketInvestmentApproveAction approveAction:
                    await ApproveAsync(approveAction);
                    break;

                case TicketInvestmentDenyAction denyAction:
                    await DenyAsync(denyAction);
                    break;

                case TicketProgressUpsertAction upsertProgressAction:
                    await UpsertProgressAsync(upsertProgressAction);
                    break;

                case TicketInvestmentOperateAction operateAction:
                    await OperateAsync(operateAction);
                    break;

                case TicketInvestmentAcceptAction acceptAction:
                    await AcceptAsync(acceptAction);
                    break;

                case TicketAcceptanceSalesRemarkAction salesRemarkAction:
                    await SalesRemarkAsync(salesRemarkAction);
                    break;

                case TicketAcceptanceCompanyRemarkAction companyRemarkAction:
                    await CompanyRemarkAsync(companyRemarkAction);
                    break;

                case TicketAcceptanceCustomerDevelopmentRemarkAction customerDevelopmentRemarkAction:
                    await CustomerDevelopmentRemarkAsync(customerDevelopmentRemarkAction);
                    break;

                case TicketConsumerRewardUpsertAction upsertConsumerRewardAction:
                    await UpsertConsumerRewardAsync(upsertConsumerRewardAction);
                    break;

                case TicketFinalSettlementUpsertAction upsertFinalSettlementAction:
                    await UpsertFinalSettlementAsync(upsertFinalSettlementAction);
                    break;

                case TicketGenerateAction generateAction:
                    await GenerateAsync(generateAction);
                    break;

                case TicketInvesmentUpsertPrintTicketQuantityAction generateAction:
                    await UpsertPrintTicketQuantityAsync(generateAction);
                    break;

                case TicketInvestmentClearAction clearAction:
                    await ClearAsync(clearAction);
                    break;
            }
        }

        public async Task ApproveAsync(TicketInvestmentApproveAction action)
        {
            var customerManager = action.IocResolver.Resolve<ICustomerManager>();
            var permissionChecker = action.IocResolver.Resolve<IPermissionChecker>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CustomerNotManagedByUser").Build();
            }

            if (Status == TicketInvestmentStatus.RequestInvestment)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.ApproveRequest"))
                {
                    Status = TicketInvestmentStatus.ConfirmedRequestInvestment;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ConfirmedRequestInvestment)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.ConfirmValid1"))
                {
                    Status = TicketInvestmentStatus.ValidRequestInvestment1;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ValidRequestInvestment1)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.ConfirmValid2"))
                {
                    Status = TicketInvestmentStatus.ValidRequestInvestment2;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ValidRequestInvestment2)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.ConfirmInvestment"))
                {
                    Status = TicketInvestmentStatus.ConfirmedInvestment;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ConfirmedInvestment)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.ApproveInvestment1"))
                {
                    Status = TicketInvestmentStatus.ApproveInvestment;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ApproveInvestment)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.ApproveInvestment2"))
                {
                    Status = TicketInvestmentStatus.Approved;
                    IsActive = true;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
            }

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
            await SendNotificationAsync(action.IocResolver);
        }

        public async Task DenyAsync(TicketInvestmentDenyAction action)
        {
            var customerManager = action.IocResolver.Resolve<ICustomerManager>();
            var permissionChecker = action.IocResolver.Resolve<IPermissionChecker>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CustomerNotManagedByUser").Build();
            }
            if (Status == TicketInvestmentStatus.RequestInvestment)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.DenyRequest"))
                {
                    Status = TicketInvestmentStatus.DeniedRequestInvestment;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ConfirmedRequestInvestment)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.DenyValid1"))
                {
                    Status = TicketInvestmentStatus.InValidRequestInvestment1;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ValidRequestInvestment1)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.DenyValid2"))
                {
                    Status = TicketInvestmentStatus.InValidRequestInvestment2;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ValidRequestInvestment2)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.DenyInvestmentConfirmation"))
                {
                    Status = TicketInvestmentStatus.DeniedInvestmentConfirmation;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ConfirmedInvestment)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.DenyInvestment1"))
                {
                    Status = TicketInvestmentStatus.DeniedInvestmentApproval;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else if (Status == TicketInvestmentStatus.ApproveInvestment)
            {
                if (permissionChecker.IsGranted("TicketInvestments") || permissionChecker.IsGranted("TicketInvestments.DenyInvestment2"))
                {
                    Status = TicketInvestmentStatus.Denied;
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
                }
            }
            else
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.NoPermission").Build();
            }

            var budgetManager = action.IocResolver.Resolve<IBudgetManager>();
            await budgetManager.TemporaryUseAsync(
               BudgetInvestmentType.BTTT,
               CustomerId,
               CreationTime,
               InvestmentAmount * -1
            );

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
            await SendNotificationAsync(action.IocResolver);
        }

        public async Task RegisterAsync(TicketInvestmentRegisterAction action)
        {
            var staffRepository = action.IocResolver.Resolve<IRepository<Staff, int>>();
            var staff = staffRepository.GetAll().FirstOrDefault(p => p.UserId == action.UserId);
            if (staff == null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.RegisterUserInvalid").Build();
            }
            //Check exists BTTT
            var ticketInvestmentRepository = action.IocResolver.Resolve<IRepository<TicketInvestment, int>>();
            var pendingTicket = ticketInvestmentRepository.GetAll().Where(p => p.CustomerId == action.CustomerId && p.Status != TicketInvestmentStatus.Denied && p.Status != TicketInvestmentStatus.FinalSettlement).FirstOrDefault();

            if (pendingTicket != null)
            {
                var customerRepository = action.IocResolver.Resolve<IRepository<Customer, int>>();
                var customer = await customerRepository.GetAsync(action.CustomerId);
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.PendingExists", customer.Name, pendingTicket.Code).Build();
            }

            var customerManager = action.IocResolver.Resolve<ICustomerManager>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, action.CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CustomerNotManagedByUser").Build();
            }

            if (action.BuyBeginDate == DateTime.MinValue)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.BuyBeginDateInvalid").Build();
            }

            if (action.BuyEndDate == DateTime.MinValue)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.BuyEndDateInvalid").Build();
            }

            if (action.OperationDate == DateTime.MinValue)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationDateInputInvalid").Build();
            }

            var imageResizer = action.IocResolver.Resolve<IImageResizer>();
            Status = TicketInvestmentStatus.RequestInvestment;
            Code = await action.IocResolver.Resolve<ITicketInvestmentManager>().GenerateCodeAsync(action.CustomerId);
            CustomerId = action.CustomerId;
            StockQuantity = action.StockQuantity;
            RewardPackageId = action.RewardPackageId;
            PointsForTicket = action.PointsForTicket;
            BuyBeginDate = action.BuyBeginDate.ToLocalTime().Date;
            BuyEndDate = action.BuyEndDate.ToLocalTime().EndOfDay();
            IssueTicketBeginDate = action.IssueTicketBeginDate.ToLocalTime().Date;
            OperationDate = action.OperationDate.ToLocalTime().Date;

            RegisterNote = action.RegisterNote ?? "";
            SurveyPhoto1 = action.SurveyPhoto1 != SurveyPhoto1 ? await imageResizer.ResizeBase64Image(action.SurveyPhoto1) : SurveyPhoto1;
            SurveyPhoto2 = action.SurveyPhoto2 != SurveyPhoto2 ? await imageResizer.ResizeBase64Image(action.SurveyPhoto2) : SurveyPhoto2;
            SurveyPhoto3 = action.SurveyPhoto3 != SurveyPhoto3 ? await imageResizer.ResizeBase64Image(action.SurveyPhoto3) : SurveyPhoto3;
            SurveyPhoto4 = action.SurveyPhoto4 != SurveyPhoto4 ? await imageResizer.ResizeBase64Image(action.SurveyPhoto4) : SurveyPhoto4;
            SurveyPhoto5 = action.SurveyPhoto5 != SurveyPhoto5 ? await imageResizer.ResizeBase64Image(action.SurveyPhoto5) : SurveyPhoto5;

            //Check Sales Commitment
            if (BuyEndDate < BuyBeginDate)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.BuyDateInvalid").Build();
            }

            var buyEndYearMonth = Convert.ToInt32(BuyEndDate.ToString("yyyyMM"));
            var buyDate = new DateTime(BuyBeginDate.Year, BuyBeginDate.Month, 1);
            var buyYearMonth = Convert.ToInt32(buyDate.ToString("yyyyMM"));

            int totalMonths = 0;
            do
            {
                totalMonths++;
                var buyYear = buyDate.Year;
                var buyMonth = buyDate.Month;

                var salesCommit = action.SaleCommitments.FirstOrDefault(p => p.Year == buyYear && p.Month == buyMonth);
                if (salesCommit == null)
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.SalesCommitmentNotExists", buyYear.ToString(), buyMonth.ToString()).Build();
                }
                buyDate = buyDate.AddMonths(1);
                buyYearMonth = Convert.ToInt32(buyDate.ToString("yyyyMM"));
            }
            while (buyYearMonth <= buyEndYearMonth);

            if (totalMonths != action.SaleCommitments.Count)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.SalesCommitmentNotEqual").Build();
            }

            foreach (var item in action.SaleCommitments)
            {
                var salesCommitment = new TicketSalesCommitment();
                await salesCommitment.ApplyActionAsync(new TicketSalesCommitmentUpsertAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    item.Year,
                    item.Month,
                    item.Amount)
                );

                _salesCommitments.Add(salesCommitment);
            }
            CommitmentAmount = _salesCommitments.Sum(p => p.Amount);

            var investmentSettingRespository = action.IocResolver.Resolve<IRepository<InvestmentSetting, int>>();
            var setting = investmentSettingRespository.GetAll().FirstOrDefault();
            if (setting == null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("InvestmentSetting.NotExists").Build();
            }
            IssueTicketEndDate = OperationDate.Subtract(new TimeSpan(setting.EndIssueDaysBeforeOperation, 0, 0, 0, 0));
            var validIssueBeginDate = Clock.Now.Add(new TimeSpan(setting.BeginIssueDaysAfterCurrent, 0, 0, 0, 0)).Date;
            if (IssueTicketBeginDate < validIssueBeginDate)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.IssueBeginDateInvalid", validIssueBeginDate.ToShortDateString()).Build();
            }

            if (OperationDate < new DateTime(BuyEndDate.Year, BuyEndDate.Month, 1))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationDateBuyInvalid").Build();
            }

            if (OperationDate <= IssueTicketBeginDate)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationDateInvalid", IssueTicketBeginDate.ToShortDateString()).Build();
            }

            var rewardPackageRepository = action.IocResolver.Resolve<IRepository<RewardPackage, int>>();

            var rewardPackage = rewardPackageRepository
                .GetAllIncluding(p => p.RewardItems)
                .FirstOrDefault(p => p.Id == action.RewardPackageId);

            if (rewardPackage == null)
            {
                throw new EntityNotFoundException(typeof(RewardPackage), action.RewardPackageId);
            }

            foreach (var item in rewardPackage.RewardItems)
            {
                var rewardItem = new TicketRewardItem();
                await rewardItem.ApplyActionAsync(new TicketRewardItemUpsertAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    item.Id,
                    item.Quantity,
                    item.Price)
                );
                _rewardItems.Add(rewardItem);
            }
            RewardAmount = _rewardItems.Sum(p => p.Amount);
            TicketQuantity = _rewardItems.Sum(p => p.Quantity);
            SalesPlanAmount = TicketQuantity * action.PointsForTicket * setting.AmountPerPoint;

            var materialRepository = action.IocResolver.Resolve<IRepository<Material, int>>();
            foreach (var item in action.Materials)
            {
                var material = await materialRepository.GetAsync(item.MaterialId);

                var ticketMaterial = new TicketMaterial();
                await ticketMaterial.ApplyActionAsync(new TicketMaterialUpsertAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    item.MaterialId,
                    item.RegisterQuantity,
                    material.Value,
                    item.Note)
                );

                _materials.Add(ticketMaterial);
            }
            MaterialAmount = _materials.Sum(p => p.Amount);
            InvestmentAmount = RewardAmount + MaterialAmount;

            if (setting.MaxInvestAmount < MaterialAmount)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OverMaxMaterial", setting.MaxInvestAmount.ToString("N0")).Build();
            }

            var budgetManager = action.IocResolver.Resolve<IBudgetManager>();
            var budget = await budgetManager.TemporaryUseAsync(
                BudgetInvestmentType.BTTT,
                CustomerId,
                Clock.Now.Date,
                InvestmentAmount
            );

            RegisterStaffId = staff.Id;
            BudgetId = budget.Id;
            CycleId = budget.CycleId;

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
            await SendNotificationAsync(action.IocResolver);
        }
        public async Task UpdateAsync(TicketInvestmentUpdateAction action)
        {
            if (action.OperationDate == DateTime.MinValue)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationDateInputInvalid").Build();
            }

            if (OperationDate < new DateTime(BuyEndDate.Year, BuyEndDate.Month, 1))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationDateBuyInvalid").Build();
            }

            if (OperationDate <= IssueTicketBeginDate)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationDateInvalid", IssueTicketBeginDate.ToShortDateString()).Build();
            }

            OperationDate = action.OperationDate.ToLocalTime().BeginOfDay();
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task<string> GetUserFullNameRole(IIocResolver iocResolver)
        {
            int userId = iocResolver.Resolve<ICbmsSession>().UserId.Value;
            var userManager = iocResolver.Resolve<IUserManager>();
            var user = await userManager.FindByIdAsync(userId);
            var roles = await userManager.GetRolesAsync(userId);

            return user.Name + " - " + roles.FirstOrDefault().DisplayName;
        }

        private async Task<(string, string)> GetNotificationStaffContent(IIocResolver iocResolver)
        {
            IAppSettingManager appSettingManager = iocResolver.Resolve<IAppSettingManager>();
            IRepository<Customer, int> customerRepository = iocResolver.Resolve<IRepository<Customer, int>>();
            ILocalizationSource localizationSource = iocResolver.Resolve<ILocalizationManager>().GetDefaultSource();

            var customer = await customerRepository.GetAsync(CustomerId);
            string template = (await appSettingManager.GetAsync("BTTT_NOTIFY_TEMPLATE")) ?? "";
            string roleName = await GetUserFullNameRole(iocResolver);
            string status = localizationSource.GetString("TicketInvestment.Status." + Status.ToString());

            string subjectTemplate = (await appSettingManager.GetAsync("BTTT_NOTIFY_SUBJECT_TEMPLATE")) ?? "";
            return (string.Format(subjectTemplate, Code, customer.Code + " - " + customer.Name), string.Format(template, Code, customer.Code + " - " + customer.Name, status, roleName));
        }

        private async Task<(string, string)> GetNotificationCustomerContent(IIocResolver iocResolver)
        {
            IAppSettingManager appSettingManager = iocResolver.Resolve<IAppSettingManager>();
            IRepository<Customer, int> customerRepository = iocResolver.Resolve<IRepository<Customer, int>>();
            ILocalizationSource localizationSource = iocResolver.Resolve<ILocalizationManager>().GetDefaultSource();

            var customer = await customerRepository.GetAsync(CustomerId);
         
            string subjectTemplate = (await appSettingManager.GetAsync("BTTT_NOTIFY_SHOP_SUBJECT_TEMPLATE")) ?? "";

            string content = "";
            string subject = string.Format(subjectTemplate, Code, customer.Code + " - " + customer.Name);

            if(Status == TicketInvestmentStatus.RequestInvestment)
            {
                string template = (await appSettingManager.GetAsync("BTTT_NOTIFY_SHOP_HOLDING_TEMPLATE")) ?? "";
                content = string.Format(template, Code, customer.Code + " - " + customer.Name);
            }
            else if (Status == TicketInvestmentStatus.Approved)
            {
                string template = (await appSettingManager.GetAsync("BTTT_NOTIFY_SHOP_APPROVED_TEMPLATE")) ?? "";
                content = string.Format(template, Code, customer.Code + " - " + customer.Name);
            }
            else if (Status == TicketInvestmentStatus.Denied 
                || Status == TicketInvestmentStatus.DeniedInvestmentApproval 
                || Status == TicketInvestmentStatus.DeniedInvestmentConfirmation 
                ||Status == TicketInvestmentStatus.DeniedRequestInvestment 
                || Status == TicketInvestmentStatus.InValidRequestInvestment1 
                || Status == TicketInvestmentStatus.InValidRequestInvestment2)
            {
                string template = (await appSettingManager.GetAsync("BTTT_NOTIFY_SHOP_DENIED_TEMPLATE")) ?? "";
                content = string.Format(template, Code, customer.Code + " - " + customer.Name);
            }

            return (subject, content);
        }

        private async Task SendNotificationAsync(IIocResolver iocResolver)
        {
            IStaffUserFinder staffFinder = iocResolver.Resolve<IStaffUserFinder>();
            INotificationManager notificationManager = iocResolver.Resolve<INotificationManager>();
            IRepository<Customer, int> customerRepository = iocResolver.Resolve<IRepository<Customer, int>>();

            (string subjectStaff, string contentStaff) = await GetNotificationStaffContent(iocResolver);
            (string subjectCustomer, string contentCustomer) = await GetNotificationCustomerContent(iocResolver);

            var registerStaff = await staffFinder.FindById(RegisterStaffId);
            var customer = await customerRepository.GetAsync(CustomerId);

            if (Status == TicketInvestmentStatus.RequestInvestment)
            {
                var asmStaff = await staffFinder.FindAsmOfStaff(RegisterStaffId);
                if (asmStaff != null)
                {
                    await notificationManager.CreateAndSendSync(NotificationObjectType.Sales, subjectStaff, contentStaff, contentStaff, new List<int>() { asmStaff.UserId.Value, registerStaff.UserId.Value });
                }
                await notificationManager.CreateAndSendSync(NotificationObjectType.Shop, subjectCustomer, contentCustomer, contentCustomer, new List<int>() { customer.UserId.Value });
            }
            else if (Status == TicketInvestmentStatus.DeniedRequestInvestment)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value);
                await notificationManager.CreateAndSendSync(NotificationObjectType.Shop, subjectCustomer, contentCustomer, contentCustomer, new List<int>() { customer.UserId.Value });
            }
            else if (Status == TicketInvestmentStatus.ConfirmedRequestInvestment)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value, KmsConsts.TradeAdminRole);
            }
            else if (Status == TicketInvestmentStatus.InValidRequestInvestment1)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value);
                await notificationManager.CreateAndSendSync(NotificationObjectType.Shop, subjectCustomer, contentCustomer, contentCustomer, new List<int>() { customer.UserId.Value });
            }
            else if (Status == TicketInvestmentStatus.ValidRequestInvestment1)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value, KmsConsts.CustomerDevelopmentManagerRole);
            }
            else if (Status == TicketInvestmentStatus.InValidRequestInvestment2)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value);
                await notificationManager.CreateAndSendSync(NotificationObjectType.Shop, subjectCustomer, contentCustomer, contentCustomer, new List<int>() { customer.UserId.Value });
            }
            else if (Status == TicketInvestmentStatus.ValidRequestInvestment2)
            {
                var rsmStaff = await staffFinder.FindRsmOfStaff(RegisterStaffId);
                if (rsmStaff != null)
                {
                    await notificationManager.CreateAndSendSync(NotificationObjectType.Sales, subjectStaff, contentStaff, contentStaff, new List<int>() { rsmStaff.UserId.Value, registerStaff.UserId.Value });
                }
            }
            else if (Status == TicketInvestmentStatus.DeniedInvestmentConfirmation)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value);
                await notificationManager.CreateAndSendSync(NotificationObjectType.Shop, subjectCustomer, contentCustomer, contentCustomer, new List<int>() { customer.UserId.Value });
            }
            else if (Status == TicketInvestmentStatus.ConfirmedInvestment)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value, KmsConsts.SalesDirector);
            }
            else if (Status == TicketInvestmentStatus.DeniedInvestmentApproval)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value);
                await notificationManager.CreateAndSendSync(NotificationObjectType.Shop, subjectCustomer, contentCustomer, contentCustomer, new List<int>() { customer.UserId.Value });
            }
            else if (Status == TicketInvestmentStatus.Approved)
            {
                await SendNotificationToStaffUsersAsync(iocResolver, subjectStaff, contentStaff, registerStaff.UserId.Value);
                await notificationManager.CreateAndSendSync(NotificationObjectType.Shop, subjectCustomer, contentCustomer, contentCustomer, new List<int>() { customer.UserId.Value });
            }
        }

        private async Task<List<int>> GetUserIds(IIocResolver iocResolver, int registerStaffUserId, params string[] roleNames)
        {
            IStaffUserFinder staffUserFinder = iocResolver.Resolve<IStaffUserFinder>();
            var userIds = (await staffUserFinder.FindUsersManageCustomer(CustomerId, roleNames))
                    .Select(p => p.Id)
                    .ToList();
            userIds.Add(registerStaffUserId);

            return userIds;
        }

        private async Task SendNotificationToStaffUsersAsync(IIocResolver iocResolver, string subject, string content, int registerStaffUserId, params string[] roleNames)
        {
            INotificationManager notificationManager = iocResolver.Resolve<INotificationManager>();
            var userIds = await GetUserIds(iocResolver, registerStaffUserId, roleNames);
            await notificationManager.CreateAndSendSync(NotificationObjectType.Sales, subject, content, content, userIds);
        }

        public async Task UpsertProgressAsync(TicketProgressUpsertAction action)
        {
            var customerManager = action.IocResolver.Resolve<ICustomerManager>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CustomerNotManagedByUser").Build();
            }
            if (Status != TicketInvestmentStatus.Approved && Status != TicketInvestmentStatus.Doing)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CantUpdateProgress").Build();
            }
            TicketProgress progress = null;
            if (!action.ProgressId.IsNew())
            {
                progress = _progresses.FirstOrDefault(p => p.Id == action.ProgressId);
                if (progress == null)
                {
                    throw new EntityNotFoundException(typeof(TicketProgress), action.ProgressId);
                }
            }
            else
            {
                progress = new TicketProgress();
                _progresses.Add(progress);
            }

            Status = TicketInvestmentStatus.Doing;

            await progress.ApplyActionAsync(action);

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        public async Task OperateAsync(TicketInvestmentOperateAction action)
        {
            var customerManager = action.IocResolver.Resolve<ICustomerManager>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CustomerNotManagedByUser").Build();
            }

            if (action.OperationDate.ToLocalTime() <= IssueTicketBeginDate.ToLocalTime())
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationDateInvalid", IssueTicketBeginDate.ToLocalTime().ToString("d")).Build();
            }

            if (action.OperationDate.ToString("yyyyMM") != BuyEndDate.ToString("yyyyMM"))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationDateBuyInvalid").Build();
            }

            if (Status != TicketInvestmentStatus.Doing)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.OperationInvalid").Build();
            }

            if (TicketOperation == null)
            {
                TicketOperation = new TicketOperation();
            }

            await TicketOperation.ApplyActionAsync(new TicketOperationUpsertAction(
                action.IocResolver,
                action.LocalizationSource,
                action.UserId,
                action.OperationDate.ToLocalTime(),
                action.StockQuantity,
                action.Photo1,
                action.Photo2,
                action.Photo3,
                action.Photo4,
                action.Photo5,
                action.Note
             ));

            if (action.CompleteOperation)
            {
                Status = TicketInvestmentStatus.Operated;
            }

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        public async Task AcceptAsync(TicketInvestmentAcceptAction action)
        {
            var customerManager = action.IocResolver.Resolve<ICustomerManager>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CustomerNotManagedByUser").Build();
            }

            if (action.AcceptanceDate.ToLocalTime().Date < TicketOperation.OperationDate.ToLocalTime().Date)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.AcceptanceDateInvalid", TicketOperation.OperationDate.ToLocalTime().ToString("d")).Build();
            }

            if (Status != TicketInvestmentStatus.Operated)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.AcceptanceInvalid").Build();
            }

            if (TicketAcceptance == null)
            {
                TicketAcceptance = new TicketAcceptance();
            }

            await TicketAcceptance.ApplyActionAsync(new TicketAcceptanceUpsertAction(
                action.IocResolver,
                action.LocalizationSource,
                action.UserId,
                action.AcceptanceDate.ToLocalTime().Date,
                action.ActualSalesAmount,
                action.Photo1,
                action.Photo2,
                action.Photo3,
                action.Photo4,
                action.Photo5,
                action.Note
             ));

            if (action.Complete)
            {
                Status = TicketInvestmentStatus.Accepted;
                ActualSalesAmount = await action.IocResolver.Resolve<ICustomerManager>().GetActualSalesAmountAsync(CustomerId, BuyBeginDate, BuyEndDate);

                var actualRewardAmount = (from p in _consumerRewards
                                          join r in _rewardItems on p.RewardItemId equals r.RewardItemId
                                          select new
                                          {
                                              Amount = p.RewardQuantity * r.Price
                                          }).Sum(p => p.Amount);
                var actualInvestmentAmount = actualRewardAmount + MaterialAmount;

                var budgetManager = action.IocResolver.Resolve<IBudgetManager>();
                await budgetManager.UseAsync(
                   BudgetInvestmentType.BTTT,
                   CustomerId,
                   CreationTime,
                   InvestmentAmount,
                   actualInvestmentAmount
                );

                await customerManager.ScheduleCalculateEfficientAsync(CustomerId);
            }

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        public async Task SalesRemarkAsync(TicketAcceptanceSalesRemarkAction action)
        {
            if (Status != TicketInvestmentStatus.Operated && Status != TicketInvestmentStatus.Accepted && Status != TicketInvestmentStatus.FinalSettlement)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.RemarkInvalid").Build();
            }

            await TicketAcceptance.ApplyActionAsync(action);

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        public async Task CustomerDevelopmentRemarkAsync(TicketAcceptanceCustomerDevelopmentRemarkAction action)
        {
            if (Status != TicketInvestmentStatus.Operated && Status != TicketInvestmentStatus.Accepted && Status != TicketInvestmentStatus.FinalSettlement)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.RemarkInvalid").Build();
            }

            await TicketAcceptance.ApplyActionAsync(action);

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        public async Task CompanyRemarkAsync(TicketAcceptanceCompanyRemarkAction action)
        {
            if (Status != TicketInvestmentStatus.Operated && Status != TicketInvestmentStatus.Accepted && Status != TicketInvestmentStatus.FinalSettlement)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.RemarkInvalid").Build();
            }

            await TicketAcceptance.ApplyActionAsync(action);

            var customerManager = action.IocResolver.Resolve<ICustomerManager>();
            await customerManager.ScheduleCalculateEfficientAsync(CustomerId);

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        public async Task UpsertConsumerRewardAsync(TicketConsumerRewardUpsertAction action)
        {
            var customerManager = action.IocResolver.Resolve<ICustomerManager>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CustomerNotManagedByUser").Build();
            }
            if (Status != TicketInvestmentStatus.Operated && Status != TicketInvestmentStatus.Accepted)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CantUpdateConsumerReward").Build();
            }

            var registerRewardItem = _rewardItems.FirstOrDefault(p => p.RewardItemId == action.RewardItemId);
            if (registerRewardItem == null)
            {
                var rewardItemRespository = action.IocResolver.Resolve<IRepository<RewardItem, int>>();
                var rewardItem = await rewardItemRespository.GetAsync(action.RewardItemId);
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.RewardItemNotRegister", rewardItem.Name).Build();
            }

            if (registerRewardItem.Quantity < action.Quantity)
            {
                var rewardItemRespository = action.IocResolver.Resolve<IRepository<RewardItem, int>>();
                var rewardItem = await rewardItemRespository.GetAsync(action.RewardItemId);
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.ConsumerRewardQuantityInvalid", rewardItem.Name, rewardItem.Quantity.ToString("N0")).Build();
            }

            var consumerReward = _consumerRewards.FirstOrDefault(p => p.RewardItemId == action.RewardItemId);
            if (consumerReward == null)
            {
                consumerReward = new TicketConsumerReward();
                _consumerRewards.Add(consumerReward);
            }

            action.UpdateQuantity(registerRewardItem.Quantity);

            await consumerReward.ApplyActionAsync(action);

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        public async Task UpsertFinalSettlementAsync(TicketFinalSettlementUpsertAction action)
        {
            var customerManager = action.IocResolver.Resolve<ICustomerManager>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.CustomerNotManagedByUser").Build();
            }

            if (action.Date.ToLocalTime().Date < TicketAcceptance.AcceptanceDate.ToLocalTime().Date)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.FinalSettleDateInvalid", TicketAcceptance.AcceptanceDate.ToLocalTime().ToString("d")).Build();
            }

            if (Status != TicketInvestmentStatus.Accepted)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("TicketInvestment.FinalSettleInvalid").Build();
            }

            if (TicketFinalSettlement == null)
            {
                TicketFinalSettlement = new TicketFinalSettlement();
            }

            await TicketFinalSettlement.ApplyActionAsync(action);

            if (action.Complete)
            {
                Status = TicketInvestmentStatus.FinalSettlement;
            }

            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        public async Task GenerateAsync(TicketGenerateAction action)
        {
            var ticket = new Ticket();
            await ticket.ApplyActionAsync(new TicketCreateAction(
                action.IocResolver,
                action.LocalizationSource,
                action.ConsumerPhone,
                action.ConsumerName,
                Id)
            );

            _tickets.Add(ticket);

            PrintTicketQuantity = _tickets.Where(p => p.PrintDate.HasValue).Count();
            SmsTicketQuantity = _tickets.Count();

            if (action.GetTicket != null)
            {
                action.GetTicket(ticket);
            }
        }

        public async Task LogHistoryAsync(IIocResolver iocResolver, ILocalizationSource localizationSource)
        {
            var history = new TicketInvestmentHistory();

            var data = await iocResolver.Resolve<ITicketInvestmentManager>().GetHistoryDataAsync(this); ;

            await history.ApplyActionAsync(new TicketInvestmentHistoryUpsertAction(iocResolver, localizationSource, data, Status));

            _histories.Add(history);
        }

        public async Task UpsertPrintTicketQuantityAsync(TicketInvesmentUpsertPrintTicketQuantityAction action)
        {
            PrintTicketQuantity += action.TicketId.Count;
            var tickets = action.IocResolver.Resolve<IRepository<Ticket, int>>();
            foreach (var item in action.TicketId)
            {
                var ticket = await tickets.FirstOrDefaultAsync(p => p.Id == item);
                if (ticket != null)
                {
                    await ticket.ApplyActionAsync(new TicketUpdateAction(action.StaffId));
                }
                else
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Ticket.IdNotFind", item.ToString()).Build();
                }
            }
        }

        public async Task ClearAsync(TicketInvestmentClearAction action)
        {
            TicketAcceptance = null;
            TicketOperation = null;
            TicketFinalSettlement = null;
            SurveyPhoto1 = null;
            SurveyPhoto2 = null;
            SurveyPhoto3 = null;
            SurveyPhoto4 = null;
            SurveyPhoto5 = null;
            _progresses = new List<TicketProgress>();
            _tickets = new List<Ticket>();
            _rewardItems = new List<TicketRewardItem>();
            _materials = new List<TicketMaterial>();
            _consumerRewards = new List<TicketConsumerReward>();
            _salesCommitments = new List<TicketSalesCommitment>();
        }

        public bool ValidIssueTime()
        {
            return IssueTicketEndDate.Date >= DateTime.Now.Date && IssueTicketBeginDate <= DateTime.Now.Date;
        }

        public bool IsInIssueStage()
        {
            return Status == TicketInvestmentStatus.Doing || Status == TicketInvestmentStatus.Approved;
        }

        public bool OutOfTicket()
        {
            return TicketQuantity <= Tickets.Count();
        }
    }
}