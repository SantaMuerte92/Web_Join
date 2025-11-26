using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Web_Join.Models;


namespace Web_Join.Securitiy
{ 
    public class SecurityOptions
    {
    public string Pepper { get; set; } = string.Empty;
    public int Iterations { get; set; } = 150_000;
    public int SaltSize { get; set; } = 32;
    public int HashSize { get; set; } = 32;
}

    public interface IPasswordHasher
    {
        (byte[] Hash, byte[] Salt) HashPassword(string password);
        bool Verify(string password, byte[] storedHash, byte[] storedSalt);
    }



    public class PasswordHasher : IPasswordHasher
    {
        private readonly SecurityOptions _opt;
        public PasswordHasher(IOptions<SecurityOptions> options) => _opt = options.Value;

        public (byte[] Hash, byte[] Salt) HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Passwort leer.", nameof(password));

            var salt = RandomNumberGenerator.GetBytes(_opt.SaltSize);
            var hash = Derive(password, salt);
            return (hash, salt);
        }

        public bool Verify(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash.Length == 0 || storedSalt.Length == 0) return false;
            var calc = Derive(password, storedSalt);
            return CryptographicOperations.FixedTimeEquals(calc, storedHash);
        }

        private byte[] Derive(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password + _opt.Pepper, salt, _opt.Iterations, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(_opt.HashSize);
        }



    }
}
