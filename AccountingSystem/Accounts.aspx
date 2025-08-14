<%@ Page Title="Accounts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="AccountingSystem.Accounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Chart of Accounts</h1>
	<asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="false">
		<Columns>
			<asp:BoundField DataField="Code" HeaderText="Code" />
			<asp:BoundField DataField="Name" HeaderText="Name" />
			<asp:BoundField DataField="Type" HeaderText="Type" />
			<asp:CheckBoxField DataField="IsActive" HeaderText="Active" />
		</Columns>
	</asp:GridView>

	<div class="actions">
		<h3>Add Account</h3>
		<asp:TextBox ID="txtCode" runat="server" Placeholder="Code" />
		<asp:TextBox ID="txtName" runat="server" Placeholder="Name" Width="280" />
		<asp:DropDownList ID="ddlType" runat="server">
			<asp:ListItem Value="1">Asset</asp:ListItem>
			<asp:ListItem Value="2">Liability</asp:ListItem>
			<asp:ListItem Value="3">Equity</asp:ListItem>
			<asp:ListItem Value="4">Revenue</asp:ListItem>
			<asp:ListItem Value="5">Expense</asp:ListItem>
		</asp:DropDownList>
		<asp:CheckBox ID="chkActive" runat="server" Checked="true" Text="Active" />
		<asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
	</div>
</asp:Content>