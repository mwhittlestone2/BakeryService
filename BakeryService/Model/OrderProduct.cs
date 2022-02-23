using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryService.Model
{
    public class OrderProduct
    {
        public static string TABLE_NAME = "OrderProduct";
        public static string ORDER_ID = "OrderId";
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        
        [JsonIgnore]
        public Product Product { get; set; }
        public int Amount { get; set; }

        internal static OrderProduct MapFromReader(SqlDataReader reader)
        {
            OrderProduct orderProduct = new OrderProduct();

            orderProduct.ProductId = (int)reader[nameof(ProductId)];
            orderProduct.OrderId = ((Guid)reader[nameof(OrderId)]).ToString();
            orderProduct.Amount = (int)reader[nameof(Amount)];
            return orderProduct;
        }
    }
}
