using System;
using System.Collections.Generic;

namespace ErpSystem.Domain.Entities
{
    public class JournalEntry
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string? Memo { get; set; }
        public List<JournalEntryLine> Lines { get; set; } = new();
    }
}