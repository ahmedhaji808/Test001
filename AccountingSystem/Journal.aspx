<%@ Page Title="Journal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Journal.aspx.cs" Inherits="AccountingSystem.Journal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Journal Entry</h1>
	<div>
		<asp:TextBox ID="txtDate" runat="server" Placeholder="Date (yyyy-mm-dd)" />
		<asp:TextBox ID="txtRef" runat="server" Placeholder="Reference" />
		<asp:TextBox ID="txtDesc" runat="server" Placeholder="Description" Width="320" />
	</div>
	<h3>Lines</h3>
	<table>
		<tr>
			<th>Account Id</th>
			<th>Debit</th>
			<th>Credit</th>
		</tr>
		<tr>
			<td><asp:TextBox ID="txtAcc1" runat="server" Width="100" /></td>
			<td><asp:TextBox ID="txtDebit1" runat="server" Width="100" /></td>
			<td><asp:TextBox ID="txtCredit1" runat="server" Width="100" /></td>
		</tr>
		<tr>
			<td><asp:TextBox ID="txtAcc2" runat="server" Width="100" /></td>
			<td><asp:TextBox ID="txtDebit2" runat="server" Width="100" /></td>
			<td><asp:TextBox ID="txtCredit2" runat="server" Width="100" /></td>
		</tr>
	</table>
	<div class="actions">
		<asp:Button ID="btnPost" runat="server" Text="Post Entry" OnClick="btnPost_Click" />
		<asp:Label ID="lblMsg" runat="server" />
	</div>
</asp:Content>