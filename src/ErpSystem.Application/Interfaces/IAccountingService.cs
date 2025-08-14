using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErpSystem.Domain.Entities;

namespace ErpSystem.Application.Interfaces
{
    public interface IAccountingService
    {
        Task<Account> CreateAccountAsync(Account account);
        Task<IReadOnlyList<Account>> GetAccountsAsync();
        Task<JournalEntry> PostJournalEntryAsync(JournalEntry entry);
        Task<IReadOnlyList<JournalEntry>> GetJournalEntriesAsync(DateOnly? from = null, DateOnly? to = null);
    }
}