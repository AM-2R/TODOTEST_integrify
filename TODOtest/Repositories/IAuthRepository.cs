using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOTEST.Repositories
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(string email, string password);
        Task UpdateUserAsync(User user);
    }

}
