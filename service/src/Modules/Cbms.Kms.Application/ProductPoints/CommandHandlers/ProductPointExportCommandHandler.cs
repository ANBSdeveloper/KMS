using Aspose.Cells;
using Cbms.Kms.Application.Materials.Commands;
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

namespace Cbms.Kms.Application.Materials.CommandHandlers
{
    public class ProductPointExportCommandHandler : CommandHandlerBase, IRequestHandler<ProductPointExportCommand, string>
    {
        private readonly IConfiguration _configuration;
   
        private AppDbContext _dbContext;

        public ProductPointExportCommandHandler(IRequestSupplement supplement, IConfiguration configuration, AppDbContext dbContext) : base(supplement)
        {
           
            _configuration = configuration;
            _dbContext = dbContext;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<string> Handle(ProductPointExportCommand request, CancellationToken cancellationToken)
        {
            var downloadFolder = Path.GetTempPath();

            string license = _configuration["ImportExport:License"];
            using (var asposeStream = new MemoryStream(Convert.FromBase64String(license)))
            {
                asposeStream.Seek(0, SeekOrigin.Begin);
                new License().SetLicense(asposeStream);

                var workbook = new Workbook();
                Worksheet workSheet = workbook.Worksheets.FirstOrDefault();

                var cols = new List<string>() {"ProductCode", "ProductName", "Points", "FromDate", "ToDate"};
                for (int colIdx = 0; colIdx < cols.Count; colIdx++)
                {
                    if(cols[colIdx] == "FromDate" || cols[colIdx] == "ToDate")
                    {
                        SetCellValue(workSheet.Cells[0, colIdx], LocalizationSource.GetString("ProductPoint." + cols[colIdx]) + "\n\r (DD\\MM\\YYYY)", TextAlignmentType.Center, TextAlignmentType.Center, true, 10, true, true);
                    }
                    else
                    {
                        SetCellValue(workSheet.Cells[0, colIdx], LocalizationSource.GetString("ProductPoint." + cols[colIdx]), TextAlignmentType.Center, TextAlignmentType.Center, true, 10, true, true);
                    }                    
                    workSheet.Cells.Columns[colIdx].Width = 30;
                    
                }
                var allColumns = new List<string>();
                allColumns.AddRange(cols);
                workSheet.Cells.SetRowHeight(0, 20);

                var items = await (from productPoint in _dbContext.ProductPoints
                                   join product in _dbContext.Products on productPoint.ProductId equals product.Id
                                   join brand in _dbContext.Brands on product.BrandId equals brand.Id
                                   join productClass in _dbContext.ProductClasses on product.ProductClassId equals productClass.Id
                                   join subProductClass in _dbContext.SubProductClasses on product.SubProductClassId equals subProductClass.Id

                                   select new
                                   {
                                       ProductCode = product.Code,
                                       ProductName = product.Name,
                                       Points = productPoint.Points,
                                       FromDate = productPoint.FromDate,
                                       ToDate = productPoint.ToDate,
                                       IsActive = productPoint.IsActive
    
                                   }).ToListAsync();

                for (int i = 0; i < items.Count; i++)
                {
                    int row = i + 1;
                    workSheet.Cells[row, 0].PutValue(items[i].ProductCode);
                    workSheet.Cells[row, 1].PutValue(items[i].ProductName);
                    workSheet.Cells[row, 2].PutValue(items[i].Points);
                    workSheet.Cells[row, 3].PutValue(items[i].FromDate.ToShortDateString());
                    workSheet.Cells[row, 4].PutValue(items[i].ToDate.ToShortDateString());
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