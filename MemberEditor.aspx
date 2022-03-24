<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberEditor.aspx.cs" Inherits="MKForum.BackAdmin.MemberEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <asp:PlaceHolder ID="phEmpty" runat="server">
            <div> 無此會員</div>
        </asp:PlaceHolder>

        <table>
            <tr>
                <th> 名稱: </th>
                <td> <asp:TextBox ID="txtMember_name" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <th> 性別: </th>
                <td> 
                    <asp:RadioButtonList ID="rdbtnMember_SexList" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">男</asp:ListItem>
                        <asp:ListItem Value="2">女</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th> 生日: </th>
                <td> <asp:TextBox ID="txtMember_Birthday" runat="server" TextMode="Date"></asp:TextBox> </td>
            </tr>
            <tr>
                <th> 狀態: </th>
                <td> <asp:Label ID="lblMember_Status" runat="server"></asp:Label> </td>
            </tr>
            <tr>
                <th> 帳號: </th>
                <td> <asp:TextBox ID="txtMember_Account" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <th> 密碼: </th>
                <td> 
                    <asp:TextBox ID="txtMember_PassWord" runat="server" Visible="false"></asp:TextBox>
                     <br />
                    <asp:TextBox ID="txtMember_PassWord_Check" runat="server" Visible="false"></asp:TextBox>
                    <asp:Button ID="btnMember_Password" runat="server" Text="密碼變更" OnClick="btnMember_Password_Click" /> 
                </td>
            </tr>
            <tr>
                <th> E-MAIL: </th>
                <td> <asp:TextBox ID="txtMember_Mail" runat="server" TextMode="Email"></asp:TextBox> </td>
            </tr>
        </table>

        <asp:Button ID="btnSave" runat="server" Text="儲存" OnClick="btnSave_Click" />
        <br />
        <asp:Label ID="lblSave_notice" runat="server" Text=""></asp:Label>

    </form>
</body>
</html>
