using Microsoft.EntityFrameworkCore;

namespace Web_Join.Models
{
    public partial class BigTicketSystemContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.PasswordHash)
                    .HasColumnType("varbinary(256)")
                    .HasComment("PBKDF2 Hash");
                entity.Property(u => u.PasswordSalt)
                    .HasColumnType("varbinary(128)")
                    .HasComment("Zufälliges Salt");
                entity.Property(u => u.PasswordChangedAt)
                    .HasComment("Zeitpunkt der letzten Passwortänderung");
            });
        }
    }
}
