<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DisplayPost.aspx.cs" Inherits="MKForum.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Label ID="lblTitle" runat="server" text="標題"></asp:Label>
    </div>
    <div>
        <asp:Button ID="btnEditPost" runat="server" Text="編輯文章" onclick="btnEditPost_Click" />
    </div>
    <div>
        <asp:Button ID="btnDeletePost" runat="server" Text="刪除文章" onclick="btnDeletePost_Click" />
    </div>
    <div>
        <asp:Label ID="lblMember" runat="server" Text="作者"></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblFloor" runat="server" Text="樓層數"></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblCotent" runat="server" Text="內文"></asp:Label>
    </div>
</asp:Content>
