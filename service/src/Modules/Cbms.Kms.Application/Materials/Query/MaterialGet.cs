using Cbms.Kms.Application.Materials.Dto;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Materials.Query
{
    public class MaterialGet : EntityQuery<MaterialDto>
    {
        public MaterialGet(int id) : base(id)
        {
        }
    }
}
