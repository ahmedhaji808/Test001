using System;
using System.Globalization;
using AccountingSystem.DAL;

namespace AccountingSystem
{
	public partial class Ledger : System.Web.UI.Page
	{
		private readonly LedgerRepository repository = new LedgerRepository();

		protected void btnLoad_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(txtAccountId.Text.Trim(), out int accountId)) return;
			if (!DateTime.TryParseExact(txtFrom.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime from)) return;
			if (!DateTime.TryParseExact(txtTo.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime to)) return;
			gvLedger.DataSource = repository.GetAccountLedger(accountId, from, to);
			gvLedger.DataBind();
		}
	}
}