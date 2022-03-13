<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlackMember.aspx.cs" Inherits="MKForum.BlackMember" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="blackedAccount" runat="server"></asp:TextBox>
        <br />
        <asp:TextBox ID="RealseDate" runat="server" TextMode="DateTime"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" Text="加入黑名單" Onclick="Button1_Click"/>
        <hr />
        <asp:Label ID="Label1" runat="server" Text="懲處中名單"></asp:Label><br />
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text="Label">
                    <%#Eval("Account") %><br />    <!--被黑會員帳號-->
                    <!--%#Eval("NickName") %>   忘了怎麼一下子合併兩個表-->
                    <%#Eval("ReleaseDate") %><br />
                </asp:Label>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
