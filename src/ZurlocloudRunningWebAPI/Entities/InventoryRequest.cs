using System.Text.Json.Serialization;

namespace ZurlocloudRunningWebAPI.Entities
{
    /// <summary>
    /// Represents a maintenance request.
    /// </summary>
    public class InventoryRequest
    {
        [JsonPropertyName("id")]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("store_id")]
        public int store_id { get; set; }

        [JsonPropertyName("product_id")]
        public int product_id { get; set; }

        [JsonPropertyName("source")]
        public string source { get; set; }

        [JsonPropertyName("date")]
        public DateTime date { get; set; }

        [JsonPropertyName("details")]
        public string details { get; set; }

        [JsonPropertyName("location")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? location { get; set; }
    }
}
