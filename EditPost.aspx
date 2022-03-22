<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EditPost.aspx.cs" Inherits="MKForum.EditPost1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/EditPost.css" rel="stylesheet" />
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
                    <asp:DropDownList ID="dpdlPostStamp" runat="server" Width="100px"><asp:ListItem Value="無"></asp:ListItem></asp:DropDownList>
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
                    <asp:Button class="button" ID="btnPostImage" runat="server" Text="確定上傳" onclick="btnPostImage_Click"    />
                </td>
            </tr>
            <tr class="C">
                <th>內文 *</th>
                <td>
                    <asp:TextBox ID="txtPostCotent" runat="server" TextMode="MultiLine" cols="20" Rows="5"></asp:TextBox></td>
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
        <asp:Button ID="btnSend" runat="server" Text="送出" OnClick="btnSend_Click" OnClientClick="Createa();"/>
    </div>
     <script>
         function Createa() { alert('新增文章成功') }
     </script>
</asp:Content>
