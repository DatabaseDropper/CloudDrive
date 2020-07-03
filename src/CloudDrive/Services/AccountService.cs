using CloudDrive.Database;
using CloudDrive.Interfaces;
using CloudDrive.Miscs;
using CloudDrive.Models.Auth;
using CloudDrive.Models.Entities;
using CloudDrive.Models.Input;
using CloudDrive.Models.ViewModels;
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
            var errors = new Dictionary<string, List<string>>()
            {
                { "email", new List<string>() },
                { "login", new List<string>() },
            };

            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == input.Email.ToLower()))
                errors["email"].Add("Ten adres e-mail został już użyty!");

            if (await _context.Users.AnyAsync(x => x.Login.ToLower() == input.Login.ToLower()))
                errors["login"].Add("Ten login jest już używany");

            if (errors.Any(x => x.Value.Any()))
                return new Result<AuthToken>(false, null, errors, ErrorType.BadRequest);

            var user = User.CreateUser(input.Login, input.UserName, input.Email, 50.MBtoKB());
            user.PasswordHash = _hasher.HashPassword(user, input.Password);

            var token = _tokenService.BuildToken(user);

            token.DiskInfo = new LoginDiskInfo
            {
                DiskId = user.DiskId,
                DiskName = user.Disk.Name,
                FolderId = user.Disk.FolderId
            };

            var result = await _context.Users.AddAsync(user);
            var changedRows = await _context.SaveChangesAsync();

            if (changedRows == 0)
            {
                return new Result<AuthToken>(false, null, "Something went wrong", ErrorType.Internal);
            }

            return new Result<AuthToken>(true, token);
        }

        public async Task<Result<AccountInfoViewModel>> ObtainAccountInfo(User user)
        {
            if (user == null)
            {
                return new Result<AccountInfoViewModel>(false, null, "Data is not received", ErrorType.BadRequest);
            }

            var disk = await _context.Disks.FirstOrDefaultAsync(x => x.Id == user.DiskId);

            var result = new AccountInfoViewModel
            {
                DiskId = disk.Id,
                DiskName = disk.Name,
                FreeSpace = disk.FreeSpace,
                TotalSpace = disk.TotalSpace,
                UsedSpace = disk.UsedSpace,
                MainFolderId = disk.FolderId,
                UserName = user.Username
            };

            return new Result<AccountInfoViewModel>(true, result);
        }

        public async Task<Result<AuthToken>> TrySignIn(LoginInput input)
        {
            var user = await _context
                .Users
                .Include(x => x.Disk)
                .FirstOrDefaultAsync(x => 
                x.Email.ToLower() == input.LoginOrEmail.ToLower() 
                ||
                x.Login.ToLower() == input.LoginOrEmail.ToLower()
            );

            if (user == null)
            {
                return new Result<AuthToken>(false, null, "Incorrect credentials.", ErrorType.BadRequest);
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, input.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return new Result<AuthToken>(false, null, "Incorrect credentials.", ErrorType.BadRequest);
            }

            var token = _tokenService.BuildToken(user);

            token.DiskInfo = new LoginDiskInfo
            {
                DiskId = user.DiskId,
                DiskName = user.Disk.Name,
                FolderId = user.Disk.FolderId
            };

            return new Result<AuthToken>(true, token);
        }
    }
}
