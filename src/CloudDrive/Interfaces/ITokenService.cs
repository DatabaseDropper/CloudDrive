using CloudDrive.Models;
using CloudDrive.Models.Auth;
using CloudDrive.Models.Entities;

namespace CloudDrive.Interfaces
{
    public interface ITokenService
    {
        public AuthToken BuildToken(User user);
    }
}