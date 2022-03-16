<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CreatePost.aspx.cs" Inherits="MKForum.CreatePost" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table>
            <tr>
                <th>標題 * </th>
                <td>
                    <asp:TextBox runat="server" ID="txtTitle"></asp:TextBox></td>
            </tr>
            <tr>
                <th>類型 </th>
                <td>
                    <asp:DropDownList ID="dpdlPostStamp" runat="server" Width="100px"><asp:ListItem Value="無"></asp:ListItem></asp:DropDownList>
            </tr>   
            <tr>
                <th>封面圖</th>
                <td>
                    <asp:FileUpload ID="fuCoverImage" runat="server" />
                    <asp:Image ID="imgCoverImage" runat="server" />
                </td>
            </tr>
            <tr>
                <th>文內圖片 </th>
                <td>
                    <asp:FileUpload ID="fuPostImage" runat="server" />
                    <asp:Image ID="imgPostImage" runat="server" />
                    <asp:Button ID="btnPostImage" runat="server" Text="確定上傳" onclick="btnPostImage_Click"    />
                </td>
            </tr>
            <tr>
                <th>內文 *</th>
                <td>
                    <asp:TextBox ID="txtPostCotent" runat="server" TextMode="MultiLine" cols="20" Rows="5"></asp:TextBox></td>
            </tr>
            <tr>
                <th>#tag (用/分隔)</th>
                <td>
                    <asp:TextBox ID="txtPostHashtag" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
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
