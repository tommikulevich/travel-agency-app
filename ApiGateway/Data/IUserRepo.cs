using ApiGateway.Models;

namespace ApiGateway.Data
{
    public interface IUserRepo
    {
        List<User> GetAllUsers();
    }
}