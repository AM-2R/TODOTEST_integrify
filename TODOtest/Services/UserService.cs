using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOTEST.Services
    {
        public class UserService : IUserService
        {
            private readonly DataContext _context;

            public UserService(DataContext context)
            {
                _context = context;
            }

            public User Authenticate(string email, string password)
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return null;

                var user = _context.Users.SingleOrDefault(x => x.Email == email);

                // check if user exists
                if (user == null)
                    return null;

                // check if password is correct
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;

                // authentication successful
                return user;
            }

            public async Task<User> Create(User user, string password)
            {
                // validation
                if (string.IsNullOrWhiteSpace(password))
                    throw new AppException("Password is required");

                if (_context.Users.Any(x => x.Email == user.Email))
                    throw new AppException("Email \"" + user.Email + "\" is already taken");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }

            public async Task UpdatePassword(int userId, string currentPassword, string newPassword)
            {
                var user = _context.Users.Find(userId);

                if (user == null)
                    throw new AppException("User not found");

                if (!VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
                    throw new AppException("Current password is incorrect");

                if (string.IsNullOrWhiteSpace(newPassword))
                    throw new AppException("New password is required");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            // private helper methods

            private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
            {
                if (password == null)
                    throw new ArgumentNullException("password");
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }

            private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
            {
                if (password == null) throw new ArgumentNullException("password");
                if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
                if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
                if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordSalt");

                using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != storedHash[i]) return false;
                    }
                }

                return true;
            }
        }
    }
