<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CbtoPost.aspx.cs" Inherits="MKForum.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table border="1">
        <asp:Repeater ID="rptcBtoP" runat="server">
            <ItemTemplate>
                <div class="post">
                    <div class="postP"></div>
                    <div class="postTC">
                        <asp:Literal ID="ltlPostT" runat="server" Text='<%# Eval("Title")%>'></asp:Literal>
                        <asp:Literal ID="ltlPostC" runat="server" Text='<%# Eval("PostCotent")%>'></asp:Literal>
                    </div>
                    <div class="postB">
                        <asp:Button ID="btnPostEdit" runat="server" OnClick="btnPostEdit_Click" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
