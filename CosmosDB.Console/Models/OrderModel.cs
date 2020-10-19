using System.Collections.Generic;

namespace CosmosDB.Console.Models
{
    public class OrderModel : BaseModel
    {
        public string customerId { get; set; }
        public string type { get; set; }
        public string orderDate { get; set; }
        public List<OrderLineModel> orderLines { get; set; }
    }
}
