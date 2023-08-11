using Cbms.Kms.Application.Materials.Commands;
using Cbms.Kms.Domain.Materials;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Materials.CommandHandlers
{
    public class MaterialDeleteCommandHandler : DeleteEntityCommandHandler<MaterialDeleteCommand, Material>
    {
        public MaterialDeleteCommandHandler(IRequestSupplement supplement) : base(supplement)
        {
        }
    }
}
