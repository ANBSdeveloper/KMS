using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Materials.Commands
{
    public class MaterialDeleteCommand : DeleteEntityCommand
    {
        public MaterialDeleteCommand(int id) : base(id)
        {
        }
    }
}
