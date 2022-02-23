using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryService.Model
{
    public class Order
    {
        public static readonly string ORDER_STATUS = "OrderStatus";

        public static readonly string TABLE_NAME = "Orders";
        public static readonly string ID = "Id";

        public enum Status
        {
            Pending, Accepted, Completed
        }


        public string Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PickupDate { get; set; }
        public Status OrderStatus { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public double OrderTotal { get; set; }

        internal static Order MapFromReader(SqlDataReader reader)
        {
            Order returnOrder = new Order();
            returnOrder.CustomerId = (string)reader[nameof(CustomerId)];
            returnOrder.Id = ((Guid)reader[nameof(Id)]).ToString();
            //var orderDate = reader[nameof(OrderDate)];
            returnOrder.OrderStatus = (Status)(int)reader[ORDER_STATUS];
            returnOrder.OrderDate = (DateTime)reader[nameof(OrderDate)];
            returnOrder.PickupDate = (DateTime)reader[nameof(PickupDate)];
            returnOrder.OrderTotal = (double)reader["OrderTotal"];// order.OrderItems.Sum(o => o.Item.Price *= o.Amount);
            return returnOrder;
        }
    }
}
