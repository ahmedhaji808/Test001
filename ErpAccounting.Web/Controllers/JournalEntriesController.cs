using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ErpAccounting.Web.Data;
using ErpAccounting.Web.Models;

namespace ErpAccounting.Web.Controllers;

public class JournalEntriesController : Controller
{
	private readonly ApplicationDbContext _context;

	public JournalEntriesController(ApplicationDbContext context)
	{
		_context = context;
	}

	// GET: JournalEntries
	public async Task<IActionResult> Index()
	{
		var entries = await _context.JournalEntries
			.Include(e => e.Lines)
			.ThenInclude(l => l.Account)
			.OrderByDescending(e => e.Date)
			.ToListAsync();
		return View(entries);
	}

	// GET: JournalEntries/Details/5
	public async Task<IActionResult> Details(int? id)
	{
		if (id == null) return NotFound();
		var entry = await _context.JournalEntries
			.Include(e => e.Lines)
			.ThenInclude(l => l.Account)
			.FirstOrDefaultAsync(e => e.Id == id);
		if (entry == null) return NotFound();
		return View(entry);
	}

	// GET: JournalEntries/Create
	public IActionResult Create()
	{
		ViewBag.AccountId = new SelectList(_context.Accounts.OrderBy(a => a.Code), "Id", "Name");
		var entry = new JournalEntry
		{
			Lines = new List<JournalEntryLine>
			{
				new JournalEntryLine(),
				new JournalEntryLine()
			}
		};
		return View(entry);
	}

	// POST: JournalEntries/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(JournalEntry entry)
	{
		ValidateBalancedEntry(entry);
		if (ModelState.IsValid)
		{
			_context.Add(entry);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		ViewBag.AccountId = new SelectList(_context.Accounts.OrderBy(a => a.Code), "Id", "Name");
		return View(entry);
	}

	// GET: JournalEntries/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if (id == null) return NotFound();
		var entry = await _context.JournalEntries
			.Include(e => e.Lines)
			.FirstOrDefaultAsync(e => e.Id == id);
		if (entry == null) return NotFound();
		ViewBag.AccountId = new SelectList(_context.Accounts.OrderBy(a => a.Code), "Id", "Name");
		return View(entry);
	}

	// POST: JournalEntries/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, JournalEntry entry)
	{
		if (id != entry.Id) return NotFound();
		ValidateBalancedEntry(entry);
		if (ModelState.IsValid)
		{
			_context.Update(entry);
			// ensure lines with zero on both are removed
			entry.Lines = entry.Lines.Where(l => l.Debit != 0 || l.Credit != 0).ToList();
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		ViewBag.AccountId = new SelectList(_context.Accounts.OrderBy(a => a.Code), "Id", "Name");
		return View(entry);
	}

	// GET: JournalEntries/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null) return NotFound();
		var entry = await _context.JournalEntries
			.Include(e => e.Lines)
			.ThenInclude(l => l.Account)
			.FirstOrDefaultAsync(e => e.Id == id);
		if (entry == null) return NotFound();
		return View(entry);
	}

	// POST: JournalEntries/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var entry = await _context.JournalEntries.FindAsync(id);
		if (entry != null)
		{
			_context.JournalEntries.Remove(entry);
			await _context.SaveChangesAsync();
		}
		return RedirectToAction(nameof(Index));
	}

	private void ValidateBalancedEntry(JournalEntry entry)
	{
		var debit = entry.Lines.Sum(l => l.Debit);
		var credit = entry.Lines.Sum(l => l.Credit);
		if (debit != credit)
		{
			ModelState.AddModelError(string.Empty, $"Debits ({debit:N2}) must equal Credits ({credit:N2}).");
		}
	}
}