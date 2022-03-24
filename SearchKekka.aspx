<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SearchKekka.aspx.cs" Inherits="MKForum.SearchKekka" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="content col-sm-11 col-md-11 col-lg-11" id="PostHazimari">
            <input type="hidden" id="sortid" class="sortid" runat="server"/>
            <input type="hidden" id="hftest" class="hftest" runat="server"/>
            <asp:Repeater ID="rptcBtoP" runat="server">
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
</asp:Content>
