﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MKForum.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/index.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="hot-cboards">
        <div class="title">
            <img src="./images/HOT.png">
            <h1>熱門討論區</h1>
        </div>
        <div class="content">
            <h1>K-POP </h1>
            <h1>跑跑薑餅人 </h1>
            <h1>哈利波特 </h1>
            <h1>迷因分享區 </h1>
        </div>
    </div>



    <div class="hot-posts">
        <div class="title">
            <img src="./images/HOT.png">
            <h1>熱門文章</h1>
        </div>


        <%--        <div class="content">
            <h2>音樂討論區 </h2>
            <div class="article">
                <h1>標題:神滅之刃OP Lisa</h1>
                <p>The Last Take版本聽到我都哭了....(繼續閱讀)</p>
                <h3>發文者:LoveLeeSA_666</h3>
                <h4>日期:2022/02/12</h4>
                <h5>
                    <br />
                </h5>
            </div>
        </div>--%>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>

                <div class="content">
                    <%--<a href="DisplayPost.aspx?CboardID=2&PostID=d54c4cc3-5063-49ba-88f2-ce5aae83fc66"></a>--%>
                    <h2>音樂討論區 </h2>
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
