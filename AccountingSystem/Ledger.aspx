<%@ Page Title="Ledger" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ledger.aspx.cs" Inherits="AccountingSystem.Ledger" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>General Ledger</h1>
	<div>
		<asp:TextBox ID="txtAccountId" runat="server" Placeholder="Account Id" />
		<asp:TextBox ID="txtFrom" runat="server" Placeholder="From (yyyy-mm-dd)" />
		<asp:TextBox ID="txtTo" runat="server" Placeholder="To (yyyy-mm-dd)" />
		<asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
	</div>
	<asp:GridView ID="gvLedger" runat="server" AutoGenerateColumns="false">
		<Columns>
			<asp:BoundField DataField="EntryDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
			<asp:BoundField DataField="Reference" HeaderText="Reference" />
			<asp:BoundField DataField="Description" HeaderText="Description" />
			<asp:BoundField DataField="Debit" HeaderText="Debit" DataFormatString="{0:N2}" />
			<asp:BoundField DataField="Credit" HeaderText="Credit" DataFormatString="{0:N2}" />
		</Columns>
	</asp:GridView>
</asp:Content>