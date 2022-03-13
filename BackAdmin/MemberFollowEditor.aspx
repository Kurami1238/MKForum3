<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberFollowEditor.aspx.cs" Inherits="MKForum.BackAdmin.MemberFollowEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="lblMemberFollow_FollowStatus" runat="server"></asp:Label>
        <asp:Button ID="btnMemberFollow_FollowStatus" runat="server" Text="追蹤" Onclick="btnMemberFollow_FollowStatus_Click"/>
    </form>
</body>
</html>
