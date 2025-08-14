using System.ComponentModel.DataAnnotations;

namespace ErpAccounting.Web.Models;

public class JournalEntry
{
	public int Id { get; set; }

	[DataType(DataType.Date)]
	[Display(Name = "Entry Date")]
	public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

	[StringLength(50)]
	public string? Reference { get; set; }

	[StringLength(500)]
	public string? Description { get; set; }

	public List<JournalEntryLine> Lines { get; set; } = new List<JournalEntryLine>();
}

public class JournalEntryLine
{
	public int Id { get; set; }

	[Required]
	public int JournalEntryId { get; set; }
	public JournalEntry? JournalEntry { get; set; }

	[Required]
	public int AccountId { get; set; }
	public Account? Account { get; set; }

	[Range(0, 999999999999.99)]
	public decimal Debit { get; set; }

	[Range(0, 999999999999.99)]
	public decimal Credit { get; set; }

	[StringLength(200)]
	public string? Memo { get; set; }
}