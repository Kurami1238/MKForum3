<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/MemberPage.Master" AutoEventWireup="true" CodeBehind="MemberRegisterList.aspx.cs" Inherits="MKForum.BackAdmin.MemberRegisterList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Button ID="btnCreate" runat="server" Text="新增" OnClick="btnCreate_Click" />
    <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClick="btnDelete_Click" /><br />

    <asp:TextBox runat="server" ID="txtKeyword" placeholder="請輸入搜尋文字"></asp:TextBox>
    <asp:Button runat="server" ID="btnSearch" Text="搜尋" OnClick="btnSearch_Click" />
    <br />

    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField>
                 <ItemTemplate>
                     <asp:CheckBox runat="server" ID="ckbDel" />
                     <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("MemberID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MemberID" HeaderText="代碼" />
            <asp:BoundField DataField="Account" HeaderText="帳號" />
            <%--<asp:BoundField DataField="CreateDate" HeaderText="建立日期" />--%>
            <asp:TemplateField HeaderText="管理">
                <ItemTemplate>
                    <a href="MemberRegisterPage.aspx?ID=<%# Eval("MemberID") %>">編輯</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:PlaceHolder runat="server" ID="plcEmpty" Visible="false">
        <p>未有資料</p>
    </asp:PlaceHolder>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
