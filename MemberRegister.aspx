<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberRegister.aspx.cs" Inherits="MKForum.MemberRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <th>大頭照</th>
                    <td>
                        <asp:FileUpload class="button" ID="fuCoverImage" runat="server" />
                        <asp:Image ID="imgCoverImage" runat="server" />
                    </td>
                </tr>

                <tr>
                    <th>暱稱</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtMember_name"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <th>性別: </th>
                    <td>
                        <asp:RadioButtonList ID="rdbtnMember_SexList" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">男</asp:ListItem>
                            <asp:ListItem Value="2">女</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>

                <tr>
                    <th>生日</th>
                    <td>
                        <asp:TextBox ID="txtMember_Birthday" runat="server" TextMode="Date"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <th>帳號</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtAccount"></asp:TextBox>
                        <asp:Button ID="btnAccountcheck" runat="server" Text="帳號確認" OnClick="btnAccouncheck_Click" Type="Button" />
                        <asp:Literal ID="ltlAccountcheckmsg" runat="server"></asp:Literal>
                    </td>
                </tr>

                <tr>
                    <th>密碼</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox> <br />
                        <asp:TextBox runat="server" ID="txtPassword_Check" placeholder="密碼確認" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <th>Email</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtEmail" TextMode="Email"></asp:TextBox>
                    </td>
                </tr>



                <tr>
                    <th>驗證碼</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtCapt"></asp:TextBox>
                    </td>
                </tr>
            </table>


            <asp:Button runat="server" ID="btnSave" Text="儲存" OnClick="btnSave_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="取消" OnClick="btnCancel_Click" />
            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>

        </div>
    </form>
</body>
</html>
