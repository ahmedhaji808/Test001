<%@ Page Title="Trial Balance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrialBalance.aspx.cs" Inherits="AccountingSystem.TrialBalance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Trial Balance</h1>
	<div>
		<asp:TextBox ID="txtTbDate" runat="server" Placeholder="As of (yyyy-mm-dd)" />
		<asp:Button ID="btnTbLoad" runat="server" Text="Load" OnClick="btnTbLoad_Click" />
	</div>
	<asp:GridView ID="gvTrial" runat="server" AutoGenerateColumns="false">
		<Columns>
			<asp:BoundField DataField="AccountId" HeaderText="Account Id" />
			<asp:BoundField DataField="Debit" HeaderText="Debit" DataFormatString="{0:N2}" />
			<asp:BoundField DataField="Credit" HeaderText="Credit" DataFormatString="{0:N2}" />
			<asp:BoundField DataField="Net" HeaderText="Net (Debit-Credit)" DataFormatString="{0:N2}" />
		</Columns>
	</asp:GridView>
</asp:Content>