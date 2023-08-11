using Aspose.Cells;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Materials.Commands;
using Cbms.Kms.Application.Materials.Dto;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Materials;
using Cbms.Kms.Domain.Materials.Actions;
using Cbms.Kms.Domain.MaterialTypes;
using Cbms.Kms.Domain.MaterialTypes.Actions;
using Cbms.Mediator;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Materials.CommandHandlers
{
    public class MaterialImportCommandHandler : CommandHandlerBase, IRequestHandler<MaterialImportCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Material, int> _materialRepository;
        private readonly IRepository<MaterialType, int> _materialTypeRepository;

        public MaterialImportCommandHandler(IRequestSupplement supplement, IConfiguration configuration, IRepository<Material, int> materialRepository, IRepository<MaterialType, int> materialTypeRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _configuration = configuration;
            _materialRepository = materialRepository;
            _materialTypeRepository = materialTypeRepository;
        }

        public async Task<Unit> Handle(MaterialImportCommand request, CancellationToken cancellationToken)
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

                    List<MaterialDto> headers = new List<MaterialDto>();

                    Worksheet workSheet = workbook.Worksheets[0];

                    StringBuilder stringBuilder = new StringBuilder();

                    Regex regex = new Regex("^[a-zA-Z0-9&_\\-#]*$");

                    var colMapper = new Dictionary<string, int>() {
                        {"MaterialCode", 0 },
                        {"MaterialName", 1 },
                        {"MaterialTypeCode", 2 },
                        {"MaterialTypeName", 3 },
                        {"IsDesign", 4 },
                        {"Value", 5 },
                        {"Status", 6 }
                    };

                    for (int line = 2; line <= workSheet.Cells.MaxDataRow + 1; line++)
                    {
                        int index = line - 1;
                        string code = workSheet.Cells[index, colMapper["MaterialCode"]].StringValue.ToUpper();
                        if (!string.IsNullOrEmpty(code))
                        {
                            code = code.Trim();

                            if (code.Trim().Length > 30)
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportLengthInvalid", line.ToString(), LocalizationSource.GetString("Material.MaterialCode"), "30").Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }

                            if (!regex.IsMatch(code))
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportSpecialCharacter", line.ToString(), LocalizationSource.GetString("Material.MaterialCode")).Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }

                            string name = workSheet.Cells[index, colMapper["MaterialName"]].StringValue;
                            if (string.IsNullOrEmpty(name))
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportEmpty", line.ToString(), LocalizationSource.GetString("Material.MaterialName")).Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }
                            name = name.Trim();
                            if (name.Length > 200)
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportLengthInvalid", line.ToString(), LocalizationSource.GetString("Material.MaterialName"), "200").Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }

                            int materialTypeId = 0;
                            string materialTypeCode = workSheet.Cells[index, colMapper["MaterialTypeCode"]].StringValue;
                            string materialTypeName = workSheet.Cells[index, colMapper["MaterialTypeName"]].StringValue;
                            if (string.IsNullOrEmpty(materialTypeCode))
                            {
                                var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportEmpty", line.ToString(), LocalizationSource.GetString("Material.MaterialTypeCode")).Build();
                                stringBuilder.Append($"" + message.Message + "<br>");
                                continue;
                            }
                            else
                            {
                                materialTypeCode = materialTypeCode.Trim();

                                if (materialTypeCode.Trim().Length > 30)
                                {
                                    var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportLengthInvalid", line.ToString(), LocalizationSource.GetString("Material.MaterialTypeCode"), "30").Build();
                                    stringBuilder.Append($"" + message.Message + "<br>");
                                    continue;
                                }

                                if (!regex.IsMatch(materialTypeCode))
                                {
                                    var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportSpecialCharacter", line.ToString(), LocalizationSource.GetString("Material.MaterialTypeCode"), "30").Build();
                                    stringBuilder.Append($"" + message.Message + "<br>");
                                    continue;
                                }

                                var objMaterialType = await _materialTypeRepository.FirstOrDefaultAsync(p => p.Code.ToUpper() == materialTypeCode.ToUpper());
                                if (objMaterialType == null)
                                {
                                    if (string.IsNullOrEmpty(materialTypeName))
                                    {
                                        var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportEmpty", line.ToString(), LocalizationSource.GetString("Material.MaterialTypeName")).Build();
                                        stringBuilder.Append($"" + message.Message + "<br>");
                                        continue;
                                    }

                                    materialTypeCode = materialTypeCode.Trim();

                                    if (materialTypeCode.Trim().Length > 30)
                                    {
                                        var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportLengthInvalid", line.ToString(), LocalizationSource.GetString("Material.MaterialTypeCode"), "30").Build();
                                        stringBuilder.Append($"" + message.Message + "<br>");
                                        continue;
                                    }

                                    materialTypeName = materialTypeName.Trim();

                                    if (materialTypeName.Trim().Length > 200)
                                    {
                                        var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportLengthInvalid", line.ToString(), LocalizationSource.GetString("Material.MaterialTypeName"), "200").Build();
                                        stringBuilder.Append($"" + message.Message + "<br>");
                                        continue;
                                    }

                                    objMaterialType = MaterialType.Create();
                                    await _materialTypeRepository.InsertAsync(objMaterialType);

                                    await objMaterialType.ApplyActionAsync(new MaterialTypeUpsertAction(
                                        materialTypeCode.ToUpper(),
                                        materialTypeName,
                                        true
                                    ));
                                    //await _materialTypeRepository.UnitOfWork.CommitAsync();
                                    //objMaterialType = await _materialTypeRepository.FirstOrDefaultAsync(p => p.Code.ToUpper() == materialTypeCode.ToUpper());
                                    materialTypeId = objMaterialType.Id;
                                }
                                else
                                {
                                    materialTypeId = objMaterialType.Id;
                                }
                            }
                            bool isDesign = false;
                            string design = workSheet.Cells[index, colMapper["IsDesign"]].StringValue;
                            if (!string.IsNullOrEmpty(design))
                            {
                                if (design.ToUpper() == "TRUE" || design.ToUpper() == "FALSE")
                                {
                                    if (design.ToUpper() == "TRUE")
                                    {
                                        isDesign = true;
                                    }
                                }
                                else
                                {
                                    var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportFormatBool", line.ToString(), LocalizationSource.GetString("Material.IsDesign")).Build();
                                    stringBuilder.Append($"" + message.Message + "<br>");
                                    continue;
                                }
                            }
                            decimal value = 0;
                            string strValue = workSheet.Cells[index, colMapper["Value"]].StringValue;
                            if (!string.IsNullOrEmpty(strValue))
                            {
                                try
                                {
                                    value = decimal.Parse(strValue);
                                    if(value < 0)
                                    {
                                        var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportNumber", line.ToString(), LocalizationSource.GetString("Material.Value")).Build();
                                        stringBuilder.Append($"" + message.Message + "<br>");
                                        continue;
                                    }
                                }
                                catch
                                {
                                    
                                    var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportFormatNumber", line.ToString(), LocalizationSource.GetString("Material.Value")).Build();
                                    stringBuilder.Append($"" + message.Message + "<br>");
                                    continue;
                                }
                            }
                            bool isActive = false;
                            string status = workSheet.Cells[index, colMapper["Status"]].StringValue;
                            if (!string.IsNullOrEmpty(status))
                            {
                                if (status.ToUpper() == "TRUE" || status.ToUpper() == "FALSE")
                                {
                                    if (status.ToUpper() == "TRUE")
                                    {
                                        isActive = true;
                                    }
                                }
                                else
                                {
                                    var message = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Material.ImportFormatBool", line.ToString(), LocalizationSource.GetString("Material.Status") ).Build();
                                    stringBuilder.Append($"" + message.Message + "<br>");
                                    continue;
                                }
                            }

                            var objMaterial = await _materialRepository.FirstOrDefaultAsync(p => p.Code.ToUpper() == code.ToUpper());

                            Material entity = null;
                            if (objMaterial != null)
                            {
                                entity = await _materialRepository.GetAsync(objMaterial.Id);
                            }

                            if (entity == null)
                            {
                                entity = Material.Create();
                                await _materialRepository.InsertAsync(entity);
                            }

                            await entity.ApplyActionAsync(new UpsertMaterialAction(
                                code,
                                name,
                                materialTypeId,
                                "",
                                value,
                                isActive,
                                isDesign
                            ));
                        }
                    }

                    if (stringBuilder.Length > 0)
                    {
                        throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Import.Error", stringBuilder.ToString()).Build();
                    }
                    await _materialRepository.UnitOfWork.CommitAsync();
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