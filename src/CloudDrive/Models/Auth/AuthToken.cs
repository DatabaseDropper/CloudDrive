namespace CloudDrive.Models.Auth
{
    public class AuthToken
    {
        public AuthToken(string token, string id, long expires)
        {
            Token = token;
            Id = id;
            ExpiresAt = expires;
        }

        public string Token { get; set; }

        public string Id { get; set; }

        public long ExpiresAt { get; set; }
    }
}
