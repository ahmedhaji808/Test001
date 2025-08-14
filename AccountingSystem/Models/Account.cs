using System;

namespace AccountingSystem.Models
{
	public class Account
	{
		public int AccountId { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public AccountType Type { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAtUtc { get; set; }
	}
}