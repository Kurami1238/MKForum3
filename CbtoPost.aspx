<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CbtoPost.aspx.cs" Inherits="MKForum.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table border="1">
        <asp:Repeater ID="rptcBtoP" runat="server">
            <ItemTemplate>
                <a href="DisplayPost.aspx?CboardID=<%# Eval("CboardID")%>&PostID=<%# Eval("PostID")%>" title="前往：<%# Eval("Title")%>">
                    <div class="post" runat="server">
                        <div class="postP">
                            <asp:PlaceHolder runat="server" Visible='<%# !string.IsNullOrWhiteSpace(Eval("CoverImage") as string)%>'>
                                <img id="imgPostP" src="<%# Eval("CoverImage") as string%>" width="300" height="300" />
                            </asp:PlaceHolder>
                            <%--<img ID="imgPostP" src="<%# (!string.IsNullOrWhiteSpace(Eval("CoverImage") as string))?Eval("CoverImage"):""%>" width="200" height="200"/>--%>
                        </div>
                        <div class="postT">
                            <asp:Literal ID="ltlPostT" runat="server" Text='<%# Eval("Title")%>'></asp:Literal>
                        </div>
                        <div class="postC">
                            <asp:Literal ID="ltlPostC" runat="server" Text='<%# Eval("PostCotent")%>'></asp:Literal>
                        </div>
                        <div class="postB" runat="server">
                            <asp:Button ID="btnPostEdit" runat="server" OnClick="btnPostEdit_Click" Text="編輯" />
                        </div>
                    </div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <asp:Button ID="btnCreatePost" runat="server" Text="新增文章" OnClick="btnCreatePost_Click" />
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
