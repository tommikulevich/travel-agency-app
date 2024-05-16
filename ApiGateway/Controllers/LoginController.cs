using ApiGateway.Data;
using ApiGateway.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Trip.Dtos;
using Shared.Trip.Events;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IUserRepo _userRepo;
        public LoginController(IUserRepo UserRepo)
        {
            _userRepo = UserRepo;
        }
        [HttpPost]
        [Route("Auth")]
        public IActionResult Auth(string UserName, string Password)
        {
            var valid = false;

            Guid found = Guid.Parse("00000000-0000-0000-0000-000000000000");
            List<User> users = _userRepo.GetAllUsers();  
            foreach(var user in users)
            {
                if(user.Password == Password && user.UserName == UserName)
                {
                    valid = true;
                    found = user.Id;
                    break;
                }
            }

            if (valid)
            {
                return Ok(new { UserId = found.ToString() });
            }
            else
            {
                return NotFound();
            }
        }

    }

}
