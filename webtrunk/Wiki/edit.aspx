<%@ Page Language="C#" Theme="Theme1" MasterPageFile="~/Wiki/wiki.master" AutoEventWireup="true"
    CodeFile="edit.aspx.cs" Inherits="Wiki_edit" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="pageHeader" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <h1>
                    Editing
                    <asp:Label ID="pageNameLabel" runat="server" Text="Label"></asp:Label>
                </h1>
            </td>
            <td align="right" valign="bottom">
                This Page:
                <asp:HyperLink ID="viewPageLink" runat="server" NavigateUrl="wiki.aspx">Cancel Edit</asp:HyperLink>
                &nbsp;<asp:HyperLink ID="historyLink" runat="server" NavigateUrl="history.aspx">History</asp:HyperLink>
            </td>
            <td width="20px">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" runat="Server">
    <asp:AccessDataSource ID="pageContentData" runat="server" DataFile="~/App_Data/agate.mdb">
    </asp:AccessDataSource>
    <asp:Literal ID="editPretext" runat="server" Visible="False"></asp:Literal>
    <asp:TextBox ID="editContent" runat="server" BorderStyle="Solid" BorderWidth="1px"
        Height="450px" TextMode="MultiLine" Width="98%"></asp:TextBox>
    <br />
    <br />
    Copyright notice goes here.<br />
    <br />
    <asp:Button ID="saveButton" runat="server" Text="Save Page" OnClick="saveButton_Click" />
    <asp:Button ID="Button1" runat="server" Text="Preview" />
    <asp:Button ID="Button2" runat="server" Text="See Changes" />
</asp:Content>
