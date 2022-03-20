<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DisplayPost.aspx.cs" Inherits="MKForum.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Display.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="MainPost col-sm-11 col-md-11 col-lg-11">
        <div class="PostT">
            <asp:Label ID="lblTitle" runat="server" Text="標題"></asp:Label>
        </div>
        <asp:PlaceHolder ID="phl" runat="server" Visible='<%# (string.Compare(this.hfMemberID.Value, HttpContext.Current.Session["MemberID"].ToString()) == 0) %>'>
            <asp:HiddenField runat="server" ID="hfMemberID" />
            <div class="Postbtn">
                <div>
                    <asp:Button ID="btnEditPost" runat="server" Text="編輯文章" OnClick="btnEditPost_Click" />
                </div>
                <div>
                    <asp:Button ID="btnDeletePost" runat="server" Text="刪除文章" OnClick="btnDeletePost_Click" OnClientClick="dela();" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="PostM">
            <asp:Label ID="lblMember" runat="server" Text="作者"></asp:Label>
        </div>
        <div class="PostF">
            <asp:Label ID="lblFloor" runat="server" Text="樓層數"></asp:Label>
        </div>
        <div class="PostC">
            <asp:Label ID="lblCotent" runat="server" Text="內文"></asp:Label>
        </div>
    </div>
    <div class="nomainPost col-sm-11 col-md-11 col-lg-11">
        <asp:Repeater ID="rptNmP" runat="server" OnItemCommand="rptNmP_ItemCommand">
            <ItemTemplate>
                <asp:PlaceHolder ID="Nmphl" runat="server" Visible='<%# (string.Compare(Eval("MemberID").ToString(), HttpContext.Current.Session["MemberID"].ToString()) == 0)%>'>
                    <div class="nmbtn">
                        <div>
                            <asp:Button ID="btnEditNmPost" runat="server" Text="編輯回覆" CommandName="btnEditNmpost" CommandArgument='<%# Eval("PostID") %>' />
                        </div>
                        <div>
                            <asp:Button ID="btnDeleteNmPost" CommandName="btnDeleteNmPost" runat="server" Text="刪除回覆" CommandArgument='<%# Eval("PostID")%>' OnClientClick="dela();" />
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:HiddenField runat="server" ID="hfNmPID" Value='<%# Eval("PostID")%>' />
                <a href="MemberStastus.aspx?MemberID=<%# Eval("MemberID")%>" title="前往：<%# Eval("MemberID")%>會員頁">
                    <div class="nmM">
                        <asp:Label ID="lblNmMember" runat="server" Text='<%# Eval("MemberAccount")%>'></asp:Label>
                    </div>
                </a>
                <div class="nmF">
                    <asp:Label ID="lblNmFloor" runat="server" Text='<%# Eval("Floor")+"樓" %>'></asp:Label>
                </div>
                <div class="nmC">
                    <asp:Label ID="lblNmPostcotent" runat="server" Text='<%# Eval("PostCotent") %>'></asp:Label>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="CNNmPost">
        <asp:TextBox runat="server" ID="txtCNNmPost" placeholder="請輸入回覆訊息"></asp:TextBox>
        <asp:Button runat="server" ID="btnCNNmPost" OnClick="btnCNNmPost_Click" Text="新增回覆" OnClientClick="NmCreatePosta();" />
    </div>
    <script>
        function dela() { alert('刪除成功') }
        function NmCreatePosta() { alert("回覆成功") }
    </script>
</asp:Content>
