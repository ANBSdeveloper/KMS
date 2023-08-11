using Newtonsoft.Json;

namespace Cbms.Kms.Application.Integration.Dto
{
    public class RewardAppResultDto<T>
    {
        [JsonProperty("meta")]
        public MetaDto Meta { get; set; }
        [JsonProperty("response")]
        public T Response { get; set; }


        public class MetaDto
        {
            [JsonProperty("status")]
            public int Status { get; set; }
            [JsonProperty("msg")]
            public string Message { get; set; }
            [JsonProperty("error")]
            public string Error { get; set; }
        }
    }

    public class SpoonCodeResultDto
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("spoonCode")]
        public string Code { get; set; }
    }

}
