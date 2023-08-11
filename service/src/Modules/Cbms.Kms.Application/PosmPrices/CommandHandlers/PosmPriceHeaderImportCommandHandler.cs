using Aspose.Cells;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmItems.Commands;
using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Kms.Application.PosmPrices.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmClasses.Actions;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Domain.PosmItems.Actions;
using Cbms.Kms.Domain.PosmPrices;
using Cbms.Kms.Domain.PosmPrices.Actions;
using Cbms.Kms.Infrastructure.Migrations;
using Cbms.Localization.Sources;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static IdentityServer4.Models.IdentityResources;

namespace Cbms.Kms.Application.PosmPrices.CommandHandlers
{
    public class PosmPriceHeaderImportItem
    {
        public string PosmItemCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Line { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class PosmPriceHeaderImportCommandHandler : CommandHandlerBase, IRequestHandler<PosmPriceHeaderImportCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<PosmItem, int> _posmItemRepository;
        private readonly IRepository<PosmPriceHeader, int> _posmPriceHeaderRepository;
        private readonly IAppLogger _appLogger;

        public PosmPriceHeaderImportCommandHandler(
            IRequestSupplement supplement
            , IConfiguration configuration
            , IRepository<PosmItem, int> posmItemRepository
            , IRepository<PosmPriceHeader, int> posmPriceHeaderRepository
            , IAppLogger appLogger) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _configuration = configuration;
            _posmItemRepository = posmItemRepository;
            _posmPriceHeaderRepository = posmPriceHeaderRepository;
            _appLogger= appLogger;
        }

        public async Task<Unit> Handle(PosmPriceHeaderImportCommand request, CancellationToken cancellationToken)
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

                List<PosmPriceHeaderImportItem> lines = new List<PosmPriceHeaderImportItem>();
              
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
                        {"Name", 1 },
                        {"FromDate", 2 },
                        {"ToDate", 3 },
                        {"PosmItemCode", 4 },
                        {"Price", 8}
                    };

