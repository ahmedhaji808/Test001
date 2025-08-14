using System;
using AccountingSystem.DAL;
using AccountingSystem.Models;

namespace AccountingSystem
{
	public partial class Accounts : System.Web.UI.Page
	{
		private readonly ChartOfAccountsRepository repository = new ChartOfAccountsRepository();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGrid();
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
			{
				return;
			}
			Account account = new Account
			{
				Code = txtCode.Text.Trim(),
				Name = txtName.Text.Trim(),
				Type = (AccountType)int.Parse(ddlType.SelectedValue),
				IsActive = chkActive.Checked
			};
			repository.Create(account);
			ClearForm();
			BindGrid();
		}

		private void BindGrid()
		{
			gvAccounts.DataSource = repository.GetAll();
			gvAccounts.DataBind();
		}

		private void ClearForm()
		{
			txtCode.Text = string.Empty;
			txtName.Text = string.Empty;
			ddlType.SelectedIndex = 0;
			chkActive.Checked = true;
		}
	}
}