using Aspose.Cells;
using Cbms.Kms.Application.PosmItems.Commands;
using Cbms.Kms.Application.PosmPrices.Commands;
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
    public class PosmPriceHeaderExportCommandHandler : CommandHandlerBase, IRequestHandler<PosmPriceHeaderExportCommand, string>
    {
        private readonly IConfiguration _configuration;
   
        private AppDbContext _dbContext;

        public PosmPriceHeaderExportCommandHandler(IRequestSupplement supplement, IConfiguration configuration, AppDbContext dbContext) : base(supplement)
        {
           
            _configuration = configuration;
            _dbContext = dbContext;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<string> Handle(PosmPriceHeaderExportCommand request, CancellationToken cancellationToken)
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
                                  "Code", "Name", "FromDate", "ToDate","PosmItemCode", "PosmItemName", "UnitType", "CalcType", "Price"
                                };
                for (int colIdx = 0; colIdx < cols.Count; colIdx++)
                {
                    SetCellValue(workSheet.Cells[0, colIdx], LocalizationSource.GetString("PosmPrice." + cols[colIdx]), TextAlignmentType.Center, TextAlignmentType.Center, true, 10, true, true);
                    workSheet.Cells.Columns[colIdx].Width = 30;
                    
                }
                var allColumns = new List<string>();
                allColumns.AddRange(cols);
                workSheet.Cells.SetRowHeight(0, 20);

                var items = await (from header in _dbContext.PosmPriceeHeaders
                                   join detail in _dbContext.PosmPriceDetails on header.Id equals detail.PosmPriceHeaderId
                                   join item in _dbContext.PosmItems on detail.PosmItemId equals item.Id
                                   orderby header.Code, item.Code
                                   select new
                                   {
                                       header.FromDate,
                                       header.ToDate,
                                       header.Name,
                                       header.Code,
                                       item.UnitType,
                                       item.CalcType,
                                       PosmItemCode = item.Code,
                                       PosmItemName = item.Name,
                                       detail.Price
                                   }).ToListAsync();

                for (int i = 0; i < items.Count; i++)
                {
                    int row = i + 1;
                    workSheet.Cells[row, 0].PutValue(items[i].Code);
                    workSheet.Cells[row, 1].PutValue(items[i].Name);
                    workSheet.Cells[row, 2].PutValue(items[i].FromDate.ToString("MM/dd/yyyy"), false);
                    workSheet.Cells[row, 3].PutValue(items[i].ToDate.ToString("MM/dd/yyyy"), false);
                    workSheet.Cells[row, 4].PutValue(items[i].PosmItemCode);
                    workSheet.Cells[row, 5].PutValue(items[i].PosmItemName);
                    workSheet.Cells[row, 6].PutValue(items[i].UnitType.ToString());
                    workSheet.Cells[row, 7].PutValue(items[i].CalcType.ToString());
                    workSheet.Cells[row, 8].PutValue(items[i].Price);
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