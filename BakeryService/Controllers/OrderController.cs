using BakeryService.DAL;
using BakeryService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BakeryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IConfiguration _configuration;
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<OrderController>
        [Route("[action]/{orderId}")]
        [HttpGet]
        //[Route("GetOrder")]
        public Order GetOrder(string orderId)
        {
            BakeryDatabase bakeryDatabase = new BakeryDatabase(_configuration.GetConnectionString("Default"));
            return bakeryDatabase.GetOrder(orderId);
        }


        // GET: api/<OrderController>
        [Route("[action]/{userId}")]
        [HttpGet]
        public IEnumerable<Order> GetOrders(string userId)
        {
            BakeryDatabase bakeryDatabase = new BakeryDatabase(_configuration.GetConnectionString("Default"));
            return bakeryDatabase.GetOrders(userId);
        }

        [HttpGet]
        public IEnumerable<Order> Get()
        {
            BakeryDatabase bakeryDatabase = new BakeryDatabase(_configuration.GetConnectionString("Default"));
            return bakeryDatabase.GetOrders();
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            try
            {
                BakeryDatabase bakeryDatabase = new BakeryDatabase(_configuration.GetConnectionString("Default"));
                LoginUser user = bakeryDatabase.GetUser(order.CustomerId);
                if (user == null)
                {
                    throw new Exception("Invalid User");
                }

                if (order.OrderProducts == null)
                {
                    throw new Exception("'OrderProducts' property must not be null. Rember to add list of products.");
                }

                double total = 0;
                var productsWithAmount = order.OrderProducts.Where(o => o.Amount > 0).ToList();
                if (productsWithAmount.Count == 0)
                {
                    throw new Exception("Order must contain an amount of no less than 1 product");
                }

                foreach (OrderProduct orderProduct in productsWithAmount)
                {

                    Product product = bakeryDatabase.GetProduct(orderProduct.ProductId);
                    total += product.Price * orderProduct.Amount;
                }

                if (total <= 0)
                {
                    throw new Exception("Invalid order - subtotal was zero");
                }

                order.OrderTotal = total;
                bakeryDatabase.SaveOrder(order);
            }
            catch (Exception e)
            {
              
                return BadRequest(e.Message);
            }
         

            return Ok();
        }

      

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            BakeryDatabase bakeryDatabase = new BakeryDatabase(_configuration.GetConnectionString("Default"));
            bakeryDatabase.DeleteOrder(id);
        }
    }
}
