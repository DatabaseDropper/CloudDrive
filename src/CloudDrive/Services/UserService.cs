using CloudDrive.Database;
using CloudDrive.Models;
using CloudDrive.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Services
{
    public class UserService
    {
        private readonly Context _context;

        public UserService(Context context)
        {
            _context = context;
        }

        public async Task<User> TryGetUserAsync(Guid? id)
        {
            if (id == null)
                return null;

            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }      
    }
}
