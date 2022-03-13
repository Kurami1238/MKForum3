<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberStatus.aspx.cs" Inherits="MKForum.MemberStatus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Image ID="imgMember_PicPath" runat="server" />
            <br />
            <asp:Label ID="lblMember_NickName" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="lblMember_Account" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="lblMember_MemberStatus" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
