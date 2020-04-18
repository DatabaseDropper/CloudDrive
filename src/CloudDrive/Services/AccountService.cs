using CloudDrive.Database;
using CloudDrive.Interfaces;
using CloudDrive.Miscs;
using CloudDrive.Models;
using CloudDrive.Models.Auth;
using CloudDrive.Models.Input;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Services
{
    public class AccountService
    {
        private readonly Context _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly ITokenService _tokenService;

        public AccountService(Context context, IPasswordHasher<User> hasher, ITokenService tokenService)
        {
            _context = context;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthToken>> TryRegister(RegisterInput input)
        {
            var conn = _context.Database.GetDbConnection().ConnectionString;
            var errors = new List<string>();

            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == input.Email.ToLower()))
                errors.Add("This e-mail address is already being used.");

            if (await _context.Users.AnyAsync(x => x.Login.ToLower() == input.Login.ToLower()))
                errors.Add("This login is already being used.");

            if (errors.Count > 0)
                return new Result<AuthToken>(false, null, errors);

            var user = User.CreateUser(input.Login, input.UserName, input.Email, 50.MBtoKB());
            user.PasswordHash = _hasher.HashPassword(user, input.Password);

            var token = _tokenService.BuildToken(user);
            return new Result<AuthToken>(true, token);
        }
    }
}
