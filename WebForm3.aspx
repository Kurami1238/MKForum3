<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="MKForum.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblCboardName" runat="server" Text="Label"></asp:Label>
    <br /><br />
    新增文章類型：
    <asp:TextBox ID="txtCboardtype" runat="server"></asp:TextBox>
    <br /><br />&nbsp &nbsp &nbsp
    <asp:Button ID="btnSave" runat="server" Text="確定" onclick="btnSave_Click"/>
    &nbsp &nbsp &nbsp
    <asp:Button ID="btncCancel" runat="server" Text="取消" onclick="btncCancel_Click"/>
    <br /><br />
    <asp:Label ID="lblmessage" runat="server" Text="Label"></asp:Label>
    
</asp:Content>
