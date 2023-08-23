using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Connection;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Cycles;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Timing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.PosmInvestments
{
    public class PosmInvestment : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public int CustomerId { get; private set; }
        public int RegisterStaffId { get; private set; }
        public int? BudgetId { get; private set; }
        public int CustomerLocationId { get; private set; }
        public decimal CurrentSalesAmount { get; private set; }
        public string ShopPanelPhoto1 { get; private set; }
        public string ShopPanelPhoto2 { get; private set; }
        public string ShopPanelPhoto3 { get; private set; }
        public string ShopPanelPhoto4 { get; private set; }
        public string VisibilityPhoto1 { get; private set; }
        public string VisibilityPhoto2 { get; private set; }
        public string VisibilityPhoto3 { get; private set; }
        public string VisibilityPhoto4 { get; private set; }
        public string VisibilityCompetitorPhoto1 { get; private set; }
        public string VisibilityCompetitorPhoto2 { get; private set; }
        public string VisibilityCompetitorPhoto3 { get; private set; }
        public string VisibilityCompetitorPhoto4 { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public string Note { get; private set; }
        public string ApproveNote { get; private set; }
        public string SetupContact1 { get; private set; }
        public string SetupContact2 { get; private set; }
        public decimal InvestmentAmount { get; private set; }
        public decimal CommitmentAmount { get; private set; }
		public decimal VTDCommitmentAmount { get; private set; }
		public decimal MilkIndustryAmount { get; private set; }
		public int CycleId { get; private set; }
        public string CancelReason { get; private set; }
		public string DesignPhoto1 { get; set; }
		public string DesignPhoto2 { get; set; }
		public string DesignPhoto3 { get; set; }
		public string DesignPhoto4 { get; set; }		

		public PosmInvestmentStatus Status { get; private set; }
        public IReadOnlyCollection<PosmSalesCommitment> SalesCommitments => _salesCommitments;
        public List<PosmSalesCommitment> _salesCommitments = new List<PosmSalesCommitment>();
        public IReadOnlyCollection<PosmInvestmentItem> Items => _items;
        public List<PosmInvestmentItem> _items = new List<PosmInvestmentItem>();

		public IReadOnlyCollection<PosmItem> PosmItems => _posmItem;
		public List<PosmItem> _posmItem = new List<PosmItem>();

		public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmInvestmentImportHistoryAction importAction:
                    await ImportHistoryAsync(importAction);
                    break;

                case PosmInvestmentRegisterAction registerAction:
                    await RegisterAsync(registerAction);
                    break;

                case PosmInvestmentAsmApproveRequestAction asmApproveAction:
                    await AsmApproveRequestAsync(asmApproveAction);
                    break;

                case PosmInvestmentAsmDenyRequestAction asmDenyAction:
                    await AsmDenyRequestAsync(asmDenyAction);
                    break;

                case PosmInvestmentRsmApproveRequestAction rsmApproveAction:
                    await RsmApproveRequestAsync(rsmApproveAction);
                    break;

                case PosmInvestmentRsmDenyRequestAction rsmDenyAction:
                    await RsmDenyRequestAsync(rsmDenyAction);
                    break;

                case PosmInvestmentTradeApproveRequestAction tradeApproveAction:
                    await TradeApproveRequestAsync(tradeApproveAction);
                    break;

                case PosmInvestmentTradeDenyRequestAction tradeDenyAction:
                    await TradeDenyRequestAsync(tradeDenyAction);
                    break;

                case PosmInvestmentDirectorApproveRequestAction directorApproveAction:
                    await DirectorApproveRequestAsync(directorApproveAction);
                    break;

                case PosmInvestmentDirectorDenyRequestAction directorDenyAction:
                    await DirectorDenyRequestAsync(directorDenyAction);
                    break;

                case PosmInvestmentSupplyConfirmRequestAction procConfirmAction:
                    await SupplyConfirmRequestAsync(procConfirmAction);
                    break;

                case PosmInvestmentSupplyDenyRequestAction procDenyAction:
                    await SupplyDenyRequestAsync(procDenyAction);
                    break;

                case PosmInvestmentSupSuggestAction supSuggest:
                    await SupSuggestAsync(supSuggest);
                    break;
                case PosmInvestmentAsmConfirmSuggestAction asmConfiirmSuggest:
                    await AsmConfirmSuggestAsync(asmConfiirmSuggest);
                    break;
                case PosmInvestmentRsmConfirmSuggestAction rsmConfiirmSuggest:
                    await RsmConfirmSuggestAsync(rsmConfiirmSuggest);
                    break;
                case PosmInvestmentTradeConfirmSuggestAction tradeConfirmSuggestAction:
                    await TradeConfirmSuggestAsync(tradeConfirmSuggestAction);
                    break;
                case PosmInvestmentMarketingConfirmProduceAction marketingnConfirmProduceAction:
                    await MarketingConfirmProduceAsync(marketingnConfirmProduceAction);
                    break;
				case PosmInvestmentMarketingConfirmProduceNewAction marketingnConfirmProduceNewAction:
					await MarketingConfirmProduceNewAsync(marketingnConfirmProduceNewAction);
					break;
				case PosmInvestmentSupConfirmProduceAction supConfirmProduceAction:
                    await SupConfirmProduceAsync(supConfirmProduceAction);
                    break;
                case PosmInvestmentSupplyConfirmProduceAction procConfirmProduceAction:
                    await ProcConfirmProduceAsync(procConfirmProduceAction);
                    break;
                case PosmInvestmentSupAcceptAction supAcceptction:
                    await SupAcceptAsync(supAcceptction);
                    break;
                case PosmInvestmentAsmConfirmAcceptAction asmConfirmAcceptAction:
                    await AsmConfirmAcceptAsync(asmConfirmAcceptAction);
                    break;
                case PosmInvestmentTradeConfirmAcceptAction tradeConfirmAcceptAction:
                    await TradeConfirmAcceptAsync(tradeConfirmAcceptAction);
                    break;
                case PosmInvestmentSalesRemarkAction salesRemarkAction:
                    await SalesRemarkAsync(salesRemarkAction);
                    break;
                case PosmInvestmentCompanyRemarkAction companyRemarkAction:
                    await CompanyRemarkAsync(companyRemarkAction);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public async Task RegisterAsync(PosmInvestmentRegisterAction action)
        {
            var staffRepository = action.IocResolver.Resolve<IRepository<Staff, int>>();
            var staff = staffRepository.GetAll().FirstOrDefault(p => p.UserId == action.UserId);
            if (staff == null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.RegisterUserInvalid").Build();
            }
            //Check exists POSM
            var posmInvestmentRepository = action.IocResolver.Resolve<IRepository<PosmInvestment, int>>();
            var pendingPosm = posmInvestmentRepository.GetAll().Where(p => p.CustomerId == action.CustomerId
            && p.Status != PosmInvestmentStatus.ASMDeniedRequest 
            && p.Status != PosmInvestmentStatus.ConfirmedAccept2
            && p.Status != PosmInvestmentStatus.RSMDeniedRequest
            && p.Status != PosmInvestmentStatus.TradeDeniedRequest
            && p.Status != PosmInvestmentStatus.DirectorDeniedRequest).FirstOrDefault();

            if (pendingPosm != null)
            {
                var customerRepository = action.IocResolver.Resolve<IRepository<Customer, int>>();
                var customer = await customerRepository.GetAsync(action.CustomerId);
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.PendingExists", customer.Name, pendingPosm.Code).Build();
            }

            var customerManager = action.IocResolver.Resolve<ICustomerManager>();

            if (!(await customerManager.IsManageByUserAsync(action.UserId, action.CustomerId)))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.CustomerNotManagedByUser").Build();
            }

            var imageResizer = action.IocResolver.Resolve<IImageResizer>();
            Status = PosmInvestmentStatus.Request;
            Code = await action.IocResolver.Resolve<IPosmInvestmentManager>().GenerateCodeAsync(action.CustomerId);
            CustomerId = action.CustomerId;
            RegisterDate = DateTime.Now;
            CustomerLocationId = action.CustomerLocationId;
            CurrentSalesAmount = action.CurrentSalesAmount;
            VTDCommitmentAmount = action.VTDCommitmentAmount;
            MilkIndustryAmount = action.MilkIndustryAmount;
            ShopPanelPhoto1 = action.ShopPanelPhoto1 != ShopPanelPhoto1 ? await imageResizer.ResizeBase64Image(action.ShopPanelPhoto1) : ShopPanelPhoto1;
            ShopPanelPhoto2 = action.ShopPanelPhoto2 != ShopPanelPhoto2 ? await imageResizer.ResizeBase64Image(action.ShopPanelPhoto2) : ShopPanelPhoto2;
            ShopPanelPhoto3 = action.ShopPanelPhoto3 != ShopPanelPhoto3 ? await imageResizer.ResizeBase64Image(action.ShopPanelPhoto3) : ShopPanelPhoto3;
            ShopPanelPhoto4 = action.ShopPanelPhoto4 != ShopPanelPhoto4 ? await imageResizer.ResizeBase64Image(action.ShopPanelPhoto4) : ShopPanelPhoto4;
            VisibilityPhoto1 = action.VisibilityPhoto1 != VisibilityPhoto1 ? await imageResizer.ResizeBase64Image(action.VisibilityPhoto1) : VisibilityPhoto1;
            VisibilityPhoto2 = action.VisibilityPhoto2 != VisibilityPhoto2 ? await imageResizer.ResizeBase64Image(action.VisibilityPhoto2) : VisibilityPhoto2;
            VisibilityPhoto3 = action.VisibilityPhoto3 != VisibilityPhoto3 ? await imageResizer.ResizeBase64Image(action.VisibilityPhoto3) : VisibilityPhoto3;
            VisibilityPhoto4 = action.VisibilityPhoto4 != VisibilityPhoto4 ? await imageResizer.ResizeBase64Image(action.VisibilityPhoto4) : VisibilityPhoto4;
            VisibilityCompetitorPhoto1 = action.VisibilityCompetitorPhoto1 != VisibilityCompetitorPhoto1 ? await imageResizer.ResizeBase64Image(action.VisibilityCompetitorPhoto1) : VisibilityCompetitorPhoto1;
            VisibilityCompetitorPhoto2 = action.VisibilityCompetitorPhoto2 != VisibilityCompetitorPhoto2 ? await imageResizer.ResizeBase64Image(action.VisibilityCompetitorPhoto2) : VisibilityCompetitorPhoto2;
            VisibilityCompetitorPhoto3 = action.VisibilityCompetitorPhoto3 != VisibilityCompetitorPhoto3 ? await imageResizer.ResizeBase64Image(action.VisibilityCompetitorPhoto3) : VisibilityCompetitorPhoto3;
            VisibilityCompetitorPhoto4 = action.VisibilityCompetitorPhoto4 != VisibilityCompetitorPhoto4 ? await imageResizer.ResizeBase64Image(action.VisibilityCompetitorPhoto4) : VisibilityCompetitorPhoto4;

            SetupContact1 = action.SetupContact1 ?? "";
            SetupContact2 = action.SetupContact2 ?? ""; ;
            Note = action.Note ?? "";

            foreach (var item in action.SalesCommitments)
            {
                var salesCommitment = new PosmSalesCommitment();
                await salesCommitment.ApplyActionAsync(new PosmSalesCommitmentUpsertAction(
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

        
            var posmItemRepository = action.IocResolver.Resolve<IRepository<PosmItem, int>>();
            var posmCatalogRepository = action.IocResolver.Resolve<IRepository<PosmCatalog, int>>();
            foreach (var item in action.Items)
            {
                var posmCatalog = await posmCatalogRepository.GetAsync(item.PosmCatalogId);
                var posmItem = await posmItemRepository.GetAsync(posmCatalog.PosmItemId);
                var investmentItem = new PosmInvestmentItem();
                await investmentItem.ApplyActionAsync(new PosmInvestmentItemRegisterAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    posmItem.PosmClassId,
                    posmItem.Id,
                    item.PanelShopName,
                    item.PanelShopPhone,
                    item.PanelShopAddress,
                    item.PanelOtherInfo,
                    item.PanelProductLine,
                    item.PosmCatalogId,
                    item.Width,
                    item.Height,
                    item.Depth,
                    item.SideWidth1,
                    item.SideWidth2,
                    item.Qty,
                    item.SetupPlanDate,
                    item.RequestType,
                    item.RequestReason,
                    item.Photo1,
                    item.Photo2,
                    item.Photo3,
                    item.Photo4
                    )
                );

                _items.Add(investmentItem);
            }
            InvestmentAmount = _items.Sum(p => p.TotalCost);

            var budgetManager = action.IocResolver.Resolve<IBudgetManager>();
            var budget = await budgetManager.TemporaryUseAsync(
                BudgetInvestmentType.POSM,
                CustomerId,
                Clock.Now.Date,
                InvestmentAmount
            );

            RegisterStaffId = staff.Id;
            BudgetId = budget.Id;
            CycleId = budget.CycleId;

        }

        public async Task ImportHistoryAsync(PosmInvestmentImportHistoryAction action)
        {
            var staffRepository = action.IocResolver.Resolve<IRepository<Staff, int>>();
            var staff = staffRepository.GetAll().FirstOrDefault(p => p.Id == action.RegisterStaffId);
            if (staff == null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.RegisterUserInvalid").Build();
            }
            //Check exists POSM
            var posmInvestmentRepository = action.IocResolver.Resolve<IRepository<PosmInvestment, int>>();
            var pendingPosm = posmInvestmentRepository.GetAll().Where(p => p.CustomerId == action.CustomerId && p.Status != PosmInvestmentStatus.ASMDeniedRequest && p.Status != PosmInvestmentStatus.ConfirmedAccept2).FirstOrDefault();

            if (pendingPosm != null)
            {
                var customerRepository = action.IocResolver.Resolve<IRepository<Customer, int>>();
                var customer = await customerRepository.GetAsync(action.CustomerId);
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.PendingExists", customer.Name, pendingPosm.Code).Build();
            }

            var imageResizer = action.IocResolver.Resolve<IImageResizer>();
            Status = PosmInvestmentStatus.Request;
            Code = action.Code;
            CustomerId = action.CustomerId;
            RegisterDate = action.RegisterDate;
            CustomerLocationId = action.CustomerLocationId;
            CurrentSalesAmount = action.CurrentSalesAmount;
            
            SetupContact1 = action.SetupContact1 ?? "";
            SetupContact2 = action.SetupContact2 ?? ""; ;
            Note = action.Note ?? "";

            foreach (var item in action.SalesCommitments)
            {
                var salesCommitment = new PosmSalesCommitment();
                await salesCommitment.ApplyActionAsync(new PosmSalesCommitmentUpsertAction(
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


            var posmItemRepository = action.IocResolver.Resolve<IRepository<PosmItem, int>>();
            var posmCatalogRepository = action.IocResolver.Resolve<IRepository<PosmCatalog, int>>();
            foreach (var item in action.Items)
            {
                var posmCatalog = await posmCatalogRepository.GetAsync(item.PosmCatalogId);
                var posmItem = await posmItemRepository.GetAsync(posmCatalog.PosmItemId);
                var investmentItem = new PosmInvestmentItem();
                await investmentItem.ApplyActionAsync(new PosmInvestmentItemImportHistoryAction(
                    action.IocResolver,
                    action.LocalizationSource,
                    item.PosmItemId,
                    item.PosmClassId,
                    item.CalcType,
                    item.PanelShopName,
                    item.PanelShopPhone,
                    item.PanelShopAddress,
                    item.PanelOtherInfo,
                    item.PanelProductLine,
                    item.PosmCatalogId,
                    item.Width,
                    item.Height,
                    item.Depth,
                    item.SideWidth1,
                    item.SideWidth2,
                    item.Qty,
                    item.UnitPrice,
                    item.SetupPlanDate,
                    item.RequestType,
                    item.PrepareDate,
                    item.VendorId,
                    item.OperationDate,
                    item.AcceptanceDate
                    )
                );

                _items.Add(investmentItem);
            }

            InvestmentAmount =  _items.Sum(p => p.TotalCost);

            RegisterStaffId = staff.Id;

            CycleId = action.CycleId;
            Status = PosmInvestmentStatus.ConfirmedAccept2;
        }

        public async Task AsmApproveRequestAsync(PosmInvestmentAsmApproveRequestAction action)
        {
            if (Status != PosmInvestmentStatus.Request)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            Status = PosmInvestmentStatus.AsmApprovedRequest;
            ApproveNote = action.Note ?? "";

            _items.ForEach(async item => await item.ApplyActionAsync(action));
        }

        public async Task AsmDenyRequestAsync(PosmInvestmentAsmDenyRequestAction action)
        {
            if (Status != PosmInvestmentStatus.Request)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            Status = PosmInvestmentStatus.ASMDeniedRequest;
            ApproveNote = action.Note ?? "";

            _items.ForEach(async item => await item.ApplyActionAsync(action));
        }

        public async Task RsmApproveRequestAsync(PosmInvestmentRsmApproveRequestAction action)
        {
            if (Status != PosmInvestmentStatus.AsmApprovedRequest)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            Status = PosmInvestmentStatus.RsmApprovedRequest;
            ApproveNote = action.Note ?? "";

            _items.ForEach(async item => await item.ApplyActionAsync(action));
        }

        public async Task RsmDenyRequestAsync(PosmInvestmentRsmDenyRequestAction action)
        {
            if (Status != PosmInvestmentStatus.AsmApprovedRequest)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            Status = PosmInvestmentStatus.RSMDeniedRequest;
            ApproveNote = action.Note ?? "";

            _items.ForEach(async item => await item.ApplyActionAsync(action));
        }

        public async Task TradeApproveRequestAsync(PosmInvestmentTradeApproveRequestAction action)
        {
            if (Status != PosmInvestmentStatus.RsmApprovedRequest)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            Status = PosmInvestmentStatus.TradeApprovedRequest;
            ApproveNote = action.Note ?? "";

            _items.ForEach(async item => await item.ApplyActionAsync(action));
        }

        public async Task TradeDenyRequestAsync(PosmInvestmentTradeDenyRequestAction action)
        {
            if (Status != PosmInvestmentStatus.RsmApprovedRequest)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            Status = PosmInvestmentStatus.TradeDeniedRequest;
            ApproveNote = action.Note ?? "";

            _items.ForEach(async item => await item.ApplyActionAsync(action));
        }

        public async Task DirectorApproveRequestAsync(PosmInvestmentDirectorApproveRequestAction action)
        {
            if (Status != PosmInvestmentStatus.TradeApprovedRequest)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }

            Status = PosmInvestmentStatus.DirectorApprovedRequest;

            _items.ForEach(async item => await item.ApplyActionAsync(action));
        }

        public async Task DirectorDenyRequestAsync(PosmInvestmentDirectorDenyRequestAction action)
        {
            if (Status != PosmInvestmentStatus.TradeApprovedRequest)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            Status = PosmInvestmentStatus.DirectorDeniedRequest;
            Note = action.Note ?? "";
            _items.ForEach(async item => await item.ApplyActionAsync(action));
        }

        public async Task SupplyConfirmRequestAsync(PosmInvestmentSupplyConfirmRequestAction action)
        {
            if (Status != PosmInvestmentStatus.DirectorApprovedRequest && Status != PosmInvestmentStatus.InvalidOrder)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }

            await item.ApplyActionAsync(action);

            InvestmentAmount = _items.Sum(p => p.TotalCost);
            if (_items.Count(p => p.Status == PosmInvestmentItemStatus.ValidOrder) == _items.Count)
            {
                Status = PosmInvestmentStatus.ValidOrder;
            }
        }

        public async Task SupplyDenyRequestAsync(PosmInvestmentSupplyDenyRequestAction action)
        {
            if (Status != PosmInvestmentStatus.DirectorApprovedRequest)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ActionInvalid").Build();
            }
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }

            await item.ApplyActionAsync(action);

            if (_items.Count(p => p.Status == PosmInvestmentItemStatus.InvalidOrder) == _items.Count)
            {
                Status = PosmInvestmentStatus.InvalidOrder;
            }
        }

        public async Task SupSuggestAsync(PosmInvestmentSupSuggestAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }

            await item.ApplyActionAsync(action);
        }

        public async Task AsmConfirmSuggestAsync(PosmInvestmentAsmConfirmSuggestAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }

            await item.ApplyActionAsync(action);
        }

        public async Task RsmConfirmSuggestAsync(PosmInvestmentRsmConfirmSuggestAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }

            await item.ApplyActionAsync(action);
        }
        public async Task TradeConfirmSuggestAsync(PosmInvestmentTradeConfirmSuggestAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }

            await item.ApplyActionAsync(action);
        }

        public async Task MarketingConfirmProduceAsync(PosmInvestmentMarketingConfirmProduceAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }

            await item.ApplyActionAsync(action);

            if (_items.Count(p => p.Status == PosmInvestmentItemStatus.ConfirmedProduce1) == _items.Count)
            {
                Status = PosmInvestmentStatus.ConfirmedProduce1;
            }
        }

		public async Task MarketingConfirmProduceNewAsync(PosmInvestmentMarketingConfirmProduceNewAction action)
		{
			//var posmItemRepository = action.IocResolver.Resolve<IRepository<PosmItem, int>>();		
			var item = _items.Where(p => p.PosmInvestmentId == action.PosmInvestmentId);
			if (item == null)
			{
				throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentId);
			}

            foreach (var _item in item)
            {
                var posmInvestmentItem = _items.FirstOrDefault(p => p.Id == _item.Id);
                if (posmInvestmentItem == null)
                {
                    throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentId);
                }

                if (string.IsNullOrEmpty(posmInvestmentItem.OperationPhoto1) && string.IsNullOrEmpty(posmInvestmentItem.OperationPhoto2)
                    && string.IsNullOrEmpty(posmInvestmentItem.OperationPhoto3) && string.IsNullOrEmpty(posmInvestmentItem.OperationPhoto4))
                {
                    //var posmItem = await posmItemRepository.FirstOrDefaultAsync(p => p.Id == _item.PosmItemId);
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmItem.OperationPhotoNotNull").Build();
                }

                await posmInvestmentItem.ApplyActionAsync(action);
            }			

			if (_items.Count(p => p.Status == PosmInvestmentItemStatus.ConfirmedProduce1) == _items.Count)
			{
				Status = PosmInvestmentStatus.ConfirmedProduce1;

				var imageResizer = action.IocResolver.Resolve<IImageResizer>();
				string imgObject = "PosmInvestmentDesign";
                if (!string.IsNullOrEmpty(action.DesignPhoto1)) {
					string imgPathPhoto1 = await imageResizer.SaveImgFromBase64(imgObject, action.PosmInvestmentId.ToString(), 
                        action.DesignPhoto1, DesignPhoto1, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath);
					DesignPhoto1 = imgPathPhoto1;
				}

				if (!string.IsNullOrEmpty(action.DesignPhoto2))
				{
					string imgPathPhoto2 = await imageResizer.SaveImgFromBase64(imgObject, action.PosmInvestmentId.ToString(),
						action.DesignPhoto2, DesignPhoto2, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath);
					DesignPhoto2 = imgPathPhoto2;
				}

				if (!string.IsNullOrEmpty(action.DesignPhoto3))
				{
					string imgPathPhoto3 = await imageResizer.SaveImgFromBase64(imgObject, action.PosmInvestmentId.ToString(),
						action.DesignPhoto3, DesignPhoto3, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath);
					DesignPhoto3 = imgPathPhoto3;
				}

				if (!string.IsNullOrEmpty(action.DesignPhoto4))
				{
					string imgPathPhoto4 = await imageResizer.SaveImgFromBase64(imgObject, action.PosmInvestmentId.ToString(),
						action.DesignPhoto4, DesignPhoto4, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath);
					DesignPhoto4 = imgPathPhoto4;
				}
			}

		}

		public async Task SupConfirmProduceAsync(PosmInvestmentSupConfirmProduceAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }
            await item.ApplyActionAsync(action);
            if (_items.Count(p => p.Status == PosmInvestmentItemStatus.ConfirmedProduce2) == _items.Count)
            {
                Status = PosmInvestmentStatus.ConfirmedProduce2;
            }
          
        }

        public async Task ProcConfirmProduceAsync(PosmInvestmentSupplyConfirmProduceAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }
            await item.ApplyActionAsync(action);
            if (_items.Count(p => p.Status == PosmInvestmentItemStatus.ConfirmedVendorProduce) == _items.Count)
            {
                Status = PosmInvestmentStatus.ConfirmedVendorProduce;
            }
          
        }

        public async Task SupAcceptAsync(PosmInvestmentSupAcceptAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }
            await item.ApplyActionAsync(action);
            if (_items.Count(p => p.Status == PosmInvestmentItemStatus.Accepted) == _items.Count)
            {
                Status = PosmInvestmentStatus.Accepted;
            }

            
        }

        public async Task AsmConfirmAcceptAsync(PosmInvestmentAsmConfirmAcceptAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }
            await item.ApplyActionAsync(action);
            if (_items.Count(p => p.Status == PosmInvestmentItemStatus.ConfirmedAccept1) == _items.Count)
            {
                Status = PosmInvestmentStatus.ConfirmAccept1;
            }

           
        }

        public async Task TradeConfirmAcceptAsync(PosmInvestmentTradeConfirmAcceptAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }
            await item.ApplyActionAsync(action);
            if (_items.Count(p => p.Status == PosmInvestmentItemStatus.ConfirmedAccept2) == _items.Count)
            {
                Status = PosmInvestmentStatus.ConfirmedAccept2;

                var budgetManager = action.IocResolver.Resolve<IBudgetManager>();
                await budgetManager.UseAsync(
                   BudgetInvestmentType.POSM,
                   CustomerId,
                   CreationTime,
                   InvestmentAmount,
                   InvestmentAmount
                );
            }

           
        }

        public async Task SalesRemarkAsync(PosmInvestmentSalesRemarkAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }
            await item.ApplyActionAsync(action);
        }

        public async Task CompanyRemarkAsync(PosmInvestmentCompanyRemarkAction action)
        {
            var item = _items.FirstOrDefault(p => p.Id == action.PosmInvestmentItemId);
            if (item == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), action.PosmInvestmentItemId);
            }
            await item.ApplyActionAsync(action);
        }
    }
}