using System;
using System.Globalization;
using AccountingSystem.DAL;
using AccountingSystem.Models;

namespace AccountingSystem
{
	public partial class Journal : System.Web.UI.Page
	{
		private readonly JournalRepository repository = new JournalRepository();

		protected void btnPost_Click(object sender, EventArgs e)
		{
			lblMsg.Text = string.Empty;
			if (!DateTime.TryParseExact(txtDate.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
			{
				lblMsg.Text = "Invalid date. Use yyyy-mm-dd.";
				return;
			}
			JournalEntry entry = new JournalEntry
			{
				EntryDate = date,
				Reference = string.IsNullOrWhiteSpace(txtRef.Text) ? null : txtRef.Text.Trim(),
				Description = string.IsNullOrWhiteSpace(txtDesc.Text) ? null : txtDesc.Text.Trim()
			};
			if (!TryAddLine(txtAcc1.Text, txtDebit1.Text, txtCredit1.Text, entry)) return;
			if (!TryAddLine(txtAcc2.Text, txtDebit2.Text, txtCredit2.Text, entry)) return;

			decimal totalDebit = 0, totalCredit = 0;
			foreach (var l in entry.Lines)
			{
				totalDebit += l.Debit;
				totalCredit += l.Credit;
			}
			if (totalDebit != totalCredit)
			{
				lblMsg.Text = "Entry not balanced.";
				return;
			}

			try
			{
				int id = repository.CreateEntry(entry);
				lblMsg.Text = "Posted entry #" + id;
			}
			catch (Exception ex)
			{
				lblMsg.Text = "Error: " + ex.Message;
			}
		}

		private bool TryAddLine(string accountIdText, string debitText, string creditText, JournalEntry entry)
		{
			if (string.IsNullOrWhiteSpace(accountIdText)) return true; // Optional line
			if (!int.TryParse(accountIdText.Trim(), out int accountId))
			{
				lblMsg.Text = "Invalid account id.";
				return false;
			}
			decimal debit = 0, credit = 0;
			if (!string.IsNullOrWhiteSpace(debitText) && !decimal.TryParse(debitText.Trim(), out debit))
			{
				lblMsg.Text = "Invalid debit.";
				return false;
			}
			if (!string.IsNullOrWhiteSpace(creditText) && !decimal.TryParse(creditText.Trim(), out credit))
			{
				lblMsg.Text = "Invalid credit.";
				return false;
			}
			entry.Lines.Add(new JournalLine { AccountId = accountId, Debit = debit, Credit = credit });
			return true;
		}
	}
}