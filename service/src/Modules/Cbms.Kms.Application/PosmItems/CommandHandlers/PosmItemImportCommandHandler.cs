using Aspose.Cells;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmItems.Commands;
using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmClasses.Actions;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Domain.PosmItems.Actions;
using Cbms.Kms.Domain.PosmTypes.Actions;
using Cbms.Kms.Infrastructure.Migrations;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmItems.CommandHandlers
{
    public class PosmItemImportItem
    {
        public string PosmItemCode { get; set; }
        public string PosmItemName { get; set; }
        public string PosmClassCode { get; set; }
        public string PosmClassName { get; set; }
        public string PosmTypeCode { get; set; }
        public string PosmTypeName { get; set; }
        public string Link { get; set; }
        public string UnitType { get; set; }
        public string CalcType { get; set; }
        public string IsActive { get; set; }
        public string PosmCatalogCode { get; set; }
        public string PosmCatalogName { get; set; }
        public int Line { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class PosmItemImportCommandHandler : CommandHandlerBase, IRequestHandler<PosmItemImportCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<PosmItem, int> _posmItemRepository;
        private readonly IRepository<Domain.PosmClasses.PosmClass, int> _posmClassRepository;
        private readonly IRepository<Domain.PosmTypes.PosmType, int> _posmTypeRepository;
        private readonly IAppLogger _appLogger;

        public PosmItemImportCommandHandler(
            IRequestSupplement supplement
            , IConfiguration configuration
            , IRepository<PosmItem, int> posmRepository
            , IRepository<Domain.PosmClasses.PosmClass, int> posmClassRepository
            , IRepository<Domain.PosmTypes.PosmType, int> posmTypeRepository
            , IAppLogger appLogger) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _configuration = configuration;
            _posmItemRepository = posmRepository;
            _posmClassRepository = posmClassRepository;
            _posmTypeRepository = posmTypeRepository;
            _appLogger = appLogger;
        }

        public async Task<Unit> Handle(PosmItemImportCommand request, CancellationToken cancellationToken)
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

                List<PosmItemImportItem> lines = new List<PosmItemImportItem>();
              
                using (var asposeStream = new MemoryStream(Convert.FromBase64String(license)))
                {
                    asposeStream.Seek(0, SeekOrigin.Begin);
                    new License().SetLicense(asposeStream);
                    var workbook = new Workbook(path);

                    List<PosmItemDto> headers = new List<PosmItemDto>();

                    Worksheet workSheet = workbook.Worksheets[0];

                    StringBuilder stringBuilder = new StringBuilder();

                

                    var mapper = new Dictionary<string, int>() {
                        {"PosmItemCode", 0 },
                        {"PosmItemName", 1 },
                        {"PosmClassCode", 2 },
                        {"PosmClassName", 3 },
                        {"PosmTypeCode", 4 },
                        {"PosmTypeName", 5 },
                        {"Link", 6 },
                        {"UnitType", 7 },
                        {"CalcType", 8 },
                        {"IsActive", 9 },
                        {"PosmCatalogCode", 10 },
                        {"PosmCatalogName", 11 }
                    };

                    for (int line = 2; line <= workSheet.Cells.MaxDataRow + 1; line++)
                    {
                        int index = line - 1;
                        string posmItemCode = workSheet.Cells[index, mapper["PosmItemCode"]].StringValue.ToUpper().Trim();

                        if (string.IsNullOrEmpty(posmItemCode)) break;
                        string posmItemName = workSheet.Cells[index, mapper["PosmItemName"]].StringValue.ToUpper().Trim();
                        string posmClassCode = workSheet.Cells[index, mapper["PosmClassCode"]].StringValue.ToUpper().Trim();
                        string posmClassName = workSheet.Cells[index, mapper["PosmClassName"]].StringValue.ToUpper().Trim();
                        string posmTypeCode = workSheet.Cells[index, mapper["PosmTypeCode"]].StringValue.ToUpper().Trim();
                        string posmTypeName = workSheet.Cells[index, mapper["PosmTypeName"]].StringValue.ToUpper().Trim();
                        string link = workSheet.Cells[index, mapper["Link"]].StringValue.Trim();
                        string unitType = workSheet.Cells[index, mapper["UnitType"]].StringValue.Trim();
                        string calcType = workSheet.Cells[index, mapper["CalcType"]].StringValue.ToUpper().Trim();
                        string active = workSheet.Cells[index, mapper["IsActive"]].StringValue.ToUpper().Trim();

                        string posmCatalogCode = workSheet.Cells[index, mapper["PosmCatalogCode"]].StringValue.ToUpper().Trim();
                        string posmCatalogName = workSheet.Cells[index, mapper["PosmCatalogName"]].StringValue.ToUpper().Trim();
                       
                        lines.Add(new PosmItemImportItem()
                        {
                            CalcType= calcType,
                            PosmItemCode= posmItemCode,
                            PosmItemName= posmItemName,
                            PosmClassCode= posmClassCode,
                            PosmClassName= posmClassName,
                            PosmTypeCode = posmTypeCode,
                            PosmTypeName = posmTypeName,
                            IsActive = active,
                            Link = link,
                            UnitType= unitType,
                            PosmCatalogCode= posmCatalogCode,
                            PosmCatalogName= posmCatalogName,
                            Line = line
                        });
                    }
                }



                var units = new string[] { "M", "M2", "PCS" };
                var calcs = new string[] { "WH", "HD", "WHD", "F", "Q" };
                var classCodes = lines.Select(line => line.PosmClassCode).Distinct().ToList();
                foreach (var code in classCodes)
                {
                    var item = lines.LastOrDefault(p => p.PosmClassCode == code);
                    if (code.Trim().Length > 30)
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("ImportLengthInvalid", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmClassCode"), "30")
                            .Build().Message;
                        continue;
                    }

                    if (string.IsNullOrEmpty(item.PosmClassName))
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportEmpty", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmClassName"))
                            .Build().Message;
                        continue;
                    }

                    if (item.PosmClassName.Length > 200)
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                             .MessageCode("PosmItem.ImportLengthInvalid", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmClassName"), "200")
                             .Build().Message;
                        continue;
                    }

                    var posmClass = await _posmClassRepository.FirstOrDefaultAsync(p => p.Code.ToUpper() == code);
                    if (posmClass == null)
                    {

                        posmClass = Domain.PosmClasses.PosmClass.Create();
                        await _posmClassRepository.InsertAsync(posmClass);

                        await posmClass.ApplyActionAsync(new PosmClassUpsertAction(
                          code,
                          item.PosmClassName,
                          false,
                          true
                      ));
                    }
                }

                await _posmClassRepository.UnitOfWork.CommitAsync();

                var typeCodes = lines.Select(line => line.PosmTypeCode).Distinct().ToList();
                foreach (var code in typeCodes)
                {
                    var item = lines.LastOrDefault(p => p.PosmTypeCode == code);
                    if (code.Trim().Length > 30)
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("ImportLengthInvalid", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmTypeCode"), "30")
                            .Build().Message;
                        continue;
                    }

                    if (string.IsNullOrEmpty(item.PosmTypeName))
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportEmpty", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmTypeName"))
                            .Build().Message;
                        continue;
                    }

                    if (item.PosmTypeName.Length > 200)
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                             .MessageCode("PosmItem.ImportLengthInvalid", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmTypeName"), "200")
                             .Build().Message;
                        continue;
                    }

                    var posmType = await _posmTypeRepository.FirstOrDefaultAsync(p => p.Code.ToUpper() == code);
                    if (posmType == null)
                    {

                        posmType = Domain.PosmTypes.PosmType.Create();
                        await _posmTypeRepository.InsertAsync(posmType);

                        await posmType.ApplyActionAsync(new PosmTypeUpsertAction(
                          code,
                          item.PosmTypeName,
                          true
                      ));
                    }
                }

                await _posmTypeRepository.UnitOfWork.CommitAsync();

                Regex regex = new Regex("^[a-zA-Z0-9&_\\-#]*$");
                var posmItemCodes = lines.Select(line => line.PosmItemCode).Distinct().ToList();
                foreach (var code in posmItemCodes)
                {
                    var item = lines.LastOrDefault(p => p.PosmItemCode == code);
           
                    if (item.PosmItemCode.Trim().Length > 30)
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportLengthInvalid", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmItemCode"), "30")
                            .Build().Message;
                        continue;
                    }

                    if (!regex.IsMatch(code))
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportSpecialCharacter", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmItemCode"))
                            .Build().Message;
                        continue;
                    }

                    
                    if (string.IsNullOrEmpty(item.PosmItemName))
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportEmpty", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmItemName"))
                            .Build().Message;
                        continue;
                    }
                 
                    if (item.PosmItemName.Length > 200)
                    {
                       item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportLengthInvalid", item.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmItemName"), "200")
                            .Build().Message;
                        continue;
                    }


                    var posmClass = await _posmClassRepository.FirstOrDefaultAsync(p => p.Code == item.PosmClassCode);
                    var posmType = await _posmTypeRepository.FirstOrDefaultAsync(p => p.Code == item.PosmTypeCode);

                    if (string.IsNullOrEmpty(item.Link))
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportEmpty", item.Line.ToString(), LocalizationSource.GetString("PosmItem.Link"))
                            .Build().Message;
                       
                        continue;
                    }

                    PosmUnitType unitType = 0;
                    if (string.IsNullOrEmpty(item.UnitType))
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportEmpty", item.Line.ToString(), LocalizationSource.GetString("PosmItem.UnitType"))
                            .Build().Message;

                        continue;
                    }
             
                    try
                    {
                        unitType = (PosmUnitType) Enum.Parse(typeof(PosmUnitType), item.UnitType, true);
                    }
                    catch
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                         .MessageCode("PosmItem.ImportInvalid", item.Line.ToString(), LocalizationSource.GetString("PosmItem.UnitType"))
                         .Build().Message;

                        continue;
                    }
                  

                    PosmCalcType calcType = 0;

                    if (string.IsNullOrEmpty(item.CalcType))
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                            .MessageCode("PosmItem.ImportEmpty", item.Line.ToString(), LocalizationSource.GetString("PosmItem.UnitType"))
                            .Build().Message;

                        continue;
                    }

                    try
                    {
                        calcType = (PosmCalcType)Enum.Parse(typeof(PosmCalcType), item.CalcType, true);
                    }
                    catch
                    {
                        item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                         .MessageCode("PosmItem.ImportInvalid", item.Line.ToString(), LocalizationSource.GetString("PosmItem.UnitType"))
                         .Build().Message;

                        continue;
                    }


                    bool isActive = false;
                  
                    if (!string.IsNullOrEmpty(item.IsActive))
                    {
                        if (item.IsActive.ToUpper() == "TRUE" || item.IsActive.ToUpper() == "FALSE")
                        {
                            if (item.IsActive.ToUpper() == "TRUE")
                            {
                                isActive = true;
                            }
                        }
                        else
                        {
                            item.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmItem.ImportFormatBool", item.Line.ToString(), LocalizationSource.GetString("PosmItem.Status"))
                                .Build().Message;
                            continue;
                        }
                    }

                    var catalogs = lines.Where(p => p.PosmItemCode == code).ToList();
                    var catalogActions = new List<PosmCatalogUpsertAction>();
                    foreach (var catalog in catalogs)
                    {
                        if (catalog.PosmCatalogCode.Trim().Length > 30)
                        {
                            catalog.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmItem.ImportLengthInvalid", catalog.Line.ToString(), LocalizationSource.GetString("PosmItem.PosmCatalogCode"), "30")
                                .Build().Message;
                            continue;
                        }

                        if (!regex.IsMatch(catalog.PosmCatalogCode))
                        {
                            catalog.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmItem.ImportSpecialCharacter", catalog.Line.ToString(), LocalizationSource.GetString("PosmCatalog.PosmCatalogCode"))
                                .Build().Message;
                            continue;
                        }


                        if (string.IsNullOrEmpty(catalog.PosmCatalogName))
                        {
                            catalog.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                .MessageCode("PosmItem.ImportEmpty", catalog.Line.ToString(), LocalizationSource.GetString("PosmCatalog.PosmCatalogName"))
                                .Build().Message;
                            continue;
                        }

                        if (catalog.PosmCatalogName.Length > 200)
                        {
                            catalog.ErrorMessage = BusinessExceptionBuilder.Create(LocalizationSource)
                                 .MessageCode("PosmItem.ImportLengthInvalid", catalog.Line.ToString(), LocalizationSource.GetString("PosmCatalog.PosmCatalogName"), "200")
                                 .Build().Message;
                            continue;
                        }
                        catalogActions.Add(new PosmCatalogUpsertAction(null, catalog.PosmCatalogCode, catalog.PosmCatalogName, ""));
                    }


                    var possmItem = await _posmItemRepository.GetAllIncluding(p=>p.PosmCatalogs)
                        .FirstOrDefaultAsync(p => p.Code.ToUpper() == code.ToUpper());


                    if (possmItem == null)
                    {
                        possmItem = PosmItem.Create();
                        await _posmItemRepository.InsertAsync(possmItem);
                    }

                    await possmItem.ApplyActionAsync(new PosmItemUpsertAction(
                        item.PosmItemCode,
                        item.PosmItemName,
                        posmClass.Id,
                        posmType.Id,
                        isActive,
                        item.Link,
                        unitType,
                        calcType,
                        catalogActions,
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
                    await _appLogger.LogErrorAsync("IMPORT_POSM_ITEM", JsonConvert.SerializeObject(errorItem));
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