using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDB.Console.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace CosmosDB.Console
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await CreateCategory();
            var categories = await GetCategories();
            var productsByCategory = await GetProductsByCategory();
            var productsByTag = await GetProductsByTag();
            var customerData = await GetCustomerData("38b31de6-4b90-433c-be68-563b57fd41b8");
        }

        private static async Task CreateCategory()
        {
            var database = GetDatabase();
            Container container = database.GetContainer("productmeta");

            var category = new CategoryModel() { id = Guid.NewGuid(), name = "Food", type = "category" };

            await container.CreateItemAsync(category);
        }

        private static async Task<List<CategoryModel>> GetCategories()
        {
            var database = GetDatabase();
            Container container = database.GetContainer("productmeta");

            List<CategoryModel> result = new List<CategoryModel>();
            using (var feedIterator = container.GetItemQueryIterator<CategoryModel>($"SELECT * FROM productmeta WHERE productmeta.type = 'category'"))
            {
                while (feedIterator.HasMoreResults)
                {
                    var response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private static async Task<List<ProductModel>> GetProductsByCategory()
        {
            var database = GetDatabase();
            Container container = database.GetContainer("product");

            List<ProductModel> result = new List<ProductModel>();
            using (var feedIterator = container.GetItemQueryIterator<ProductModel>($"SELECT * FROM product WHERE product.categoryId = '794c4554-a64d-45c1-8403-7f3d3c9c8e75'"))
            {
                while (feedIterator.HasMoreResults)
                {
                    var response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private static async Task<List<ProductModel>> GetProductsByTag()
        {
            var database = GetDatabase();
            Container container = database.GetContainer("product");

            List<ProductModel> result = new List<ProductModel>();
            using (var feedIterator = container.GetItemQueryIterator<ProductModel>("SELECT * FROM product WHERE ARRAY_CONTAINS(product.tagIds,  { 'id': '835ae795-0482-4b65-bbdb-24ac0bff8d7c' }, true)"))
            {
                while (feedIterator.HasMoreResults)
                {
                    var response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private static async Task<CustomerOrders> GetCustomerData(string customerId)
        {
            var database = GetDatabase();
            Container container = database.GetContainer("customer");

            CustomerOrders result = new CustomerOrders();

            using (var feedIterator = container.GetItemQueryIterator<Document>($"SELECT * FROM customer WHERE customer.customerId = '{customerId}'"))
            {
                while (feedIterator.HasMoreResults)
                {
                    var response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        var type = item.GetPropertyValue<string>("type");
                        switch (type)
                        {
                            case "customer":
                                result.Customer = JsonConvert.DeserializeObject<CustomerModel>(item.ToString());
                                break;
                            case "order":
                                result.Orders.Add(JsonConvert.DeserializeObject<OrderModel>(item.ToString()));
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private static Microsoft.Azure.Cosmos.Database GetDatabase()
        {
            CosmosClient client = new CosmosClient("https://cosmosdb-erwin.documents.azure.com:443/", "nDGOYFbQHAFozwlabVFQKqOmMBHonGPTAjax2WE1gvcaiVr6DZsc2NJZidbxNbI8dGAGHmnONRQxoYxfc4hPlA==");
            Microsoft.Azure.Cosmos.Database database = client.GetDatabase("OrdersDB");

            return database;
        }
    }
}
