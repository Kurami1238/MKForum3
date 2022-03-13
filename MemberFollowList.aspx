<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberFollowList.aspx.cs" Inherits="MKForum.MemberFollowList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">


        <asp:Repeater ID="rptMemberFollows" runat="server">
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server"> 
                    <%# !(bool)Eval("Replied")
                            ? Eval("PointID") == null 

                               ? Eval("LastEditTime") == null 
                                    ? string.Format("新文章:「{0}」 {1}", Eval("Title"), Eval("PostDate", "{0:yyyy/MM/dd}")) 
                                    : string.Format("新文章:「{0}」 {1}", Eval("Title"), Eval("LastEditTime", "{0:yyyy/MM/dd}"))

                                : Eval("LastEditTime") == null 
                                    ? string.Format("第{0}樓 新回復:「{1}」 {2}", Eval("Floor"), Eval("PostCotent"), Eval("PostDate", "{0:yyyy/MM/dd}"))
                                    : string.Format("第{0}樓 新回復:「{1}」 {2}", Eval("Floor"), Eval("PostCotent"), Eval("LastEditTime", "{0:yyyy/MM/dd}")) 

                            : ""
                    %> 
                </asp:Label>

                <br />
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
