using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErpAccounting.Web.Data;
using ErpAccounting.Web.Models;

namespace ErpAccounting.Web.Controllers;

public class AccountsController : Controller
{
	private readonly ApplicationDbContext _context;

	public AccountsController(ApplicationDbContext context)
	{
		_context = context;
	}

	// GET: Accounts
	public async Task<IActionResult> Index()
	{
		var accounts = await _context.Accounts
			.Include(a => a.ParentAccount)
			.OrderBy(a => a.Code)
			.ToListAsync();
		return View(accounts);
	}

	// GET: Accounts/Details/5
	public async Task<IActionResult> Details(int? id)
	{
		if (id == null) return NotFound();
		var account = await _context.Accounts
			.Include(a => a.ParentAccount)
			.FirstOrDefaultAsync(m => m.Id == id);
		if (account == null) return NotFound();
		return View(account);
	}

	// GET: Accounts/Create
	public IActionResult Create()
	{
		ViewData["ParentAccountId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Accounts.OrderBy(a => a.Code), "Id", "Name");
		return View();
	}

	// POST: Accounts/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create([Bind("Id,Code,Name,Type,NormalBalance,IsActive,ParentAccountId")] Account account)
	{
		if (ModelState.IsValid)
		{
			_context.Add(account);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		ViewData["ParentAccountId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Accounts.OrderBy(a => a.Code), "Id", "Name", account.ParentAccountId);
		return View(account);
	}

	// GET: Accounts/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if (id == null) return NotFound();
		var account = await _context.Accounts.FindAsync(id);
		if (account == null) return NotFound();
		ViewData["ParentAccountId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Accounts.Where(a => a.Id != id).OrderBy(a => a.Code), "Id", "Name", account.ParentAccountId);
		return View(account);
	}

	// POST: Accounts/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Type,NormalBalance,IsActive,ParentAccountId")] Account account)
	{
		if (id != account.Id) return NotFound();
		if (ModelState.IsValid)
		{
			try
			{
				_context.Update(account);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AccountExists(account.Id)) return NotFound();
				else throw;
			}
			return RedirectToAction(nameof(Index));
		}
		ViewData["ParentAccountId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Accounts.Where(a => a.Id != id).OrderBy(a => a.Code), "Id", "Name", account.ParentAccountId);
		return View(account);
	}

	// GET: Accounts/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null) return NotFound();
		var account = await _context.Accounts
			.Include(a => a.ParentAccount)
			.FirstOrDefaultAsync(m => m.Id == id);
		if (account == null) return NotFound();
		return View(account);
	}

	// POST: Accounts/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var account = await _context.Accounts.FindAsync(id);
		if (account != null)
		{
			_context.Accounts.Remove(account);
		}
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	private bool AccountExists(int id)
	{
		return _context.Accounts.Any(e => e.Id == id);
	}
}