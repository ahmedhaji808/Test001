using System;

namespace AccountingSystem.Models
{
	public class JournalLine
	{
		public int JournalLineId { get; set; }
		public int JournalEntryId { get; set; }
		public int AccountId { get; set; }
		public decimal Debit { get; set; }
		public decimal Credit { get; set; }
	}
}