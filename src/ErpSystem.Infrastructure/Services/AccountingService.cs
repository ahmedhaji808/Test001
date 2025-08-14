using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErpSystem.Application.Interfaces;
using ErpSystem.Domain.Entities;
using ErpSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ErpSystem.Infrastructure.Services
{
    public class AccountingService : IAccountingService
    {
        private readonly AppDbContext _dbContext;

        public AccountingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }

        public async Task<IReadOnlyList<Account>> GetAccountsAsync()
        {
            var accounts = await _dbContext.Accounts
                .OrderBy(a => a.Code)
                .ToListAsync();
            return accounts;
        }

        public async Task<JournalEntry> PostJournalEntryAsync(JournalEntry entry)
        {
            if (entry.Lines == null || entry.Lines.Count < 2)
                throw new InvalidOperationException("A journal entry must have at least two lines.");

            var totalDebit = entry.Lines.Sum(l => l.Debit);
            var totalCredit = entry.Lines.Sum(l => l.Credit);
            if (totalDebit != totalCredit)
                throw new InvalidOperationException("Debits and credits must balance.");

            _dbContext.JournalEntries.Add(entry);
            await _dbContext.SaveChangesAsync();
            return entry;
        }

        public async Task<IReadOnlyList<JournalEntry>> GetJournalEntriesAsync(DateOnly? from = null, DateOnly? to = null)
        {
            var query = _dbContext.JournalEntries
                .Include(j => j.Lines)
                .AsQueryable();

            if (from.HasValue)
            {
                var fromDateTime = from.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(j => j.Date.ToDateTime(TimeOnly.MinValue) >= fromDateTime);
            }

            if (to.HasValue)
            {
                var toDateTime = to.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(j => j.Date.ToDateTime(TimeOnly.MinValue) <= toDateTime);
            }

            var entries = await query
                .OrderByDescending(j => j.Date)
                .ToListAsync();

            return entries;
        }
    }
}