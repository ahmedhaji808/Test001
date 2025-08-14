using ErpSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ErpSystem.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
        public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Code).IsRequired();
                b.Property(x => x.Name).IsRequired();
                b.HasIndex(x => x.Code).IsUnique();
                b.HasOne(x => x.ParentAccount)
                    .WithMany()
                    .HasForeignKey(x => x.ParentAccountId);
            });

            modelBuilder.Entity<JournalEntry>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Date).HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v));
                b.Property(x => x.Reference).IsRequired();
                b.HasMany(x => x.Lines)
                    .WithOne(x => x.JournalEntry!)
                    .HasForeignKey(x => x.JournalEntryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<JournalEntryLine>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Account)
                    .WithMany()
                    .HasForeignKey(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.Property(x => x.Debit).HasPrecision(18, 2);
                b.Property(x => x.Credit).HasPrecision(18, 2);
            });
        }
    }
}