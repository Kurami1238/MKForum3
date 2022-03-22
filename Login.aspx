<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MKForum.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        * {
            background-color: black;
            color: azure;
            font-family: Consolas;
            font-size: 22pt;
        }
    </style>
</head>
<body>
   <form id="form1" runat="server">
        <asp:PlaceHolder runat="server" ID="plcLogin">Account:
            <asp:TextBox ID="txtAccount" runat="server"></asp:TextBox><br />
            Password:
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:Button ID="btnLogin" runat="server" Text="登錄" OnClick="btnLogin_Click" /><br />
            測試版登入?
            <asp:CheckBox runat="server" ID="ckbskip" Checked="true" Visible="false"/><br />

            <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
        </asp:PlaceHolder>


        <asp:PlaceHolder runat="server" ID="plcUserInfo" >
            <asp:Literal ID="ltlAccount" runat="server" /><br />
            前往 <a href="CbtoPost.aspx">子版</a><br />
            前往 <a href="test.aspx">TEST</a><br />

        </asp:PlaceHolder>
    </form>
</body>
</html>
