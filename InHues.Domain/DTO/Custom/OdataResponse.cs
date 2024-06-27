using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InHues.Domain.DTO.Custom
{
    public class OdataResponse<T>
    {

        [JsonPropertyName("@odata.count")]
        public int Count { get; set; }

        [JsonPropertyName("value")]
        public List<T> Values { get; set; } = new();
    }
    public class OdataSingleResponse<T>
    {

        [JsonPropertyName("value")]
        public T? Value { get; set; }
    }
}
