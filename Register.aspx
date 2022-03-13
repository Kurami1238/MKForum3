<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="MKForum.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        帳號:<asp:TextBox ID="txtAcc" runat="server"></asp:TextBox>
        <asp:Button ID="btnAcc" runat="server" Text="帳號驗證" />
        <asp:Literal ID="ltlAcc" runat="server" Text="-"></asp:Literal></br>
        密碼:<asp:TextBox ID="txtPwd" runat="server"></asp:TextBox></br>
        E-mail:<asp:TextBox ID="txtMail" runat="server"></asp:TextBox></br>
        驗證碼:<asp:TextBox ID="txtCapt" runat="server"></asp:TextBox></br>

    </form>
</body>
</html>
