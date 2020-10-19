using System.Collections.Generic;

namespace CosmosDB.Console.Models
{
    public class ProductModel : BaseModel
    {
        public string categoryId { get; set; }
        public string categoryname { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<TagModel> tagIds { get; set; }
    }
}
