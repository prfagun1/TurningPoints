namespace entities
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
        public bool PasswordExpired { get; set; }
        public DateTime? GenerationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
