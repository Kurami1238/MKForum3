<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DisplayPost.aspx.cs" Inherits="MKForum.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Display.css" rel="stylesheet" />
    <script src="js/showdown.js"></script>
    <link href="css/github-markdown-dark.css" rel="stylesheet" />
    <link href="css/github-markdown.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="MainPost col-sm-11 col-md-11 col-lg-11">
        <div class="mpostall">
            <div class="PostT">
                <asp:Label ID="lblTitle" runat="server" Text="標題"></asp:Label>
            </div>
            <div class="PostF">
                <asp:Label ID="lblFloor" runat="server" Text="樓層數"></asp:Label>
            </div>
            <div class="Postbtn">
                <asp:Label ID="lblMemberFollow_FollowStatus" runat="server"></asp:Label>
                <div>
                    <asp:Button class="btn" ID="btnMemberFollow_FollowStatus" runat="server" Text="追蹤" OnClick="btnMemberFollow_FollowStatus_Click" />
                </div>
                <asp:PlaceHolder ID="phl" runat="server" >
                    <asp:HiddenField runat="server" ID="hfMemberID" />
                    <div>
                        <asp:Button class="btn" ID="btnEditPost" runat="server" Text="編輯文章" OnClick="btnEditPost_Click" />
                    </div>
                    <div>
                        <asp:Button class="btn" ID="btnDeletePost" runat="server" Text="刪除文章" OnClick="btnDeletePost_Click" OnClientClick="dela();" />
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="PostM">
                <asp:Label ID="lblMember" runat="server" Text="作者"></asp:Label>
            </div>
            <div class="PostC">
                    <div class="result" id="result">
                     </div>
            <input type="hidden" id="sortid" class="content" runat="server"/>
                <%--<asp:Label ID="lblCotent" runat="server" Text="內文" ></asp:Label>--%>
            <hr />
            </div>
        </div>
    </div>
    <asp:Repeater ID="rptNmP" runat="server" OnItemCommand="rptNmP_ItemCommand">
        <ItemTemplate>
            <div class="nomainPost col-sm-11 col-md-11 col-lg-11">

                <div class="nmpostall">
                    <div class="nmF">
                        <asp:Label ID="lblNmFloor" runat="server" Text='<%# Eval("Floor")+"F" %>'></asp:Label>
                    </div>
                    <asp:PlaceHolder ID="Nmphl" runat="server" Visible='<%# (string.Compare(Eval("MemberID").ToString(), HttpContext.Current.Session["MemberID"].ToString()) == 0)%>'>
                        <div class="nmbtn">
                            <div>
                                <asp:Button class="btn" ID="btnEditNmPost" runat="server" Text="編輯回覆" CommandName="btnEditNmpost" CommandArgument='<%# Eval("PostID") %>' />
                            </div>
                            <div>
                                <asp:Button class="btn" ID="btnDeleteNmPost" CommandName="btnDeleteNmPost" runat="server" Text="刪除回覆" CommandArgument='<%# Eval("PostID")%>' OnClientClick="dela();" />
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:HiddenField runat="server" ID="hfNmPID" Value='<%# Eval("PostID")%>' />
                    <a href="MemberStastus.aspx?MemberID=<%# Eval("MemberID")%>" title="前往：<%# Eval("MemberID")%>會員頁">
                        <div class="nmM">
                            <asp:Label ID="lblNmMember" runat="server" Text='<%# "作者：" +Eval("MemberAccount")%>'></asp:Label>
                        </div>
                    </a>

                    <div class="nmC">
                        <asp:Label ID="lblNmPostcotent" runat="server" Text='<%# Eval("PostCotent") %>'></asp:Label>
                    <hr />
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="CNNmPost col-sm-11 col-md-11 col-lg-11">
        <asp:TextBox class="CNNmT" runat="server" ID="txtCNNmPost" placeholder="請輸入回覆訊息"></asp:TextBox>
        <asp:Button class="CNNmB" runat="server" ID="btnCNNmPost" OnClick="btnCNNmPost_Click" Text="新增回覆" OnClientClick="NmCreatePosta();"/>
    </div>
    <script>
        $(document).ready(function () {

            var text = $(".content").val();
            var converter = new showdown.Converter();
            var html = converter.makeHtml(text);
            $('.result').html(html);

        });
        
        function dela() { alert('刪除成功') }
        function NmCreatePosta() { alert("回覆成功") }
        
    </script>
</asp:Content>
