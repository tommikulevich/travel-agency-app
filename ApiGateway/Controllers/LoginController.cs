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
            private readonly List<User> users = new() { 
            new User { Id = Guid.Parse("ed35e5bf-c0b7-45db-8312-640e517b2414"), UserName = "Ala", Password = "MaKota"},
            new User { Id = Guid.Parse("a33daeee-3245-4cae-88ed-e2643c87a482"), UserName = "Kevin", Password = "SamWDomu"},
            new User { Id = Guid.Parse("21d90ae6-f2f3-4552-85ce-5fbb95d2eee2"), UserName = "User1", Password = "1resU"},
            new User { Id = Guid.Parse("82041dce-2adf-4af7-968d-011c3c59e5c2"), UserName = "User2", Password = "2resU"},
            new User { Id = Guid.Parse("3c89c7b5-f834-47c0-8c5e-8fe0c8d222bc"), UserName = "Alicja", Password = "Bogdan"},
            new User { Id = Guid.Parse("7051c674-adcc-420f-944b-4feda1df0c16"), UserName = "Bogdan", Password = "Alicja"},
            new User { Id = Guid.Parse("b5642e66-bd5b-40f3-8065-d79fd90d95c4"), UserName = "Fortuna", Password = "KolemSieToczy"},
            new User { Id = Guid.Parse("14942299-2257-4ff8-b34d-fcdf5ea85479"), UserName = "Zagreus", Password = "Thanatos"},
            new User { Id = Guid.Parse("d29c03a4-55a5-48f0-92ae-4856ca21b61e"), UserName = "Naru", Password = "Mitsu"},
            new User { Id = Guid.Parse("70884a6e-48da-46ad-bbbd-b28849de6ea8"), UserName = "SanLang", Password = "XieLian"},
        };
        [HttpPost]
        [Route("Auth")]
        public IActionResult Auth(string UserName, string Password)
        {
            var valid = false;
            // check for valid login credentials
            Guid found = Guid.Parse("00000000-0000-0000-0000-000000000000");
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
