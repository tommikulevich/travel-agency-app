using ApiGateway.Models;

namespace ApiGateway.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly UserDbContext _context;

        public UserRepo(UserDbContext context)
        {
            _context = context; 
        }

        public List<User> GetAllUsers()
        {
            var result = _context.User.ToList();
            return result;
        }
    }
}