using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Integration.Dto
{
    public class AreaDto
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string ZoneId { get; private set; }
        public int SalesOrgId { get; private set; }
    }
}
