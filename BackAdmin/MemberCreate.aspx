<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberCreate.aspx.cs" Inherits="MKForum.BackAdmin.MemberCreate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <th> 名稱: </th>
                <td> <asp:TextBox ID="txtMember_name" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <th> 性別: </th>
                <td> <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                        <asp:RadioButton runat="server"></asp:RadioButton>
                        <asp:RadioButton runat="server"></asp:RadioButton>
                     </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th> 生日: </th>
                <td> <asp:TextBox ID="txtMember_Birthday" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <th> 狀態: </th>
                <td> <asp:TextBox ID="lblMember_Status" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <th> 帳號: </th>
                <td> <asp:TextBox ID="txtMember_Account" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <th> 密碼: </th>
                <td> <asp:TextBox ID="btnMember_Password" runat="server" Text="密碼" /> </td>
                <td> <asp:TextBox ID="TextBox1" runat="server" Text="確認密碼" /> </td>
            </tr>
            <tr>
                <th> E-MAIL: </th>
                <td> <asp:TextBox ID="txtMember_Mail" runat="server"></asp:TextBox> </td>
            </tr>
        </table>
    </form>
</body>
</html>
