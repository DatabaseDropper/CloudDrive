using CloudDrive.Models;
using CloudDrive.Models.Auth;

namespace CloudDrive.Interfaces
{
    public interface ITokenService
    {
        public AuthToken BuildToken(User user);
    }
}