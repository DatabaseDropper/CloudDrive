namespace CloudDrive.Models.Auth
{
    public class AuthToken
    {
        public AuthToken(string token, string id, long expires, string username)
        {
            Token = token;
            Id = id;
            ExpiresAt = expires;
            UserName = username;
        }

        public string UserName { get; set; }

        public string Token { get; set; }

        public string Id { get; set; }

        public long ExpiresAt { get; set; }

        public LoginDiskInfo DiskInfo { get; set; }
    }
}
