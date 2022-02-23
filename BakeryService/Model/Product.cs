using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryService.Model
{
    public class Product
    {
        public const string PRODUCT_TYPE = "ProductType";
        public const string PRODUCT_ID = "Id";
        public const string TABLE_NAME = "Product";
        public const string PRODUCT_DESCRIPTION = "ProductDescription";
        public const string PRODUCT_NAME = "ProductName";
        public const string ICON = "Icon";
        public const string PRICE = "Price";

        public enum ProductTypes
        {
            Cake, Bread
        }
        public int Id { get; set; }
        public ProductTypes ProductType { get; set; }

        /// <summary>
        /// Rundstykke, kanelsnegl mm.
        /// </summary>
        /// <example>Rundstykke</example>
        public string ProductName { get; set; }
        
        /// <summary>
        /// Lækkert bagværk. Bagt med godt råvarer af din bagermester 
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// 15.75, 5.50 etc.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Dette er et ikon/billede i base64stringformat.
        /// </summary>
        public string Icon { get; set; }

        internal static Product MapFromReader(SqlDataReader reader)
        {
            Product product = new Product();
            product.Id = (int)reader[PRODUCT_ID];
            product.ProductName = (string)reader[PRODUCT_NAME];
            product.ProductDescription = (string)reader[PRODUCT_DESCRIPTION];
            product.Price = (double)reader[PRICE];
            product.ProductType = (ProductTypes)reader[PRODUCT_TYPE];
            product.Icon = reader[ICON]?.ToString();
            return product;
        }
    }
}
