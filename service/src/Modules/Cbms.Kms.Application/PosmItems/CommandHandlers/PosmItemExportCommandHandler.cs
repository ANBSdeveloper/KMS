using Aspose.Cells;
using Cbms.Kms.Application.PosmItems.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmItems.CommandHandlers
{
    public class PosmItemExportCommandHandler : CommandHandlerBase, IRequestHandler<PosmItemExportCommand, string>
    {
        private readonly IConfiguration _configuration;
   
        private AppDbContext _dbContext;

        public PosmItemExportCommandHandler(IRequestSupplement supplement, IConfiguration configuration, AppDbContext dbContext) : base(supplement)
        {
           
            _configuration = configuration;
            _dbContext = dbContext;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<string> Handle(PosmItemExportCommand request, CancellationToken cancellationToken)
        {
            var downloadFolder = Path.GetTempPath();

            string license = _configuration["ImportExport:License"];
            using (var asposeStream = new MemoryStream(Convert.FromBase64String(license)))
            {
                asposeStream.Seek(0, SeekOrigin.Begin);
                new License().SetLicense(asposeStream);

                var workbook = new Workbook();
                Worksheet workSheet = workbook.Worksheets.FirstOrDefault();

                var cols = new List<string>() {
                                  "PosmItemCode", "PosmItemName", "PosmClassCode", "PosmClassName", "PosmTypeCode", "PosmTypeName","Link", "UnitType", "CalcType" , "IsActive", "PosmCatalogCode", "PosmCatalogName"
                                };
                for (int colIdx = 0; colIdx < cols.Count; colIdx++)
                {
                    SetCellValue(workSheet.Cells[0, colIdx], LocalizationSource.GetString("PosmItem." +cols[colIdx]), TextAlignmentType.Center, TextAlignmentType.Center, true, 10, true, true);
                    workSheet.Cells.Columns[colIdx].Width = 30;
                    
                }
                var allColumns = new List<string>();
                allColumns.AddRange(cols);
                workSheet.Cells.SetRowHeight(0, 20);

                var items = await (from posmItem in _dbContext.PosmItems
                                   join posmClass in _dbContext.PosmClasses on posmItem.PosmClassId equals posmClass.Id
                                   join posmType in _dbContext.PosmTypes on posmItem.PosmTypeId equals posmType.Id into posmTypeL
                                   from posmType in posmTypeL.DefaultIfEmpty()
                                   join posmCatalog in _dbContext.PosmCatalogs on posmItem.Id equals posmCatalog.PosmItemId
                                   select new
                                   {
                                       posmItem.IsActive,
                                       PosmTypeCode = posmType.Code,
                                       PosmTypeName = posmType.Name,
                                       posmItem.Code,
                                       posmItem.Name,
                                       posmItem.Link,
                                       PosmClassCode = posmClass.Code,
                                       PosmClassName = posmClass.Name,
                                       posmItem.UnitType,
                                       posmItem.CalcType,
                                       PosmCatalogCode = posmCatalog.Code,
                                       PosmCatalogName = posmCatalog.Name,
                                   }).ToListAsync();

                for (int i = 0; i < items.Count; i++)
                {
                    int row = i + 1;
                    workSheet.Cells[row, 0].PutValue(items[i].Code);
                    workSheet.Cells[row, 1].PutValue(items[i].Name);
                    workSheet.Cells[row, 2].PutValue(items[i].PosmClassCode);
                    workSheet.Cells[row, 3].PutValue(items[i].PosmClassName);
                    workSheet.Cells[row, 4].PutValue(items[i].PosmTypeCode);
                    workSheet.Cells[row, 5].PutValue(items[i].PosmTypeName);
                    workSheet.Cells[row, 6].PutValue(items[i].Link);
                    workSheet.Cells[row, 7].PutValue(items[i].UnitType.ToString());
                    workSheet.Cells[row, 8].PutValue(items[i].CalcType.ToString());
                    workSheet.Cells[row, 9].PutValue(items[i].IsActive ? "TRUE": "FALSE");
                    workSheet.Cells[row, 10].PutValue(items[i].PosmCatalogCode);
                    workSheet.Cells[row, 11].PutValue(items[i].PosmCatalogName);
                }

                string fileName = Path.Combine(downloadFolder, Guid.NewGuid().ToString() + ".xlsx");
                workbook.Save(fileName, SaveFormat.Xlsx);
                return fileName;
            }
        }

        private void SetCellValue(Cell c, string lang, TextAlignmentType alignV, TextAlignmentType alignH, bool isBold, int size, bool isTitle, bool isBackground = false)
        {
            c.PutValue(" " + lang);
            var style = c.GetStyle();
            style.Font.IsBold = isBold;
            style.Font.Size = size;
            style.HorizontalAlignment = alignH;
            style.VerticalAlignment = alignV;
            c.SetStyle(style);
        }
    }
}