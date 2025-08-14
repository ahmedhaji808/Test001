using System;
using System.Globalization;
using System.Linq;
using AccountingSystem.DAL;

namespace AccountingSystem
{
	public partial class TrialBalance : System.Web.UI.Page
	{
		private readonly JournalRepository repository = new JournalRepository();

		protected void btnTbLoad_Click(object sender, EventArgs e)
		{
			if (!DateTime.TryParseExact(txtTbDate.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime asOf)) return;
			var rows = repository.GetTrialBalance(asOf)
				.Select(x => new { AccountId = x.AccountId, Debit = x.Debit, Credit = x.Credit, Net = x.Debit - x.Credit });
			gvTrial.DataSource = rows;
			gvTrial.DataBind();
		}
	}
}