<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DisplayPost.aspx.cs" Inherits="MKForum.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Display.css" rel="stylesheet" />
    <script src="js/showdown.js"></script>
    <link href="css/github-markdown-dark.css" rel="stylesheet" />
    <link href="css/github-markdown.css" rel="stylesheet" />
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <style>
        .modal-content > *{
            background-color:rgb(30, 30, 30);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="MainPost col-sm-11 col-md-11 col-lg-11">
            <input type="hidden" id="msgmsg" class="msgmsg" runat="server"/>
        <div class="mpostall">
            <div class="PostT">
                <asp:Label ID="lblTitle" runat="server" Text="標題"></asp:Label>
            </div>
            <div class="PostF">
                <asp:Label ID="lblFloor" runat="server" Text="樓層數"></asp:Label>
            </div>
            <div class="Postbtn">
                <div>
                    <asp:Button class="btn" ID="btnMemberFollow_FollowStatus" runat="server" Text="追蹤" OnClick="btnMemberFollow_FollowStatus_Click" />
                </div>
                <asp:PlaceHolder ID="phl" runat="server">
                    <asp:HiddenField runat="server" ID="hfMemberID" />
                    <div>
                        <asp:Button class="btn" ID="btnEditPost" runat="server" Text="編輯文章" OnClick="btnEditPost_Click" />
                    </div>
                    <div>
                        <button type="button" class="btn" data-bs-toggle="modal" data-bs-target="#hondouni">
                            刪除文章
                        </button>
                        <%--<asp:Button class="btn" ID="btnDeletePost" runat="server" Text="刪除文章" OnClick="btnDeletePost_Click" OnClientClick="dela();" />--%>
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="PostM">
            <input type="hidden" id="sakusyaacc" class="sortid" runat="server"/>
                <a href='<%= "SearchKekka.aspx?keyword=" + this.sakusyaacc.Value + "&searcharea=srchWriter" %>' title="搜尋此：<%= this.sakusyaacc.Value%> 作者">
                <asp:Label ID="lblMember" runat="server" Text=""></asp:Label>
                    </a>
            </div>
            <div class="PostC">
                <div class="result" id="result">
                </div>
                <input type="hidden" id="sortid" class="content" runat="server" />
                <%--<asp:Label ID="lblCotent" runat="server" Text="內文" ></asp:Label>--%>
                <hr />
            </div>
        </div>
        <asp:Label CssClass="btnl" ID="lblMemberFollow_FollowStatus" runat="server"></asp:Label>
        <div class="allpht">
            <asp:Repeater ID="rptpht" runat="server">
                <ItemTemplate>
                    <a href='<%# "SearchKekka.aspx?keyword=" + Eval("Naiyo") + "&searcharea=srchTag" %>'>
                        <div class="pht">
                            <asp:Label runat="server" Text='<%# "#"+Eval("Naiyo") %>'></asp:Label>
                        </div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <asp:Repeater ID="rptNmP" runat="server" OnItemCommand="rptNmP_ItemCommand">
        <ItemTemplate>
            <div class="nomainPost col-sm-11 col-md-11 col-lg-11">
                <div class="nmpostall">
                    <div class="nmF">
                        
                            <asp:Label ID="lblNmFloor" runat="server" Text='<%# Eval("Floor")+"F" %>'></asp:Label>
                    </div>
                    <asp:PlaceHolder ID="Nmphl" runat="server" Visible='<%# (HttpContext.Current.Session["MemberID"] != null) ? (string.Compare(Eval("MemberID").ToString(), HttpContext.Current.Session["MemberID"].ToString()) == 0) : false%>'>
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
                    <a href='<%# "SearchKekka.aspx?keyword=" + Eval("MemberAccount") + "&searcharea=srchWriter" %>' title="搜尋此：<%# Eval("MemberID")%> 作者">
                        <div class="nmM">
                            <asp:Label ID="lblNmMember" runat="server" Text='<%# "作者：" +Eval("MemberAccount")%>'></asp:Label>
                        </div>
                    </a>

                    <div class="nmC">
                        <a name='<%# Eval("Floor") %>'>
                        <asp:Label ID="lblNmPostcotent" runat="server" Text='<%# Eval("PostCotent") %>'></asp:Label>
                        <hr />
                            </a>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <div class="CNNmPost col-sm-11 col-md-11 col-lg-11">
        <asp:TextBox class="CNNmT" runat="server" ID="txtCNNmPost" placeholder="請輸入回覆訊息"></asp:TextBox>
        <asp:Button class="CNNmB" runat="server" ID="btnCNNmPost" OnClick="btnCNNmPost_Click" Text="新增回覆" />
    </div>
    
    <div class="modal fade" id="hondouni" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" aria-modal="true" role="dialog" >
        <div class="modal-dialog" style="background-color:black;"> 
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <asp:Literal runat="server" ID="ltlModalTitle">你</asp:Literal>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal runat="server" ID="ltlModalContent">確定要刪除！？</asp:Literal>
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn" data-bs-dismiss="modal">關閉</button>
                    <button type="button" class="btn" data-bs-toggle="modal" data-bs-target="#hondouhondouni">
                            刪除文章
                        </button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="hondouhondouni" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" aria-modal="true" role="dialog" >
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <asp:Literal runat="server" ID="Literal1">你</asp:Literal>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal runat="server" ID="Literal2">真的真的要刪除ㄇ！？</asp:Literal>
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:button runat="server" ID="modalback" class="btn" type="button" data-bs-dismiss="modal" aria-label="Close" OnClick="modalback_Click" Text="我後悔了"></asp:button>
                    <asp:Button class="btn" ID="Button1" runat="server" Text="刪除文章" OnClick="btnDeletePost_Click"  />
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {

            var text = $(".content").val();
            var converter = new showdown.Converter();
            var html = converter.makeHtml(text);
            $('.result').html(html);

            var msg = $(".msgmsg").val();
            console.log(msg);
            if (msg != undefined && msg != null && msg != "")
                alert(msg);
        });


    </script>
</asp:Content>
