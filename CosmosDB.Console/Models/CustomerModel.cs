using System.Collections.Generic;

namespace CosmosDB.Console.Models
{
    public class CustomerModel : BaseModel
    {
        public string customerId { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public List<AddressModel> addresses { get; set; }
    }
}
