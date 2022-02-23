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
    public class ProductController : ControllerBase
    {
        IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<ItemController>
        /// <summary>
        /// Henter alle produkter
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            string connectionString = _configuration.GetConnectionString("Default");
            BakeryDatabase db = new BakeryDatabase(connectionString);

            return db.GetProducts();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>Gem et produkt</example>
        /// <param name="product"></param>
        [HttpPost]
        public void Post([FromBody] Product product)
        {
            string connectionString = _configuration.GetConnectionString("Default");
            BakeryDatabase db = new BakeryDatabase(connectionString);
            db.SaveProduct(product);
        }
    }
}
