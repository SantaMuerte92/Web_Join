namespace Web_Join.Models
{
    public class SecurityOptions
    {
        public string Pepper { get; set; } = string.Empty;
        public int Iterations { get; set; } = 150_000; // PBKDF2 Iterations
        public int SaltSize { get; set; } = 32;
        public int HashSize { get; set; } = 32;
    }
}
