<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tryencryption.aspx.cs" Inherits="MKForum.BackAdmin.tryencryption" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        加密密碼:
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        解密密碼:
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click"/>

    </form>
</body>
</html>
