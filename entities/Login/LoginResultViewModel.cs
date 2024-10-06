namespace entities
{
    public class LoginResultViewModel
    {
        public bool Successful { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }

        public DateTime? GenerationDate { get; set; }

    }
}
