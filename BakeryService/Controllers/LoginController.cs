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
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginUser user)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("Default");
                BakeryDatabase db = new BakeryDatabase(connectionString);
                LoginUser loginUser = db.GetUser(user.Email);
                if (loginUser == null)
                {
                    throw new Exception("User does not exist");
                }
                if (loginUser.Password != user.Password)
                {
                    throw new Exception("Unknown user or password");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                
            }
            return Ok();
        }


    }
}
