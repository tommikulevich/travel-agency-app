using Microsoft.AspNetCore.Mvc;
using ApiGateway.Data;
using ApiGateway.Models;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public LoginController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        [Route("Auth")]
        public IActionResult Auth(string userName, string password)
        {
            var valid = false;
            Guid found = Guid.Parse("00000000-0000-0000-0000-000000000000");
            
            List<User> users = _userRepo.GetAllUsers();  
            foreach(var user in users)
            {
                if(user.Password == password && user.UserName == userName)
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
                return Ok(new { UserId = "0" });
            }
        }
    }
}
