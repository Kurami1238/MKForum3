<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CbtoPost.aspx.cs" Inherits="MKForum.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/CboardsPage.css" />
    <link href="css/bootstrap.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="Cboard">
        <h1>
            <asp:Literal runat="server" ID="ltlCbn"></asp:Literal></h1>
        <div class="StampButton">
            <asp:Repeater ID="rptStamp" runat="server" OnItemCommand="rptStamp_ItemCommand">
                <ItemTemplate>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="content col-sm-11 col-md-11 col-lg-11" id="PostHazimari">
            <input type="hidden" id="sortid" class="sortid" runat="server"/>
            <input type="hidden" id="hftest" class="hftest" runat="server"/>
            <asp:Repeater ID="rptcBtoP" runat="server" OnItemCommand="rptcBtoP_ItemCommand">
                <ItemTemplate>
                    <div class="test">
                        <a class="PostA" href="DisplayPost.aspx?CboardID=<%# Eval("CboardID")%>&PostID=<%# Eval("PostID")%>" title="前往：<%# Eval("Title")%>">
                            <div class="article" runat="server">
                                <asp:PlaceHolder runat="server" Visible='<%# !string.IsNullOrWhiteSpace(Eval("CoverImage") as string)%>'>
                                    <img id="imgPostP" class="imgPostP" src="<%# Eval("CoverImage") as string%>" width="300" height="300" />
                                </asp:PlaceHolder>
                                <h6 class="PostT">
                                    <asp:Literal ID="ltlPostT" runat="server" Text='<%# Eval("Title")%>'></asp:Literal>
                                </h6>
                                <p class="PostC">
                                    <asp:Literal ID="ltlPostC" runat="server" Text='<%# Eval("PostCotent")%>'></asp:Literal>
                                </p>
                                <h3 class="PostM">
                                    <asp:Literal ID="ltlPostM" runat="server" Text='<%# "作者： " + Eval("MemberAccount") %>'></asp:Literal>
                                </h3>
                                <h4 class="PostD">
                                    <asp:Literal ID="ltlPostD" runat="server" Text='<%# (Eval("LastEditTime") != null)? "最後編輯： " + Eval("LastEditTime") : Eval("PostDate") %>'></asp:Literal>
                                </h4>
                                <input type="hidden" id="hf" name="hfcbid" class="hfcbid" value="<%# Eval("CboardID")%>" />
                                <%--    <asp:PlaceHolder ID="Nmphl" runat="server" Visible='<%# (string.Compare(Eval("MemberID").ToString(), HttpContext.Current.Session["MemberID"].ToString()) == 0)%>'>
                                <asp:Button ID="btnPostEdit" runat="server" Text="編輯" CommandName="btnEditNmpost" CommandArgument='<%# Eval("PostID") %>' />
                            </asp:PlaceHolder>--%>
                                <h5>
                                    <br />
                                </h5>
                            </div>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <asp:ImageButton class="btncreatep" ID="btnCreatePostB" runat="server" Text="新增文章" OnClick="btnCreatePost_Click" ImageUrl="css/pen.png" Height="50px" Width="50px" />
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
    <script>
        $("#StampButton").on("click", "#Sbtn", function () {
            pageIndex = 1;
        });
        var pageIndex = 1;
        var pageCount;
        var hf = $(".hftest").val();
        var sort = $(".sortid").val();
        console.log(hf)
        //------------------------------
        $(window).scroll(function () {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                GetRecords();
            }
        });
        //------------------------------
        function GetRecords() {
            pageIndex++;
            if (pageIndex == 2 || pageIndex <= pageCount) {
                //$("#loader").show();

                console.log(hf);
                var postData = {
                    "PageIndex": pageIndex,
                    "PageSize": 5,
                    "CboardID": hf,
                    "SortID": sort,
                }
                $.ajax({
                    url: "/API/MugenHandler.ashx?Action=Mugen",
                    method: "POST",
                    data: postData,
                    //contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: Mugen,
                    error: function (henzi) {
                        console.log(henzi);
                        alert("通訊失敗，請聯絡管理員。")
                    }
                });
            }
        }
        function Mugen(henzi) {
            console.log(henzi);
            var count = henzi.PageCount;
            pageCount = count;
            var list = henzi.SourceList;
            console.log(list);
            if (pageIndex <= pageCount) {
                for (var i = 0; i < list.length; i++) {
                    var rpt = $("#PostHazimari .test").eq(0).clone(true);
                    //console.log($("#PostA").attr("href"));
                    console.log(rpt.html());
                    var url = "DisplayPost.aspx?CboardID=" + list[i].CboardID + "&PostID=" + list[i].PostID;
                    var titlex = "前往：" + list[i].Title;
                    var postdd = "最後編輯：" + list[i].LastEditTime
                    $(".PostA", rpt).attr({ "href": url, "title": titlex });
                    $(".imgPostP", rpt).attr({ "src": list[i].Coverimage });
                    $(".PostT", rpt).text(list[i].Title);
                    $(".PostC", rpt).text(list[i].PostCotent);
                    $(".PostM", rpt).text(list[i].MemberAccount);
                    $(".PostD", rpt).text(postdd);
                    //$("btnPostEdit").attr({ "CommandArgument": list[i].PostID });
                    console.log(rpt.html());
                    //$("#Nmphl").text(list[i].LastEditTime);
                    $(".content").append(rpt);
                }
            }
        };
        //$("#loader").hide();


        // bubble up    
        // 上層容器   事件、找到什麼對象、觸發的內容
        // $(#).on('    ','          ',function(){})
    </script>
</asp:Content>
