using Aspose.Cells;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmInvestments.Commands;
using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.CustomerLocations;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Cycles;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.Vendors;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.CommandHandlers
{
    public class PosmInvestmentHistoryItem
    {
        public string Code { get; set; }
        public string CustomerCode { get; set; }
        public string RegisterDate { get; set; }
        public string StaffCode { get; set; }
        public string CustomerLocationCode { get; set; }
        public string CurrentSalesAmount { get; set; }
        public string Note { get; set; }
        public string SetupContact1 { get; set; }
        public string SetupContact2 { get; set; }
        public string CommitmentSales1 { get; set; }
        public string CommitmentSales2 { get; set; }
        public string PosmItemCode { get; set; }
        public string PosmCatalogCode { get; set; }
        public string PanelShopName { get; set; }
        public string PanelShopPhone { get; set; }
        public string PanelShopAddress { get; set; }
        public string PanelShopOther { get; set; }
        public string PanelProductLine { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Depth { get; set; }
        public string SideWidth1 { get; set; }
        public string SideWidth2 { get; set; }
        public string FaceWidth { get; set; }
        public string Qty { get; set; }
        public string UnitPrice { get; set; }
        public string SetupPlanDate { get; set; }
        public string RequestType { get; set; }
        public string PrepareDate { get; set; }
        public string OperationDate { get; set; }
        public string AcceptanceDate { get; set; }
        public string VendorCode { get; set; }
        public int Line { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PosmInvestmentImportHistoryCommandHandler : CommandHandlerBase, IRequestHandler<PosmInvestmentImportHistoryCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<PosmItem, int> _posmItemRepository;
        private readonly IRepository<Staff, int> _staffRepository;
        private readonly IRepository<CustomerLocation, int> _customerLocationRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<Cycle, int> _cycleRepository;
        private readonly IRepository<PosmInvestment, int> _posmInvestmentRepository;
        private readonly IRepository<PosmCatalog, int> _posmCatalogRepository;
        private readonly IRepository<Vendor, int> _vendorRepository;
        private readonly IRepository<PosmClass, int> _posmClassRepository;
        private readonly IAppLogger _appLogger;

        public PosmInvestmentImportHistoryCommandHandler(
            IRequestSupplement supplement
            , IConfiguration configuration
            , IRepository<PosmItem, int> posmItemRepository
            , IRepository<PosmInvestment, int> posmInvestmentRepository
            , IRepository<Staff, int> staffRepository
            , IRepository<CustomerLocation, int> customerLocationRepository
            , IRepository<Customer, int> customerRepository
            , IRepository<Cycle, int> cycleRepository
            , IAppLogger appLogger
            , IRepository<PosmClass, int> posmClassRepository
            , IRepository<Vendor, int> vendorRepository
            , IRepository<PosmCatalog, int> posmCatalogRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _configuration = configuration;
            _posmItemRepository = posmItemRepository;
            _staffRepository = staffRepository;
            _customerLocationRepository = customerLocationRepository;
            _customerRepository = customerRepository;
            _posmInvestmentRepository = posmInvestmentRepository;
            _appLogger = appLogger;
            _vendorRepository = vendorRepository;
            _posmCatalogRepository = posmCatalogRepository;
            _posmClassRepository = posmClassRepository;
            _cycleRepository = cycleRepository;
        }

        public async Task<Unit> Handle(PosmInvestmentImportHistoryCommand request, CancellationToken cancellationToken)
        {
            string license = _configuration["ImportExport:License"];

            string tempPath = Path.GetTempPath();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            string path = Path.Combine(tempPath, fileName);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }

                List<PosmInvestmentHistoryItem> lines = new List<PosmInvestmentHistoryItem>();

                using (var asposeStream = new MemoryStream(Convert.FromBase64String(license)))
                {
                    asposeStream.Seek(0, SeekOrigin.Begin);
                    new License().SetLicense(asposeStream);
                    var workbook = new Workbook(path);

                    List<PosmItemDto> headers = new List<PosmItemDto>();

                    Worksheet workSheet = workbook.Worksheets[0];

                    StringBuilder stringBuilder = new StringBuilder();

                    var mapper = new Dictionary<string, int>() {
                        {"Code", 0 },
                        {"CustomerCode", 1 },
                        {"RegisterDate", 2 },
                        {"StaffCode", 3 },
                        {"CustomerLocationCode", 4},
                        {"CurrentSalesAmount",  5},
                        {"Note", 6 },
                        {"SetupContact1", 7 },
                        {"SetupContact2", 8 },
                        {"CommitmentSales1", 9 },
                        {"CommitmentSales2", 10 },
                        {"PosmItemCode", 11 },
                        {"PosmCatalogCode", 13 },
                        {"PanelShopName", 14 },
                        {"PanelShopPhone", 15 },
                        {"PanelShopAddress", 16 },
                        {"PanelShopOther", 17 },
                        {"PanelProductLine", 18 },
                        {"Width", 20 },
                        {"Height", 21 },
                        {"Depth", 22 },
                        {"SideWidth1", 23 },
                        {"SideWidth2", 24 },
                        {"FaceWidth", 25 },
                        {"Qty", 27 },
                        {"UnitPrice", 28 },
                        {"SetupPlanDate", 30 },
                        {"RequestType", 31 },
                        {"PrepareDate", 32 },
                        {"VendorCode", 33 },
                        {"OperationDate", 34 },
                        {"AcceptanceDate", 35 }
                    };

                    for (int line = 3; line <= workSheet.Cells.MaxDataRow + 1; line++)
                    {
                        int index = line - 1;
                        string code = workSheet.Cells[index, mapper["Code"]].StringValue.ToUpper().Trim();

                        if (string.IsNullOrEmpty(code)) break;
                        string customerCode = workSheet.Cells[index, mapper["CustomerCode"]].StringValue.ToUpper().Trim();
                        string registerDate = workSheet.Cells[index, mapper["RegisterDate"]].DisplayStringValue.Trim();
                        string staffCode = workSheet.Cells[index, mapper["StaffCode"]].DisplayStringValue.Trim();
                        string customerLocationCode = workSheet.Cells[index, mapper["CustomerLocationCode"]].StringValue.ToUpper().Trim();
                        string currentSalesAmount = workSheet.Cells[index, mapper["CurrentSalesAmount"]].StringValue.ToUpper().Trim();
                        string note = workSheet.Cells[index, mapper["CurrentSalesAmount"]].StringValue.Trim();
                        string setupContact1 = workSheet.Cells[index, mapper["SetupContact1"]].StringValue.Trim();
                        string setupContact2 = workSheet.Cells[index, mapper["SetupContact2"]].StringValue.Trim();
                        string commitmentSales1 = workSheet.Cells[index, mapper["CommitmentSales1"]].StringValue.Trim();
                        string commitmentSales2 = workSheet.Cells[index, mapper["CommitmentSales2"]].StringValue.Trim();
                        string posmItemCode = workSheet.Cells[index, mapper["PosmItemCode"]].StringValue.Trim();
                        string posmCatalogCode = workSheet.Cells[index, mapper["PosmCatalogCode"]].StringValue.Trim();
                        string panelShopName = workSheet.Cells[index, mapper["PanelShopName"]].StringValue.Trim();
                        string panelShopPhone = workSheet.Cells[index, mapper["PanelShopPhone"]].StringValue.Trim();
                        string panelShopAddress = workSheet.Cells[index, mapper["PanelShopAddress"]].StringValue.Trim();
                        string panelShopOther = workSheet.Cells[index, mapper["PanelShopOther"]].StringValue.Trim();
                        string panelProductLine = workSheet.Cells[index, mapper["PanelProductLine"]].StringValue.Trim();
                        string width = workSheet.Cells[index, mapper["Width"]].StringValue.Trim();
                        string height = workSheet.Cells[index, mapper["Height"]].StringValue.Trim();
                        string depth = workSheet.Cells[index, mapper["Depth"]].StringValue.Trim();
                        string sideWidth1 = workSheet.Cells[index, mapper["SideWidth1"]].StringValue.Trim();
                        string sideWidth2 = workSheet.Cells[index, mapper["SideWidth2"]].StringValue.Trim();
                        string faceWidth = workSheet.Cells[index, mapper["FaceWidth"]].StringValue.Trim();
                        string qty = workSheet.Cells[index, mapper["Qty"]].StringValue.Trim();
                        string unitPrice = workSheet.Cells[index, mapper["UnitPrice"]].StringValue.Trim();
                        string setupPlanDate = workSheet.Cells[index, mapper["SetupPlanDate"]].StringValue.Trim();
                        string requestType = workSheet.Cells[index, mapper["RequestType"]].StringValue.Trim();
                        string prepareDate = workSheet.Cells[index, mapper["PrepareDate"]].StringValue.Trim();
                        string operationDate = workSheet.Cells[index, mapper["OperationDate"]].StringValue.Trim();
                        string acceptanceDate = workSheet.Cells[index, mapper["AcceptanceDate"]].StringValue.Trim();
                        string vendorCode = workSheet.Cells[index, mapper["VendorCode"]].StringValue.Trim();
                        lines.Add(new PosmInvestmentHistoryItem()
                        {
                            Code = code,
                            CustomerCode = customerCode,
                            RegisterDate = registerDate,
                            StaffCode = staffCode,
                            CustomerLocationCode = customerLocationCode,
                            CurrentSalesAmount = currentSalesAmount,
                            Note = note,
                            SetupContact1 = setupContact1,
                            SetupContact2 = setupContact2,
                            CommitmentSales1 = commitmentSales1,
                            CommitmentSales2 = commitmentSales2,
                            PosmItemCode = posmItemCode,
                            PosmCatalogCode = posmCatalogCode,
                            PanelShopName = panelShopName,
                            PanelShopPhone = panelShopPhone,
                            PanelShopAddress = panelShopAddress,
                            PanelShopOther = panelShopOther,
                            PanelProductLine = panelProductLine,
                            Width = width,
                            Height = height,
                            Depth = depth,
                            SideWidth1 = sideWidth1,
                            SideWidth2 = sideWidth2,
                            FaceWidth = faceWidth,
                            Qty = qty,
                            UnitPrice = unitPrice,
                            SetupPlanDate = setupPlanDate,
                            RequestType = requestType,
                            PrepareDate = prepareDate,
                            OperationDate = operationDate,
                            AcceptanceDate = acceptanceDate,
                            VendorCode = vendorCode,
                            Line = line
                        });
                    }
                }

                var codes = lines.Select(line => line.Code).Distinct().ToList();
                foreach (var code in codes)
                {
                    var investment = lines.LastOrDefault(p => p.Code == code);

                    if (investment.Code.Trim().Length > 30)
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportLengthInvalid", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.Code"), "30")
                            .Build().Message;
                        continue;
                    }

                    DateTime registerDate;
                    if (string.IsNullOrEmpty(investment.RegisterDate))
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.RegisterDate"))
                            .Build().Message;

                        continue;
                    }

                    try
                    {
                        registerDate = DateTime.Parse(investment.RegisterDate, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                         .MessageCode("PosmInvestment.ImportDateInvalid", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.RegisterDate"))
                         .Build().Message;

                        continue;
                    }

                    var cycle = await _cycleRepository.FirstOrDefaultAsync(p => p.FromDate <= registerDate && p.ToDate <= registerDate);
                    if (cycle == null)
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                          .MessageCode("PosmInvestment.CycleInvalid", investment.Line.ToString(), registerDate.ToString("dd/MM/yyyy"))
                          .Build().Message;

                        continue;
                    }

                    if (string.IsNullOrEmpty(investment.CustomerCode))
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.CustomerCode"))
                            .Build().Message;

                        continue;
                    }

                    var customer = await _customerRepository.FirstOrDefaultAsync(p => p.Code == investment.CustomerCode);
                    if (customer == null)
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                        .MessageCode("ImportEntityNotFoundCode", investment.Line.ToString(), LocalizationSource.GetString("Entity_Customer"), investment.CustomerCode)
                        .Build().Message;
                        continue;
                    }

                    if (string.IsNullOrEmpty(investment.CustomerCode))
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.Customer"))
                            .Build().Message;

                        continue;
                    }

                    var staff = await _staffRepository.FirstOrDefaultAsync(p => p.Code == investment.StaffCode);
                    if (staff == null)
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                        .MessageCode("ImportEntityNotFoundCode", investment.Line.ToString(), LocalizationSource.GetString("Entity_Staff"), investment.StaffCode)
                        .Build().Message;
                        continue;
                    }

                    if (string.IsNullOrEmpty(investment.CustomerLocationCode))
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.CustomerLocationCode"))
                            .Build().Message;

                        continue;
                    }

                    var customerLocation = await _customerLocationRepository.FirstOrDefaultAsync(p => p.Code == investment.CustomerLocationCode);
                    if (customerLocation == null)
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                        .MessageCode("ImportEntityNotFoundCode", investment.Line.ToString(), LocalizationSource.GetString("Entity_CustomerLocation"), investment.CustomerLocationCode)
                        .Build().Message;
                        continue;
                    }

                    decimal currentSalesAmount = 0;
                    string currentSalesAmountRaw = investment.CurrentSalesAmount;
                    if (!string.IsNullOrEmpty(currentSalesAmountRaw))
                    {
                        try
                        {
                            currentSalesAmount = Math.Round(decimal.Parse(currentSalesAmountRaw));
                            if (currentSalesAmount < 0)
                            {
                                investment.CurrentSalesAmount = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportNumber", investment.CurrentSalesAmount.ToString(), LocalizationSource.GetString("PosmInvestment.CurrentSalesAmount"))
                                    .Build().Message;
                                continue;
                            }
                        }
                        catch
                        {
                            investment.CurrentSalesAmount = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportFormatNumber", investment.CurrentSalesAmount.ToString(), LocalizationSource.GetString("PosmInvestment.CurrentSalesAmount"))
                                .Build().Message;
                            continue;
                        }
                    }

                    if (investment.Note.Trim().Length > 1000)
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportLengthInvalid", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.Note"), "1000")
                            .Build().Message;
                        continue;
                    }

                    if (string.IsNullOrEmpty(investment.SetupContact1))
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SetupContact1"))
                            .Build().Message;

                        continue;
                    }

                    if (investment.SetupContact1.Trim().Length > 1000)
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportLengthInvalid", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SetupContact1"), "1000")
                            .Build().Message;
                        continue;
                    }

                    if (string.IsNullOrEmpty(investment.SetupContact2))
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SetupContact2"))
                            .Build().Message;

                        continue;
                    }

                    if (investment.SetupContact2.Trim().Length > 1000)
                    {
                        investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportLengthInvalid", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SetupContact2"), "1000")
                            .Build().Message;
                        continue;
                    }

                    decimal commitmentSales1 = 0;
                    string commitmentSalesRaw1 = investment.CommitmentSales1;
                    if (!string.IsNullOrEmpty(commitmentSalesRaw1))
                    {
                        try
                        {
                            commitmentSales1 = Math.Round(decimal.Parse(commitmentSalesRaw1));
                            if (commitmentSales1 < 0)
                            {
                                investment.CommitmentSales1 = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportNumber", investment.CommitmentSales1.ToString(), LocalizationSource.GetString("PosmInvestment.CommitmentSales1"))
                                    .Build().Message;
                                continue;
                            }
                        }
                        catch
                        {
                            investment.CommitmentSales1 = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportFormatNumber", investment.CommitmentSales1.ToString(), LocalizationSource.GetString("PosmInvestment.CommitmentSales1"))
                                .Build().Message;
                            continue;
                        }
                    }

                    decimal commitmentSales2 = 0;
                    string commitmentSalesRaw2 = investment.CommitmentSales2;
                    if (!string.IsNullOrEmpty(commitmentSalesRaw2))
                    {
                        try
                        {
                            commitmentSales2 = Math.Round(decimal.Parse(commitmentSalesRaw2));
                            if (commitmentSales2 < 0)
                            {
                                investment.CommitmentSales2 = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportNumber", investment.CommitmentSales2.ToString(), LocalizationSource.GetString("PosmInvestment.CommitmentSales2"))
                                    .Build().Message;
                                continue;
                            }
                        }
                        catch
                        {
                            investment.CommitmentSales2 = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmInvestment.ImportFormatNumber", investment.CommitmentSales2.ToString(), LocalizationSource.GetString("PosmInvestment.CommitmentSales2"))
                                .Build().Message;
                            continue;
                        }
                    }

                    var details = lines.Where(p => p.Code == code).ToList();
                    var detailItems = new List<PosmInvestmentImportHistoryAction.PosmItem>();
                    foreach (var detail in details)
                    {
                        if (string.IsNullOrEmpty(investment.PosmItemCode))
                        {
                            investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PosmItemCode"))
                                .Build().Message;

                            continue;
                        }

                        var posmItem = await _posmItemRepository.FirstOrDefaultAsync(p => p.Code == detail.PosmItemCode);
                        if (posmItem == null)
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                          .MessageCode("ImportEntityNotFoundCode", detail.Line.ToString(), LocalizationSource.GetString("Entity_PosmItem"), detail.PosmItemCode)
                          .Build().Message;
                            continue;
                        }

                        if (string.IsNullOrEmpty(investment.PosmCatalogCode))
                        {
                            investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PosmCatalogCode"))
                                .Build().Message;

                            continue;
                        }

                        var posmCatalog = await _posmCatalogRepository.FirstOrDefaultAsync(p => p.Code == detail.PosmCatalogCode);
                        if (posmCatalog == null)
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                          .MessageCode("ImportEntityNotFoundCode", detail.Line.ToString(), LocalizationSource.GetString("Entity_PosmCatalog"), detail.PosmCatalogCode)
                          .Build().Message;
                            continue;
                        }

                        var posmClass = await _posmClassRepository.GetAsync(posmItem.PosmClassId);

                        if (posmClass.IncludeInfo)
                        {
                            if (string.IsNullOrEmpty(detail.PanelShopName))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PanelShopName"))
                                    .Build().Message;

                                continue;
                            }

                            if (detail.PanelShopName.Trim().Length > 500)
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportLengthInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PanelShopName"), "500")
                                    .Build().Message;
                                continue;
                            }

                            if (string.IsNullOrEmpty(detail.PanelShopPhone))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PanelShopPhone"))
                                    .Build().Message;

                                continue;
                            }

                            if (detail.PanelShopPhone.Trim().Length > 50)
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportLengthInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PanelShopPhone"), "50")
                                    .Build().Message;
                                continue;
                            }

                            if (string.IsNullOrEmpty(detail.PanelShopAddress))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PanelShopPhone"))
                                    .Build().Message;

                                continue;
                            }

                            if (detail.PanelShopAddress.Trim().Length > 1000)
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportLengthInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PanelShopAddress"), "1000")
                                    .Build().Message;
                                continue;
                            }
                        }

                        decimal width = 0;
                        string widthRaw = detail.Width;
                        if (posmItem.CalcType == PosmCalcType.WH || posmItem.CalcType == PosmCalcType.WHD)
                        {
                            if (string.IsNullOrEmpty(detail.Width))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PanelShopName"))
                                    .Build().Message;

                                continue;
                            }

                            if (!string.IsNullOrEmpty(widthRaw))
                            {
                                try
                                {
                                    width = Math.Round(decimal.Parse(widthRaw));
                                    if (width < 0)
                                    {
                                        detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                        .MessageCode("PosmInvestment.ImportNumber", detail.Width.ToString(), LocalizationSource.GetString("PosmInvestment.Width"))
                                        .Build().Message;
                                        continue;
                                    }
                                }
                                catch
                                {
                                    detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportFormatNumber", detail.Width.ToString(), LocalizationSource.GetString("PosmInvestment.Width"))
                                    .Build().Message;
                                    continue;
                                }
                            }
                        }

                        decimal depth = 0;
                        string depthRaw = detail.Depth;
                        if (posmItem.CalcType == PosmCalcType.HD || posmItem.CalcType == PosmCalcType.WHD)
                        {
                            if (string.IsNullOrEmpty(depthRaw))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.Depth"))
                                    .Build().Message;

                                continue;
                            }

                            if (!string.IsNullOrEmpty(depthRaw))
                            {
                                try
                                {
                                    depth = Math.Round(decimal.Parse(depthRaw));
                                    if (depth < 0)
                                    {
                                        detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                        .MessageCode("PosmInvestment.ImportNumber", detail.Depth.ToString(), LocalizationSource.GetString("PosmInvestment.Depth"))
                                        .Build().Message;
                                        continue;
                                    }
                                }
                                catch
                                {
                                    detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportFormatNumber", detail.Depth.ToString(), LocalizationSource.GetString("PosmInvestment.Depth"))
                                    .Build().Message;
                                    continue;
                                }
                            }
                        }

                        decimal height = 0;
                        string heightRaw = detail.Height;
                        if (posmItem.CalcType == PosmCalcType.HD || posmItem.CalcType == PosmCalcType.WHD || posmItem.CalcType == PosmCalcType.WH)
                        {
                            if (string.IsNullOrEmpty(heightRaw))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.Height"))
                                    .Build().Message;

                                continue;
                            }

                            if (!string.IsNullOrEmpty(heightRaw))
                            {
                                try
                                {
                                    height = Math.Round(decimal.Parse(heightRaw));
                                    if (depth < 0)
                                    {
                                        detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                        .MessageCode("PosmInvestment.ImportNumber", detail.Height.ToString(), LocalizationSource.GetString("PosmInvestment.Height"))
                                        .Build().Message;
                                        continue;
                                    }
                                }
                                catch
                                {
                                    detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportFormatNumber", detail.Height.ToString(), LocalizationSource.GetString("PosmInvestment.Height"))
                                    .Build().Message;
                                    continue;
                                }
                            }
                        }

                        decimal sideWidth1 = 0;
                        string sideWidthRaw1 = detail.SideWidth1;

                        decimal sideWidth2 = 0;
                        string sideWidthRaw2 = detail.SideWidth2;
                        if (posmItem.CalcType == PosmCalcType.F)
                        {
                            if (string.IsNullOrEmpty(sideWidthRaw1))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SideWidth1"))
                                    .Build().Message;

                                continue;
                            }

                            if (!string.IsNullOrEmpty(sideWidthRaw1))
                            {
                                try
                                {
                                    sideWidth1 = Math.Round(decimal.Parse(sideWidthRaw1));
                                    if (sideWidth1 < 0)
                                    {
                                        detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                        .MessageCode("PosmInvestment.ImportNumber", detail.SideWidth1.ToString(), LocalizationSource.GetString("PosmInvestment.SideWidth1")).
                                        Build().Message;
                                        continue;
                                    }
                                }
                                catch
                                {
                                    detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportFormatNumber", detail.SideWidth1.ToString(), LocalizationSource.GetString("PosmInvestment.SideWidth1"))
                                    .Build().Message;
                                    continue;
                                }
                            }

                            if (string.IsNullOrEmpty(sideWidthRaw2))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SideWidth2"))
                                    .Build().Message;

                                continue;
                            }

                            if (!string.IsNullOrEmpty(sideWidthRaw2))
                            {
                                try
                                {
                                    sideWidth2 = Math.Round(decimal.Parse(sideWidthRaw2));
                                    if (sideWidth2 < 0)
                                    {
                                        detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                        .MessageCode("PosmInvestment.ImportNumber", detail.SideWidth2.ToString(), LocalizationSource.GetString("PosmInvestment.SideWidth2"))
                                        .Build().Message;
                                        continue;
                                    }
                                }
                                catch
                                {
                                    detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportFormatNumber", detail.SideWidth2.ToString(), LocalizationSource.GetString("PosmInvestment.SideWidth2"))
                                    .Build().Message;
                                    continue;
                                }
                            }
                        }

                        if (posmItem.CalcType == PosmCalcType.F)
                        {
                            if (string.IsNullOrEmpty(widthRaw))
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SideWidth1"))
                                    .Build().Message;

                                continue;
                            }

                            if (!string.IsNullOrEmpty(detail.FaceWidth))
                            {
                                try
                                {
                                    width = Math.Round(decimal.Parse(detail.FaceWidth));
                                    if (width < 0)
                                    {
                                        detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                        .MessageCode("PosmInvestment.ImportNumber", detail.FaceWidth.ToString(), LocalizationSource.GetString("PosmInvestment.FaceWidth")).
                                        Build().Message;
                                        continue;
                                    }
                                }
                                catch
                                {
                                    detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportFormatNumber", detail.FaceWidth.ToString(), LocalizationSource.GetString("PosmInvestment.FaceWidth"))
                                    .Build().Message;
                                    continue;
                                }
                            }
                        }

                        int qty = 0;
                        string qtyRaw = detail.Qty;

                        if (string.IsNullOrEmpty(qtyRaw))
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.Qty"))
                                .Build().Message;

                            continue;
                        }

                        if (!string.IsNullOrEmpty(qtyRaw))
                        {
                            try
                            {
                                qty = int.Parse(qtyRaw);
                                if (qty < 0)
                                {
                                    detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportNumber", detail.Qty.ToString(), LocalizationSource.GetString("PosmInvestment.Qty"))
                                    .Build().Message;
                                    continue;
                                }
                            }
                            catch
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportFormatNumber", detail.Qty.ToString(), LocalizationSource.GetString("PosmInvestment.Qty"))
                                .Build().Message;
                                continue;
                            }
                        }

                        decimal unitPrice = 0;
                        string unitPriceRaw = detail.UnitPrice;

                        if (string.IsNullOrEmpty(unitPriceRaw))
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.UnitPrice"))
                                .Build().Message;

                            continue;
                        }

                        if (!string.IsNullOrEmpty(unitPriceRaw))
                        {
                            try
                            {
                                unitPrice = Math.Round(decimal.Parse(unitPriceRaw));
                                if (unitPrice < 0)
                                {
                                    detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmInvestment.ImportNumber", detail.UnitPrice.ToString(), LocalizationSource.GetString("PosmInvestment.UnitPrice"))
                                    .Build().Message;
                                    continue;
                                }
                            }
                            catch
                            {
                                detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportFormatNumber", detail.UnitPrice.ToString(), LocalizationSource.GetString("PosmInvestment.UnitPrice"))
                                .Build().Message;
                                continue;
                            }
                        }

                        DateTime setupPlanDate;
                        if (string.IsNullOrEmpty(detail.SetupPlanDate))
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SetupPlanDate"))
                                .Build().Message;

                            continue;
                        }

                        try
                        {
                            setupPlanDate = DateTime.Parse(detail.SetupPlanDate, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                             .MessageCode("PosmInvestment.ImportDateInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.SetupPlanDate"))
                             .Build().Message;

                            continue;
                        }

                        PosmRequestType requestType;
                        if (string.IsNullOrEmpty(detail.RequestType))
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.RequestType"))
                                .Build().Message;

                            continue;
                        }
                        try
                        {
                            requestType = (PosmRequestType)Enum.Parse(typeof(PosmRequestType), detail.RequestType, true);
                        }
                        catch
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                             .MessageCode("PosmInvestment.ImportInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.RequestType"))
                             .Build().Message;

                            continue;
                        }

                        DateTime prepareDate;
                        if (string.IsNullOrEmpty(detail.PrepareDate))
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PrepareDate"))
                                .Build().Message;

                            continue;
                        }

                        try
                        {
                            prepareDate = DateTime.Parse(detail.PrepareDate, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                             .MessageCode("PosmInvestment.ImportDateInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.PrepareDate"))
                             .Build().Message;

                            continue;
                        }

                        if (string.IsNullOrEmpty(detail.VendorCode))
                        {
                            investment.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", investment.Line.ToString(), LocalizationSource.GetString("PosmInvestment.VendorCode"))
                                .Build().Message;

                            continue;
                        }

                        var vendor = await _vendorRepository.FirstOrDefaultAsync(p => p.Code == detail.VendorCode);
                        if (vendor == null)
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                          .MessageCode("ImportEntityNotFoundCode", detail.Line.ToString(), LocalizationSource.GetString("Entity_Vendor"), detail.VendorCode)
                          .Build().Message;
                            continue;
                        }

                        DateTime operationDate;
                        if (string.IsNullOrEmpty(detail.OperationDate))
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.OperationDate"))
                                .Build().Message;

                            continue;
                        }

                        try
                        {
                            operationDate = DateTime.Parse(detail.OperationDate, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                             .MessageCode("PosmInvestment.ImportDateInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.OperationDate"))
                             .Build().Message;

                            continue;
                        }

                        DateTime acceptanceDate;
                        if (string.IsNullOrEmpty(detail.AcceptanceDate))
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmInvestment.ImportEmpty", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.AcceptanceDate"))
                                .Build().Message;

                            continue;
                        }

                        try
                        {
                            acceptanceDate = DateTime.Parse(detail.AcceptanceDate, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                             .MessageCode("PosmInvestment.ImportDateInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmInvestment.AcceptanceDate"))
                             .Build().Message;

                            continue;
                        }

                        detailItems.Add(new PosmInvestmentImportHistoryAction.PosmItem(
                                posmItem.Id,
                                posmItem.PosmClassId,
                                posmItem.CalcType,
                                detail.PanelShopName,
                                detail.PanelShopPhone,
                                detail.PanelShopAddress,
                                detail.PanelShopOther,
                                detail.PanelProductLine,
                                posmCatalog.Id,
                                width,
                                height,
                                depth,
                                sideWidth1,
                                sideWidth2,
                                qty,
                                unitPrice,
                                setupPlanDate,
                                requestType,
                                prepareDate,
                                vendor.Id,
                                operationDate,
                                acceptanceDate
                            ));
                    }

                    var invesetment = await _posmInvestmentRepository.GetAllIncluding(p => p.Items)
                        .FirstOrDefaultAsync(p => p.Code.ToUpper() == code.ToUpper());

                    if (invesetment == null)
                    {
                        invesetment = new PosmInvestment();

                        await invesetment.ApplyActionAsync(new PosmInvestmentImportHistoryAction(
                            IocResolver,
                            LocalizationSource,
                            cycle.Id,
                            code,
                            registerDate,
                            staff.Id,
                            customer.Id,
                            customerLocation.Id,
                            currentSalesAmount,
                            investment.SetupContact1,
                            investment.SetupContact2,
                            investment.Note,
                            new List<PosmInvestmentImportHistoryAction.SalesCommitment>()
                            {
                                new PosmInvestmentImportHistoryAction.SalesCommitment(registerDate.AddMonths(1).Year,registerDate.AddMonths(1).Month, commitmentSales1),
                                new PosmInvestmentImportHistoryAction.SalesCommitment(registerDate.AddMonths(2).Year,registerDate.AddMonths(2).Month, commitmentSales1),
                            },
                            detailItems
                        ));

                        await _posmInvestmentRepository.InsertAsync(invesetment);
                    }
                }
                await _posmItemRepository.UnitOfWork.CommitAsync();

                var errorItems = lines.Where(p => !string.IsNullOrEmpty(p.ErrorMessage)).Take(10).ToList();
                if (errorItems.Count > 0)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource)
                        .MessageCode("Import.Error", string.Join(";", errorItems.Select(p => p.ErrorMessage)))
                        .Build();
                    //await _appLogger.LogErrorAsync("IMPORT_POSM_ITEM", string.Join(";", errorItems.Select(p => p.ErrorMessage)));
                }

                foreach (var errorItem in errorItems)
                {
                    await _appLogger.LogErrorAsync("IMPORT_POSM_INVEStMENT", JsonConvert.SerializeObject(errorItem));
                }
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }

            return Unit.Value;
        }
    }
}