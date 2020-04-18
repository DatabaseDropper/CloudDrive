using System;
using System.Collections.Generic;

namespace CloudDrive.Models
{
	public class User
	{
		private User()
		{

		}

		private User(string login, string username, string email, string passwordHash)
		{
			Login = login;
			Username = username;
			Email = email;
			PasswordHash = passwordHash;
		}

		public Guid Id { get; private set; } = Guid.NewGuid();

		public string Login { get; private set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public string PasswordHash { get; set; }

		public Guid DiskId { get; private set; }

		public Disk Disk { get; private set; }

		public List<UserAccess> SharedFolders { get; private set; } = new List<UserAccess>();

		public DateTime RegistrationDate { get; private set; } = DateTime.Now;

		public static User CreateUser(string Login, string Username, string Email, long DiskSizeAsKB)
		{
			var user = new User(Login, Username, Email, Guid.NewGuid().ToString("N"));
			var disk = Disk.CreateDisk(DiskSizeAsKB);
			user.Disk = disk;
			user.DiskId = disk.Id;
			return user;
		}
	}
}
