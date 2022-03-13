<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreatePost.aspx.cs" Inherits="MKForum.CreatePost" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            標題：<asp:TextBox runat="server" ID="Title"></asp:TextBox><br />
            內文：<textarea runat="server" id="PostCotent" cols="20" rows="2"></textarea><br />
            <asp:Button ID="btnSend" runat="server" Text="送出" OnClick="btnSend_Click"    />
            <asp:Literal runat="server" ID="ltlmsg"></asp:Literal>
        </div>
    </form>
</body>
</html>
