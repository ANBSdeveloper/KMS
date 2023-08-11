using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Consumers.Dto
{
    public class SalesForceFindResultDto
    { 
        [JsonProperty("records")]
        public List<SalesForceFindResultRecordDto> Records { get; set; }
	}

    public class SalesForceFindResultRecordDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
