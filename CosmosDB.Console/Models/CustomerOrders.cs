using System.Collections.Generic;

namespace CosmosDB.Console.Models
{
    public class CustomerOrders
    {
        public CustomerOrders()
        {
            Customer = new CustomerModel();

            Orders = new List<OrderModel>();
        }

        public CustomerModel Customer { get; set; }

        public List<OrderModel> Orders { get; set; }
    }
}
