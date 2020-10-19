using Newtonsoft.Json;

namespace CosmosDB.Console.Models
{
    public class AddressModel
    {
        [JsonProperty("address:")]
        public string Address { get; set; }
        public string city { get; set; }
    }
}
