using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpAccounting.Web.Models;

public enum AccountType
{
	Asset = 0,
	Liability = 1,
	Equity = 2,
	Revenue = 3,
	Expense = 4
}

public enum NormalBalance
{
	Debit = 0,
	Credit = 1
}

public class Account
{
	public int Id { get; set; }

	[Required]
	[StringLength(20)]
	public string Code { get; set; } = string.Empty;

	[Required]
	[StringLength(100)]
	public string Name { get; set; } = string.Empty;

	[Required]
	public AccountType Type { get; set; }

	[Required]
	public NormalBalance NormalBalance { get; set; }

	public bool IsActive { get; set; } = true;

	public int? ParentAccountId { get; set; }
	public Account? ParentAccount { get; set; }
	public ICollection<Account> ChildAccounts { get; set; } = new List<Account>();
}