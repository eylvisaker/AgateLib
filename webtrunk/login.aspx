<%@ Page Theme="theme1"  Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
<center>
        <asp:Login ID="Login1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderPadding="4"
            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"
            ForeColor="#333333" OnAuthenticate="Login1_Authenticate">
            <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TextBoxStyle Font-Size="0.8em" />
            <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
        </asp:Login>
        
    <br />
    If you do not have an account, please
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Register.aspx">register</asp:HyperLink>.<br />
    </center>
</asp:Content>

