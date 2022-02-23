using BakeryService.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static BakeryService.Model.Product;

namespace BakeryService.DAL
{
    public class BakeryDatabase
    {
        
        private string _connectionString;
        public BakeryDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Product> GetProducts() 
        {
            List<Product> items = new List<Product>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Product";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = Product.MapFromReader(reader);
                            //new Product();
                            //product.ProductType = (ProductTypes)reader[Product.PRODUCT_TYPE];
                            //product.Id = (int)reader[Product.PRODUCT_ID];
                            //product.ProductName = (string)reader[PRODUCT_NAME];
                            //product.ProductDescription = (string)reader[PRODUCT_DESCRIPTION];
                            //product.Price = (double)reader[PRICE];
                            //product.Icon = reader[ICON]?.ToString();
                            items.Add(product);
                        }
                    }
                }
            }

            return items;
        }

        internal IEnumerable<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM Orders";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = Order.MapFromReader(reader);
                            order.OrderProducts = GetOrderProducts(order.Id);
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        internal Order GetOrder(string orderId)
        {
            Order order = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string idParam = "@Id";
                string query = $"SELECT * FROM Orders WHERE Id = {idParam}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(idParam, orderId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order = Order.MapFromReader(reader);
                            order.OrderProducts = GetOrderProducts(order.Id);
                        }
                    }
                }
            }
            return order;
        }

        internal void DeleteOrder(string id)
        {
            DeleteOrderProducts(id);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string idParam = "@Id";
                string query = $"DELETE FROM {Order.TABLE_NAME} WHERE {Order.ID} = {idParam}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(idParam, id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteOrderProducts(string orderId) 
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string idParam = "@Id";
                string query = $"DELETE FROM {OrderProduct.TABLE_NAME} WHERE {OrderProduct.ORDER_ID} = {idParam}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(idParam, orderId);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal void SaveUser(LoginUser user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string emailParam = "@Email";
                string passwordParam = "@Password";
                string query = $"INSERT INTO {LoginUser.TABLENAME} VALUES({emailParam}, {passwordParam})";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(emailParam, user.Email);
                    command.Parameters.AddWithValue(passwordParam, user.Password);
                    command.ExecuteNonQuery();
                }
            }
            
        }

        internal Product GetProduct(int productId)
        {
            Product product = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string idParam = "@IdParam";
                string query = $"SELECT * FROM {Product.TABLE_NAME} WHERE {Product.PRODUCT_ID} = {idParam}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(idParam, productId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                            product = Product.MapFromReader(reader);
                    }
                }
            }

            return product;
        }

        internal void SaveProduct(Product product)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //string productIdParam = "@IdParam";
                string productNameParam = "@ProductNameParam";
                string priceParam = "@PriceParam";
                string iconParam = "@IconParam";
                string typeParam = "@TypeParam";
                string descriptionParam = "@DescriptionParam";
                string insertProductQuery = $"INSERT INTO {Product.TABLE_NAME} VALUES( {productNameParam}, {priceParam}, {iconParam}, {typeParam}, {descriptionParam})";

                using (SqlCommand command = new SqlCommand(insertProductQuery, connection))
                {
                    command.Parameters.AddWithValue(productNameParam, product.ProductName);
                    command.Parameters.AddWithValue(priceParam, product.Price);
                    command.Parameters.AddWithValue(iconParam, product.Icon);
                    command.Parameters.AddWithValue(typeParam, product.ProductType);
                    command.Parameters.AddWithValue(descriptionParam, product.ProductDescription);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal LoginUser GetUser(string userId)
        {
            LoginUser user = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string userIdParam = "@UserId";
                string query = $"SELECT * FROM {LoginUser.TABLENAME} WHERE {LoginUser.EMAIL} = {userIdParam}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(userIdParam, userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = LoginUser.MapReader(reader);
                            return user;
                        }
                    }
                }
            }

            return user;
        }

        internal void SaveOrder(Order order)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string orderIdParam = "@OrderId";
                string customerIdParam = "@CustomerIdParam";
                string orderStatusParam = "@OrderStatusParam";
                string orderTotalParam = "@OrderTotalParam";
                string orderDateParam = "@OrderDateParam";
                string orderPickupDateParam = "@OrderPickupDateParam";

                string insertOrderQuery = $"INSERT INTO Orders VALUES({orderIdParam},{customerIdParam},{orderStatusParam},{orderTotalParam}, {orderDateParam},{orderPickupDateParam})";

                string productIdParam = "@ProductId";
                string AmountParam = "@Amount";
                string insertOrderProductsQuery = $"INSERT INTO OrderProduct VALUES({orderIdParam}, {productIdParam}, {AmountParam})";


                using (SqlCommand command = new SqlCommand(insertOrderQuery, connection))
                {
                    command.Parameters.AddWithValue(orderIdParam, order.Id);
                    command.Parameters.AddWithValue(customerIdParam, order.CustomerId);
                    command.Parameters.AddWithValue(orderStatusParam, 1);
                    command.Parameters.AddWithValue(orderTotalParam, order.OrderTotal);
                    command.Parameters.AddWithValue(orderDateParam, order.OrderDate == null ? DateTime.Now : order.OrderDate);
                    command.Parameters.AddWithValue(orderPickupDateParam, order.PickupDate == null ? DateTime.Now : order.PickupDate);
                    command.ExecuteNonQuery();

                    using (SqlCommand insertOrderProductsCommand = new SqlCommand(insertOrderProductsQuery, connection))
                    {
                        for (int orderProductIndex = 0; orderProductIndex < order.OrderProducts.Count; orderProductIndex++)
                        {
                            OrderProduct product = order.OrderProducts[orderProductIndex];
                            if (product.Amount < 1)
                            {
                                continue;
                            }
                            insertOrderProductsCommand.Parameters.AddWithValue(orderIdParam, new Guid(order.Id));
                            insertOrderProductsCommand.Parameters.AddWithValue(productIdParam, product.ProductId);
                            insertOrderProductsCommand.Parameters.AddWithValue(AmountParam, product.Amount);
                            insertOrderProductsCommand.ExecuteNonQuery();

                            insertOrderProductsCommand.Parameters.Clear();
                        }
                    }
                  
                }
            }
        }

        public List<Order> GetOrders(string userId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string customerIdParam = "@CustomerId";
                string query = $"SELECT * FROM Orders WHERE CustomerId = {customerIdParam}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(customerIdParam, userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = Order.MapFromReader(reader);
                            order.OrderProducts = GetOrderProducts(order.Id);
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        private List<OrderProduct> GetOrderProducts(string orderId) 
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string orderIdParam = "@OrderId";
                string query = $"SELECT * FROM OrderProduct OI JOIN Product P on OI.ProductId = P.Id WHERE OrderId = {orderIdParam}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(orderIdParam, orderId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = Product.MapFromReader(reader);
                            OrderProduct orderProduct = OrderProduct.MapFromReader(reader);
                            orderProduct.Product = product;
                            orderProducts.Add(orderProduct);
                        }
                    }
                }
            }

            return orderProducts;
        }



    }
}
