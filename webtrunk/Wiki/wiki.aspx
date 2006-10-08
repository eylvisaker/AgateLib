<%@ Page Theme="Theme1" Language="C#" MasterPageFile="~/Wiki/wiki.master" AutoEventWireup="true" CodeFile="wiki.aspx.cs" Inherits="Wiki_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server"><asp:Literal ID="content" runat="server"></asp:Literal>
    <asp:SqlDataSource ID="pageContentData" runat="server" ConnectionString="<%$ ConnectionStrings:Access %>"
        ProviderName="<%$ ConnectionStrings:Access.ProviderName %>"></asp:SqlDataSource>    
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="pageHeader">
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Right" Width="95%">
        This Page:
        <asp:HyperLink ID="editPageLink" runat="server" NavigateUrl="edit.aspx">Edit</asp:HyperLink>
        &nbsp; &nbsp;<asp:HyperLink ID="historyLink" runat="server" NavigateUrl="history.aspx">History</asp:HyperLink>
    </asp:Panel>
</asp:Content>

