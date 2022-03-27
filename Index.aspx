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
                    <h1><%# Eval("Naiyo") %></h1>
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

                <a href="DisplayPost.aspx?CboardID=<%# Eval("CboardID") %>&PostID=<%# Eval("PostID") %>">
                    <div class="content">
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
                </a>

            </ItemTemplate>
        </asp:Repeater>


    </div>

</asp:Content>
