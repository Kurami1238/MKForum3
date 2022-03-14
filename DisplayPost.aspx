<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DisplayPost.aspx.cs" Inherits="MKForum.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="MainPost">
        <div>
            <asp:Label ID="lblTitle" runat="server" Text="標題"></asp:Label>
        </div>
        <asp:PlaceHolder runat="server" Visible='<%# this.lblMember.ID == HttpContext.Current.Session["MemberID"] as string%>'>
            <div>
                <asp:Button ID="btnEditPost" runat="server" Text="編輯文章" OnClick="btnEditPost_Click" />
            </div>
            <div>
                <asp:Button ID="btnDeletePost" runat="server" Text="刪除文章" OnClick="btnDeletePost_Click" />
            </div>
        </asp:PlaceHolder>
        <div>
            <asp:Label ID="lblMember" runat="server" Text="作者"></asp:Label>
        </div>
        <div>
            <asp:Label ID="lblFloor" runat="server" Text="樓層數"></asp:Label>
        </div>
        <div>
            <asp:Label ID="lblCotent" runat="server" Text="內文"></asp:Label>
        </div>
    </div>
    <div class="NomainPost">
        <asp:Repeater ID="rptNmP" runat="server">
            <ItemTemplate>
                <asp:PlaceHolder runat="server" Visible='<%# Eval("MemberID") == HttpContext.Current.Session["MemberID"]%>'>
                    <div>
                        <asp:Button ID="btnEditNmPost" runat="server" Text="編輯回覆" OnClick="btnEditNmPost_Click" />
                    </div>
                    <div>
                        <asp:Button ID="btnDeleteNmPost" runat="server" Text="刪除回覆" OnClick="btnDeleteNmPost_Click" />
                    </div>
                </asp:PlaceHolder>
                <asp:HiddenField runat="server" ID="hfNmPID" Value='<%# Eval("PostID")%>' />
                <a href="MemberStastus.aspx?MemberID=<%# Eval("MemberID")%>" title="前往：<%# Eval("MemberID")%>會員頁">
                    <div>
                        <asp:Label ID="lblNmMember" runat="server" Text='<%# Eval("MemberAccount")%>'></asp:Label>
                    </div>
                </a>
                <div>
                    <asp:Label ID="lblNmFloor" runat="server" Text='<%# Eval("Floor")+"樓" %>'></asp:Label>
                </div>
                <div>
                    <asp:Label ID="lblNmPostcotent" runat="server" Text='<%# Eval("PostCotent") %>'></asp:Label>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="CNNmPost">
        <asp:TextBox runat="server" ID="txtCNNmPost" placeholder="請輸入回覆訊息"></asp:TextBox>
        <asp:Button runat="server" ID="btnCNNmPost" onclick="btnCNNmPost_Click" Text="新增回覆"/>
    </div>
</asp:Content>
