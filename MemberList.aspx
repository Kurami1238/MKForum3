<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberList.aspx.cs" Inherits="MKForum.MemberList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <asp:GridView ID="GVMembers" runat="server">
        </asp:GridView>

        <asp:PlaceHolder ID="phEmpty" runat="server">
            <div> 目前無資料 </div>
        </asp:PlaceHolder>

    </form>
</body>
</html>
