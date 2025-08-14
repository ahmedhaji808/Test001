using System;

namespace ErpSystem.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public AccountType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid? ParentAccountId { get; set; }
        public Account? ParentAccount { get; set; }
    }
}