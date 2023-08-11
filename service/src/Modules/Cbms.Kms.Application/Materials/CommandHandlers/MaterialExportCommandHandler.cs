using Aspose.Cells;
using Cbms.Kms.Application.Materials.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Materials;
using Cbms.Kms.Infrastructure;
using Cbms.Localization;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Materials.CommandHandlers
{
    public class MaterialExportCommandHandler : CommandHandlerBase, IRequestHandler<MaterialExportCommand, string>
    {
        private readonly IConfiguration _configuration;
   
        private AppDbContext _dbContext;

        public MaterialExportCommandHandler(IRequestSupplement supplement, IConfiguration configuration, AppDbContext dbContext) : base(supplement)
        {
           
            _configuration = configuration;
            _dbContext = dbContext;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<string> Handle(MaterialExportCommand request, CancellationToken cancellationToken)
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
                                  "MaterialCode", "MaterialName", "MaterialTypeCode", "MaterialTypeName","IsDesign", "Value","Status"
                                };
                for (int colIdx = 0; colIdx < cols.Count; colIdx++)
                {
                    SetCellValue(workSheet.Cells[0, colIdx], LocalizationSource.GetString("Material." +cols[colIdx]), TextAlignmentType.Center, TextAlignmentType.Center, true, 10, true, true);
                    workSheet.Cells.Columns[colIdx].Width = 30;
                    
                }
                var allColumns = new List<string>();
                allColumns.AddRange(cols);
                workSheet.Cells.SetRowHeight(0, 20);

                var items = await (from material in _dbContext.Materials
                                   join materialType in _dbContext.MaterialTypes on material.MaterialTypeId equals materialType.Id

                                   select new
                                   {
                                       CreationTime = material.CreationTime,
                                       CreatorUserId = material.CreatorUserId,
                                       Name = material.Name,
                                       Id = material.Id,
                                       IsActive = material.IsActive,
                                       LastModificationTime = material.LastModificationTime,
                                       LastModifierUserId = material.LastModifierUserId,
                                       MaterialTypeCode = materialType.Code,
                                       MaterialTypeName = materialType.Name,
                                       Code = material.Code,
                                       Description = material.Description,
                                       IsDesign = material.IsDesign,
                                       Value = material.Value
                                   }).ToListAsync();

                for (int i = 0; i < items.Count; i++)
                {
                    int row = i + 1;
                    workSheet.Cells[row, 0].PutValue(items[i].Code);
                    workSheet.Cells[row, 1].PutValue(items[i].Name);
                    workSheet.Cells[row, 2].PutValue(items[i].MaterialTypeCode);
                    workSheet.Cells[row, 3].PutValue(items[i].MaterialTypeName);
                    workSheet.Cells[row, 4].PutValue(items[i].IsDesign);
                    workSheet.Cells[row, 5].PutValue(items[i].Value);
                    workSheet.Cells[row, 6].PutValue(items[i].IsActive);
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