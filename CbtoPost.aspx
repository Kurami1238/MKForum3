<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CbtoPost.aspx.cs" Inherits="MKForum.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/CboardsPage.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="StampButton">
        <asp:Repeater ID="rptStamp" runat="server" OnItemCommand="rptStamp_ItemCommand">
            <ItemTemplate>
                <asp:Button runat="server" ID="btnStamp" Text='<%# Eval("PostSort") %>' CommandName="btnStamp"  CommandArgument='<%# Eval("SortID") %>' />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="Cboard">
        <h1>J-POP </h1>
        <div class="content">
            <asp:Repeater ID="rptcBtoP" runat="server" OnItemCommand="rptcBtoP_ItemCommand">
                <ItemTemplate>
                    <a href="DisplayPost.aspx?CboardID=<%# Eval("CboardID")%>&PostID=<%# Eval("PostID")%>" title="前往：<%# Eval("Title")%>">
                        <div class="article" runat="server">
                            <div class="postP">
                                <asp:PlaceHolder runat="server" Visible='<%# !string.IsNullOrWhiteSpace(Eval("CoverImage") as string)%>'>
                                    <img id="imgPostP" src="<%# Eval("CoverImage") as string%>" width="300" height="300" />
                                </asp:PlaceHolder>
                            </div>
                            <h6>
                                    <asp:Literal ID="ltlPostT" runat="server" Text='<%# Eval("Title")%>'></asp:Literal>
                            </h6>
                            <p>
                                    <asp:Literal ID="ltlPostC" runat="server" Text='<%# Eval("PostCotent")%>'></asp:Literal>
                            </p>
                            <h3>
                                    <asp:Literal ID="ltlPostM" runat="server" Text='<%# Eval("MemberID") %>'></asp:Literal>
                            </h3>
                            <h4>
                                    <asp:Literal ID="ltlPostD" runat="server" Text='<%# (Eval("LastEditTime") != null)? "最後編輯： " + Eval("LastEditTime") : Eval("PostDate") %>'></asp:Literal>
                            </h4>
                                <asp:PlaceHolder ID="Nmphl" runat="server" Visible='<%# (string.Compare(Eval("MemberID").ToString(), HttpContext.Current.Session["MemberID"].ToString()) == 0)%>'>
                                    <asp:Button ID="btnPostEdit" runat="server" Text="編輯" CommandName="btnEditNmpost" CommandArgument='<%# Eval("PostID") %>' />
                                </asp:PlaceHolder>
                                <h5>
                                    <br />
                                </h5>
                        </div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <asp:Button ID="btnCreatePost" runat="server" Text="新增文章" OnClick="btnCreatePost_Click" />
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
