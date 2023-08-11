using Aspose.Cells;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Materials.Commands;
using Cbms.Kms.Application.Materials.Dto;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.ProductPoints;
using Cbms.Kms.Domain.ProductPoints.Actions;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Materials.CommandHandlers
{
    public class ProductPointImportCommandHandler : CommandHandlerBase, IRequestHandler<ProductPointImportCommand>
    {
        private readonly IAppLogger _appLogger;
        private readonly IConfiguration _configuration;
        private readonly IRepository<ProductPoint, int> _productPointRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly AppDbContext _dbContext;
        public ProductPointImportCommandHandler(
            IRequestSupplement supplement,
            IConfiguration configuration,
            IRepository<ProductPoint, int> productPointRepository,
            IRepository<Product, int> productRepository,
            IAppLogger appLogger,
            AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _configuration = configuration;
            _productPointRepository = productPointRepository;
            _productRepository = productRepository;
            _appLogger = appLogger;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(ProductPointImportCommand request, CancellationToken cancellationToken)
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

                using (var asposeStream = new MemoryStream(Convert.FromBase64String(license)))
                {
                    asposeStream.Seek(0, SeekOrigin.Begin);
                    new License().SetLicense(asposeStream);
                    var workbook = new Workbook(path);

                    await _appLogger.LogInfoAsync("IMPORT_POINT", new { User = Session.UserName, ImportDate = DateTime.Now });

                    List<MaterialDto> headers = new List<MaterialDto>();

                    Worksheet workSheet = workbook.Worksheets[0];

                    StringBuilder stringBuilder = new StringBuilder();

                    var colMapper = new Dictionary<string, int>() {
                        {"ProductCode", 0 },
                        {"ProductName", 1 },
                        {"Points", 2 },
                        {"FromDate", 3 },
                        {"ToDate", 4 },
                    };

                    var productPoints = new List<ProductPointImportData>();

                    for (int line = 2; line <= workSheet.Cells.MaxDataRow + 1; line++)
                    {
                        int index = line - 1;
                        string code = workSheet.Cells[index, colMapper["ProductCode"]].StringValue.ToUpper();
                        if (!string.IsNullOrEmpty(code))
                        {
                            int productId = 0;
                            var objProduct = await _productRepository.FirstOrDefaultAsync(p => p.Code.ToUpper() == code.ToUpper());
                            if (objProduct == null)
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportExists", line.ToString(), LocalizationSource.GetString("Material.ProductCode")).Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }
                            else
                            {
                                productId = objProduct.Id;
                            }

                            decimal point = 0;
                            string strPoint = workSheet.Cells[index, colMapper["Points"]].StringValue;
                            if (!string.IsNullOrEmpty(strPoint))
                            {
                                try
                                {
                                    point = decimal.Parse(strPoint);
                                    if (point < 0)
                                    {
                                        var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportNumber", line.ToString(), LocalizationSource.GetString("Material.Points")).Build();
                                        stringBuilder.Append($"" + message.Message + "<br>");
                                        continue;
                                    }
                                }
                                catch
                                {
                                    var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportFormatNumber", line.ToString(), LocalizationSource.GetString("Material.Points")).Build();
                                    stringBuilder.Append($"" + message.Message + "<br>");
                                    continue;
                                }
                            }
                            DateTime fromDate = DateTime.Now;
                            string strFromDate = workSheet.Cells[index, colMapper["FromDate"]].StringValue;
                            if (string.IsNullOrEmpty(strFromDate))
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportEmpty", line.ToString(), LocalizationSource.GetString("Material.FromDate")).Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    fromDate = DateTime.ParseExact(strFromDate, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportFormatDate", line.ToString(), LocalizationSource.GetString("Material.FromDate")).Build();
                                    stringBuilder.Append($"" + message.Message + "<br>");
                                    continue;
                                }
                            }

                            DateTime toDate = DateTime.Now;
                            string strToDate = workSheet.Cells[index, colMapper["ToDate"]].StringValue;
                            if (string.IsNullOrEmpty(strToDate))
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportEmpty", line.ToString(), LocalizationSource.GetString("Material.ToDate")).Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    toDate = DateTime.ParseExact(strToDate, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportFormatDate", line.ToString(), LocalizationSource.GetString("Material.ToDate")).Build();
                                    stringBuilder.Append($"" + message.Message + "<br>");
                                    continue;
                                }
                            }
                            if (toDate < fromDate)
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportErrorFromDateToDate", line.ToString(), LocalizationSource.GetString("Material.ToDate"), LocalizationSource.GetString("Material.FromDate")).Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }

                            productPoints.Add(new ProductPointImportData()
                            {
                                FromDate = fromDate,
                                ToDate = toDate,
                                Line = line,
                                Points = point,
                                ProductId = productId,
                                ProductCode = objProduct.Code
                            });
                        }
                    }

                    if (stringBuilder.Length > 0)
                    {
                        throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportError", stringBuilder.ToString()).Build();
                    }

                    if (productPoints.Count == 0)
                    {
                        throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportNoData").Build();
                    }

                    foreach (var productPoint in productPoints)
                    {
                        var specItems = productPoints.Where(p => p.ProductId == productPoint.ProductId).ToList();
                        foreach (var item in specItems)
                        {
                            var duplicateProduct = specItems.FirstOrDefault(p =>
                                    p.Line != item.Line &&
                                    (
                                        (p.FromDate <= item.FromDate && p.ToDate >= item.FromDate) ||
                                        (p.FromDate <= item.ToDate && p.ToDate >= item.ToDate)
                                    ));
                                
                            if (duplicateProduct != null)
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode(
                                    "ProductPoint.ImportRangeDateConflict", 
                                    item.Line.ToString(), 
                                    item.ProductCode.ToString(),
                                    item.FromDate.ToShortDateString(),
                                    item.ToDate.ToShortDateString(),
                                    duplicateProduct.Line.ToString()
                                ).Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }
                        }
                    }

                    if (stringBuilder.Length > 0)
                    {
                        throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("ProductPoint.ImportError", stringBuilder.ToString()).Build();
                    }

                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                            INSERT INTO [dbo].[ProductPointHistories]
                            ( 
	                            Id
	                            ,[CreationTime]
	                            ,[CreatorUserId]
	                            ,[LastModificationTime]
	                            ,[LastModifierUserId]
	                            ,[ProductId]
	                            ,[Points]
	                            ,[FromDate]
	                            ,[ToDate]
	                            ,[IsActive]
                            )
                            SELECT Id
	                            ,[CreationTime]
	                            ,[CreatorUserId]
	                            ,[LastModificationTime]
	                            ,[LastModifierUserId]
	                            ,[ProductId]
	                            ,[Points]
	                            ,[FromDate]
	                            ,[ToDate]
	                            ,[IsActive] FROM ProductPoints
                            DELETE FROM ProductPoints ");

                    foreach (var productPoint in productPoints)
                    {
                        ProductPoint entity = ProductPoint.Create();
                        await _productPointRepository.InsertAsync(entity);

                        await entity.ApplyActionAsync(new ProductPointUpsertAction(
                            IocResolver,
                            LocalizationSource,
                            productPoint.ProductId,
                            productPoint.Points,
                            productPoint.FromDate,
                            productPoint.ToDate,
                            true,
                            true
                        ));
                    }

                    await _productPointRepository.UnitOfWork.CommitAsync();
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

    internal class ProductPointImportData
    {
        public DateTime FromDate { get; set; }
        public int Line { get; set; }
        public decimal Points { get; set; }
        public int ProductId { get; set; }
        public DateTime ToDate { get; set; }
        public string ProductCode { get; set; }
    }
}