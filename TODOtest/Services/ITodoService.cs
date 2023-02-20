using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOTEST.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}

}