                    for (int line = 2; line <= workSheet.Cells.MaxDataRow + 1; line++)
                    {
                        int index = line - 1;
                        string code = workSheet.Cells[index, mapper["Code"]].StringValue.ToUpper().Trim();

                        if (string.IsNullOrEmpty(code)) break;
                        string name = workSheet.Cells[index, mapper["Name"]].StringValue.ToUpper().Trim();
                        string fromDate = workSheet.Cells[index, mapper["FromDate"]].DisplayStringValue.Trim();
                        string toDate = workSheet.Cells[index, mapper["ToDate"]].DisplayStringValue.Trim();
                        string itemCode = workSheet.Cells[index, mapper["PosmItemCode"]].StringValue.ToUpper().Trim();
                        string price = workSheet.Cells[index, mapper["Price"]].StringValue.ToUpper().Trim();
                       
                        lines.Add(new PosmPriceHeaderImportItem()
                        {
                            PosmItemCode= itemCode,
                            Code = code,
                            Name = name,
                            FromDate = fromDate,
                            ToDate = toDate,
                            Price = price,
                            Line = line
                        });
                    }
                }



                var units = new string[] { "M", "M2", "PCS" };
                var calcs = new string[] { "WH", "HD", "WHD", "F", "Q" };

                Regex regex = new Regex("^[a-zA-Z0-9&_\\-#]*$");
                var codes = lines.Select(line => line.Code).Distinct().ToList();
                foreach (var code in codes)
                {
                    var header = lines.LastOrDefault(p => p.Code == code);
           
                    if (header.Code.Trim().Length > 30)
                    {
                        header.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmPrice.ImportLengthInvalid", header.Line.ToString(), LocalizationSource.GetString("PosmPrice.Code"), "30")
                            .Build().Message;
                        continue;
                    }

                    if (!regex.IsMatch(code))
                    {
                        header.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmPrice.ImportSpecialCharacter", header.Line.ToString(), LocalizationSource.GetString("PosmPrice.Code"))
                            .Build().Message;
                        continue;
                    }

                    
                    if (string.IsNullOrEmpty(header.Name))
                    {
                        header.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmPrice.ImportEmpty", header.Line.ToString(), LocalizationSource.GetString("PosmPrice.Name"))
                            .Build().Message;
                        continue;
                    }
                 
                    if (header.Name.Length > 200)
                    {
                       header.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmPrice.ImportLengthInvalid", header.Line.ToString(), LocalizationSource.GetString("PosmPrice.Name"), "200")
                            .Build().Message;
                        continue;
                    }

                    DateTime fromDate;
                    if (string.IsNullOrEmpty(header.FromDate))
                    {
                        header.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmPrice.ImportEmpty", header.Line.ToString(), LocalizationSource.GetString("PosmPrice.FromDate"))
                            .Build().Message;

                        continue;
                    }
             
                    try
                    {
                        fromDate = DateTime.Parse(header.FromDate, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        header.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                         .MessageCode("PosmPrice.ImportDateInvalid", header.Line.ToString(), LocalizationSource.GetString("PosmPrice.FromDate"))
                         .Build().Message;

                        continue;
                    }


                    DateTime toDate;
                    if (string.IsNullOrEmpty(header.ToDate))
                    {
                        header.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmPrice.ImportEmpty", header.Line.ToString(), LocalizationSource.GetString("PosmPrice.ToDate"))
                            .Build().Message;

                        continue;
                    }

                    try
                    {
                        toDate = DateTime.Parse(header.ToDate, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        header.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                         .MessageCode("PosmPrice.ImportDateInvalid", header.Line.ToString(), LocalizationSource.GetString("PosmPrice.ToDate"))
                         .Build().Message;
                        continue;
                    }


                  
                    var details = lines.Where(p => p.Code == code).ToList();
                    var detailActions = new List<PosmPriceDetailUpsertAction>();
                    foreach (var detail in details)
                    {
                        if (detail.PosmItemCode.Trim().Length > 30)
                        {
                            detail.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmItem.ImportLengthInvalid", detail.Line.ToString(), LocalizationSource.GetString("PosmPrice.PosmItemCode"), "30")
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

                        decimal price = 0;
                        string priceRaw = detail.Price;
                        if (!string.IsNullOrEmpty(priceRaw))
                        {
                            try
                            {
                                price = Math.Round(decimal.Parse(priceRaw));
                                if (price < 0)
                                {
                                    detail.Price = BusinessExceptionBuilder.Create(LocalizationSource)
                                    .MessageCode("PosmPrice.ImportNumber", detail.Price.ToString(), LocalizationSource.GetString("PosmPrice.Price"))
                                        .Build().Message;
                                    continue;
                                }
                            }
                            catch
                            {
                                detail.Price = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmPrice.ImportFormatNumber", detail.Price.ToString(), LocalizationSource.GetString("PosmPrice.Price"))
                                    .Build().Message;
                                continue;
                            }
                        }


                        detailActions.Add(new PosmPriceDetailUpsertAction(null, posmItem.Id, price));
                    }


                    var priceHeader = await _posmPriceHeaderRepository.GetAllIncluding(p=>p.PosmPriceDetails)
                        .FirstOrDefaultAsync(p => p.Code.ToUpper() == code.ToUpper());

                    if (priceHeader == null)
                    {
                        priceHeader = PosmPriceHeader.Create();
                        await _posmPriceHeaderRepository.InsertAsync(priceHeader);
                    }



                    await priceHeader.ApplyActionAsync(new PosmPriceUpsertAction(
                        header.Code,
                        header.Name,
                        fromDate,
                        toDate,
                        true,
                        detailActions,
                        new List<int>()
                    ));

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
                    await _appLogger.LogErrorAsync("IMPORT_POSM_PRICE", JsonConvert.SerializeObject(errorItem));
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