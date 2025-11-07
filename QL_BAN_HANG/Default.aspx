<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QL_BAN_HANG.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMainMenu" Runat="Server">
    
    <div class="nav-toggle" onclick="toggleNav()">☰ Menu</div>
    <ul id="nav">
        <li><a href="Default.aspx">Giới Thiệu</a></li>
        <li><a href="#">Danh Mục Sản Phẩm &raquo;</a>
            <ul>
                <li><a href="#">Trà Sữa</a></li>
                <li><a href="#">Trà Trái Cây</a></li>
                <li><a href="#">Bánh Ngọt</a></li>
                <li><a href="#">Tất cả sản phẩm</a></li>
            </ul>
        </li>
        <li id="nav-right"><a href="LoginUser.aspx">Đăng nhập</a></li>
        <li id="nav-right-2"><a href="#">Tài Khoản</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" Runat="Server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderBottom" Runat="Server">
    Nội dung hiển thị tại Bottom
</asp:Content>