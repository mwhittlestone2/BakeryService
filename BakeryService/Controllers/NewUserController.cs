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
    public class NewUserController : ControllerBase
    {
        private IConfiguration _configuration;
        public NewUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // POST api/<NewUserController>
        [HttpPost]
        public void Post([FromBody] LoginUser user)
        {
            string connectionString = _configuration.GetConnectionString("Default");
            BakeryDatabase db = new BakeryDatabase(connectionString);
            db.SaveUser(user);
        }

    }
}
