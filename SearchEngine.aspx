<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchEngine.aspx.cs" Inherits="MKForum.SearchEngine" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <asp:TextBox ID="SearchBox" runat="server"></asp:TextBox>
        <asp:DropDownList ID="srchAreaList" runat="server">
            <asp:ListItem Value="srchAllSite">全站</asp:ListItem>
            <asp:ListItem Value="srchPosts">當前子板塊</asp:ListItem>
            <asp:ListItem Value="srchWriter">作者</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click" />
        <br />

        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
            <div>查詢結果</div>
        </asp:PlaceHolder>

        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text="Label">
                    <%#Eval("PostID") %><br />
                    <%#Eval("Title") %><br />      <!--不知道怎麼顯示預覽內文-->
                </asp:Label>
            </ItemTemplate>
        </asp:Repeater>

        <asp:PlaceHolder ID="plcEmpty" runat="server" Visible="false">
            <div>無資料</div>
        </asp:PlaceHolder>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
