using BCrypt.Net;

namespace ChandiTechnologyWebApi.Classes
{
    public class PasswordHasher
    {


        public string HashPassword(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }

        public bool VerifyPassword(string plainTextPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, storedHash);
        }
    }
}
