<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ranking.aspx.cs" Inherits="MKForum.Ranking" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text="Label">
                    <%#Eval("PostID") %><br />
                    <%#Eval("Title") %><br />      <!--不知道怎麼顯示預覽內文-->
<%--                    <%#Eval("PostCotent") %><br />
                    <%#Eval("PostView") %><br />
                    <%#Eval("PostDate") %><br />--%>
                </asp:Label>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
