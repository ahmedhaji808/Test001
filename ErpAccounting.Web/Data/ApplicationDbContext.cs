using ErpAccounting.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ErpAccounting.Web.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	public DbSet<Account> Accounts => Set<Account>();
	public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
	public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Account>(entity =>
		{
			entity.HasIndex(a => a.Code).IsUnique();
			entity.Property(a => a.Code).IsRequired().HasMaxLength(20);
			entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
			entity.HasOne(a => a.ParentAccount)
				.WithMany(a => a.ChildAccounts)
				.HasForeignKey(a => a.ParentAccountId)
				.OnDelete(DeleteBehavior.Restrict);
		});

		modelBuilder.Entity<JournalEntry>(entity =>
		{
			entity.Property(e => e.Reference).HasMaxLength(50);
			entity.Property(e => e.Description).HasMaxLength(500);
		});

		modelBuilder.Entity<JournalEntryLine>(entity =>
		{
			entity.Property(l => l.Debit).HasPrecision(18, 2);
			entity.Property(l => l.Credit).HasPrecision(18, 2);
			entity.Property(l => l.Memo).HasMaxLength(200);
			entity.HasOne(l => l.Account)
				.WithMany()
				.HasForeignKey(l => l.AccountId)
				.OnDelete(DeleteBehavior.Restrict);
			entity.HasOne(l => l.JournalEntry)
				.WithMany(e => e.Lines)
				.HasForeignKey(l => l.JournalEntryId)
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}