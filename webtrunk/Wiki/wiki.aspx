<%@ Page Theme="Theme1" Language="C#" MasterPageFile="~/Wiki/wiki.master" AutoEventWireup="true" CodeFile="wiki.aspx.cs" Inherits="Wiki_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    
    <asp:AccessDataSource ID="pageContentData" runat="server" DataFile="~/App_Data/agate.mdb"></asp:AccessDataSource>
    <asp:Label ID="content" runat="server" Text="Label"></asp:Label>
    &nbsp;&nbsp;&nbsp;
    <br />
    <hr />
    Comments:<br />
    <asp:AccessDataSource ID="commentsData" runat="server" DataFile="~/App_Data/agate.mdb"></asp:AccessDataSource>
    <asp:DataList ID="DataList1" runat="server" CellPadding="4" ForeColor="#333333" BorderWidth="0px" Width="100%" >
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <AlternatingItemStyle BackColor="White" />
        <ItemStyle BackColor="#EFF3FB" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <ItemTemplate>
            Posted by
            <asp:Label ID="commentUserName" runat="server"><%# Eval("[\"UserName\"]") %></asp:Label>
            on
            <asp:Label ID="commentDate" runat="server"><%# Eval("[\"Date\"]") %></asp:Label><br />
            <br />
            <asp:Label ID="commentContent" runat="server"><%# Eval("[\"Comment\"]") %></asp:Label>
        </ItemTemplate>
        <SeparatorTemplate>
        </SeparatorTemplate>
    </asp:DataList>
</asp:Content>

