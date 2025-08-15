namespace AspnetCoreMvcFull.Domain.Entities;

public enum EntryType
{
  Expense = 0,
  Income = 1
}

public sealed class Entry
{
  public Guid Id { get; }
  public DateTime OccurredAt { get; }
  public decimal Amount { get; }
  public string? Category { get; }
  public string? Note { get; }
  public EntryType Type { get; }

  public Entry(Guid id, DateTime occurredAt, decimal amount, EntryType type, string? category, string? note)
  {
    Id = id;
    OccurredAt = occurredAt;
    Amount = amount;
    Category = category;
    Note = note;
    Type = type;
  }
}