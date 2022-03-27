<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PboardtoCboard.aspx.cs" Inherits="MKForum.PboardtoCboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/PboardtoCboard.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="Pboard">

        <h1>J-pop</h1>

        <div class="content">
            <asp:Repeater ID="rptpbtocb" runat="server">
                <ItemTemplate>
                    <a href="/CbtoPost.aspx?CboardID=<%# Eval("CboardID") %>">
                    <div class="article">
                        <div class="t">
                            <h6><%# Eval("Cname") %></h6>
                        </div>

                        <div class="p">
                            <p>
                                <%# Eval("CboardCotent") %>
                            </p>
                        </div>
                    </div>
                        </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>


</asp:Content>
