<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CreatePost.aspx.cs" Inherits="MKForum.CreatePost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/CreatePost.css" rel="stylesheet" />
    <script src="js/showdown.js"></script>
    <script src="js/jquery.min.js"></script>
    <link href="css/github-markdown.css" rel="stylesheet" />
    <link href="css/github-markdown-dark.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="zenbu col-sm-11 col-md-11 col-lg-11">
            <tr class="T">
                <th>標題 * </th>
                <td>
                    <asp:TextBox runat="server" ID="txtTitle"></asp:TextBox></td>
            </tr>
            <tr class="S">
                <th>類型 </th>
                <td>
                    <asp:DropDownList ID="dpdlPostStamp" runat="server" Width="100px">
                        <asp:ListItem Value="無"></asp:ListItem>
                    </asp:DropDownList>
            </tr>
            <tr class="Cv">
                <th>封面圖</th>
                <td>
                    <asp:FileUpload class="button" ID="fuCoverImage" runat="server" />
                    <asp:Image ID="imgCoverImage" runat="server" />
                </td>
            </tr>
            <tr class="img">
                <th>文內圖片 </th>
                <td>
                    <asp:FileUpload class="button" ID="fuPostImage" runat="server" />
                    <asp:Image ID="imgPostImage" runat="server" />
                    <asp:Button class="button" ID="btnPostImage" runat="server" Text="確定上傳" OnClick="btnPostImage_Click" />
                </td>
            </tr>
            <tr class="C">
                <th>內文 *</th>
                <td>
                    <textarea class="content" id="content" rows="6" cols="20" runat="server"></textarea>
                </td>
            </tr>
            <tr class="tag">
                <th>#tag (用/分隔)</th>
                <td>
                    <asp:TextBox ID="txtPostHashtag" runat="server"></asp:TextBox></td>
            </tr>
            <tr class="msg">
                <td colspan="2">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnSend" CssClass="cbtn" runat="server" Text="送出" OnClick="btnSend_Click" OnClientClick="Createa();" />
        <asp:Button ID="btnback" CssClass="cbtn" runat="server" Text="返回" OnClick="btnback_Click"    />
    </div>
    <div>
        <table class="kobi col-sm-8 col-md-11 col-lg-11">
            <tr class="T">
                <th>預覽 </th>
                <td>
                    <div class="result" id="result">
                       
                    </div>
                         <%--<asp:TextBox class="result2" ID="result2" runat="server" TextMode="MultiLine" ></asp:TextBox>--%>
                         <%--ValidateRequestMode="Disabled"--%>
                        <%--這裡會學習hackMD的XSS防禦函數庫 npm/XSS 以及開啟CSP來解決 XSS問題--%>
                </td>
            </tr>
        </table>
    </div>
    <script>
        $('.content').on('keyup', function () {
            var text = $(".content").val();
            var converter = new showdown.Converter();
            var html = converter.makeHtml(text);
            $('.result').html(html);
            $('.result2').html(html);
        });
        function Createa() { alert('新增文章成功') }
    </script>
</asp:Content>
