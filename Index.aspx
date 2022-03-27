<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MKForum.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/index.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="hot-cboards">
        <div class="title">
            <img src="./images/HOT.png">
            <h1>熱門標籤</h1>
        </div>
        <div class="content">
            <asp:Repeater ID="rptHotTags" runat="server">
                <ItemTemplate>
                    <h1> <%# Eval("Naiyo") %></h1>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>



    <div class="hot-posts">
        <div class="title">
            <img src="./images/HOT.png">
            <h1>熱門文章</h1>
        </div>

        <asp:Repeater ID="rptHotPosts" runat="server">
            <ItemTemplate>

                <div class="content">
                    <%--<a href="DisplayPost.aspx?CboardID=2&PostID=d54c4cc3-5063-49ba-88f2-ce5aae83fc66"></a>--%>
                    <h2><%# string.Format("{0} - {1}", Eval("Pname"), Eval("Cname"))%> </h2>
                    <div class="article">
                        <h1>標題:<%# Eval("Title") %></h1>
                        <div class="ppd">
                            <p class="ellipsis"><%# Eval("PostCotent") %></p>
                        </div>
                        <h3>發文者:<%# Eval("Account") %></h3>
                        <h4>日期:<%# Eval("PostDate") %></h4>
                        <h5>
                            <br />
                        </h5>
                    </div>
                </div>

                <%--              <asp:Label runat="server"><%# Eval("Title") %></asp:Label>
              <asp:Label runat="server"><%# Eval("PostCotent") %></asp:Label>--%>
            </ItemTemplate>
        </asp:Repeater>


    </div>

</asp:Content>
