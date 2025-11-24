using System;

namespace Web_Join.Models
{
    public partical class User
    {
		public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
		public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
		public DateTime? PasswordChangedAt { get; set; }
	}
}
