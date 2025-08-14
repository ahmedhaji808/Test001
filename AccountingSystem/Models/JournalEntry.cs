using System;
using System.Collections.Generic;

namespace AccountingSystem.Models
{
	public class JournalEntry
	{
		public int JournalEntryId { get; set; }
		public DateTime EntryDate { get; set; }
		public string Reference { get; set; }
		public string Description { get; set; }
		public List<JournalLine> Lines { get; set; } = new List<JournalLine>();
	}
}