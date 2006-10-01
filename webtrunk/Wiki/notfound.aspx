<%@ Page Theme="theme1"  Language="C#" MasterPageFile="~/Wiki/wiki.master" AutoEventWireup="true" CodeFile="notfound.aspx.cs" Inherits="Wiki_Default2" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    Page "<asp:Literal ID="pageName" runat="server"></asp:Literal>"
    was not found.<br />
    <br />
    The page you requested was not found.&nbsp; If this error came from a link you clicked
    on this site, please notify the webmaster.<br />
    <br />
    <asp:HyperLink ID="linkhome" runat="server" NavigateUrl="wiki.aspx">Return to Wiki</asp:HyperLink>
</asp:Content>

