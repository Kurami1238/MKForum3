<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EditPost.aspx.cs" Inherits="MKForum.EditPost1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/EditPost.css" rel="stylesheet" />
    <script src="js/showdown.js"></script>
    <%--<script src="http://code.jquery.com/jquery-latest.js"></script>--%>
    <script src="js/jquery.min.js"></script>
    <link href="css/github-markdown.css" rel="stylesheet" />
    <link href="css/github-markdown-dark.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
            <input type="hidden" id="memberid" class="memberid" runat="server"/>
            <input type="hidden" id="msgmsg" class="msgmsg" runat="server"/>
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
            
            <tr class="img">
                <th>文內圖片 </th>
                <td>
                    <asp:FileUpload class="button upup" ID="fuPostImage" runat="server" />
                    <asp:Image ID="imgPostImage" runat="server" />
                    <asp:Button  class="button kakutei" ID="btnPostImage" runat="server" Text="確定上傳" OnClick="btnPostImage_Click" OnClientClick="Getlink();" />
                </td>
            </tr>
            <tr class="C">
                <th>內文 *</th>
                <td>
                    <button class="dougu" type="button" onclick="big();">大</button>
                    <button class="dougu" type="button" onclick="mid();">中</button>
                    <button class="dougu" type="button" onclick="sml();">小</button>
                    <button class="dougu" type="button" onclick="narabi();">項目符號</button>
                    <textarea class="content" id="content" rows="6" cols="28" runat="server"></textarea>
                </td>
            </tr>
            <tr class="tag">
                <th>#tag (用/分隔)</th>
                <td>
                    <asp:TextBox ID="txtPostHashtag" runat="server"></asp:TextBox></td>
            </tr>
            <tr class="Cv">
                <th>封面圖</th>
                <td>
                    <asp:FileUpload class="button" ID="fuCoverImage" runat="server" />
                    <asp:Image ID="imgCoverImage" runat="server" />
                </td>
            </tr>
            <tr class="msg">
                <td colspan="2">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
       
    </div>
    <div>
        <table class="kobi col-sm-11 col-md-11 col-lg-11">
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
            <tr>
                <td colspan="2"></td>
            </tr>
        </table>
    </div>
    <div class="col-sm-8 col-md-11 col-lg-11">
     <asp:Button ID="btnSend" CssClass="cbtn" runat="server" Text="送出" OnClick="btnSend_Click" />
        <asp:Button ID="btnback" CssClass="cbtn" runat="server" Text="返回" OnClick="btnback_Click"/>
        </div>
    <script>
        $(document).ready(function () {
            var msg = $(".msgmsg").val();
            console.log(msg);
            if (msg != undefined && msg != null && msg != "")
                alert(msg);
            var text = $(".content").val();
            var converter = new showdown.Converter({ 'tables': 'true', 'tasklists': 'true', 'simpleLineBreaks': 'true', 'openLinksInNewWindow': 'true', 'simplifiedAutoLink': 'true', 'strikethrough': 'true', 'customizedHeaderId': 'true', 'emoji': 'true', 'moreStyling': 'true', 'smoothLivePreview': 'true', 'smartIndentationFix': 'true', 'ghMentions': 'true', 'omitExtraWLInCodeBlocks': 'true' });
            var html = converter.makeHtml(text);
            $('.result').html(html);
        });
        $('.content').on('keyup', function () {
            var text = $(".content").val();
            var converter = new showdown.Converter();
            var html = converter.makeHtml(text);
            $('.result').html(html);
        });
        var text;
        text = $(".content").val();
        function big() {
            text = $(".content").val();
            text += "#";
            $(".zenbu .C .content").val(text);
        }
        function mid() {
            text = $(".content").val();
            text += "###";
            $(".zenbu .C .content").val(text);
        }
        function sml() {
            text = $(".content").val();
            text += "#####";
            $(".zenbu .C .content").val(text);
        }
        function narabi() {
            text = $(".content").val();
            text += "+ a\n+ a\n+ a";
            $(".zenbu .C .content").val(text);
        }
        function Getlink() {
            var upup = $(".upup").val();
            var mid = $(".memberid").val();
            var text = $(".zenbu .C .content").val();
            console.log(text);

            var postData = {
                "MemberID": mid,
            }
            if (upup != null) {
                $.ajax({
                    url: "/API/GetLinkHandler.ashx?Action=Link",
                    method: "POST",
                    data: postData,
                    dataType: "json",
                    success: function (henzi) {
                        console.log(henzi);
                        console.log(text);
                        text += "![這是一個格式]" + (henzi);
                        console.log(text);

                        $(".zenbu .C .content").text = text;
                    },
                    error: function (henzi) {
                        console.log(henzi);
                        alert("通訊失敗，請聯絡管理員。")
                    }
                });
            }

        }
    </script>
</asp:Content>
