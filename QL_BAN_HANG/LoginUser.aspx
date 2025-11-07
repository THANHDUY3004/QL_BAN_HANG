<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoginUser.aspx.cs" Inherits="QL_BAN_HANG.LoginUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMainMenu" runat="server">
    <div class="container">
            <h2>Đăng Nhập Tài Khoản</h2>
            
            <div class="input-group">
                <asp:Label Text="Tên đăng nhập (SĐT)" runat="server" />
                <asp:TextBox ID="txtUsernameLog" runat="server" />
            </div>

            <div class="input-group">
                <asp:Label Text="Mật khẩu" runat="server" />
                <asp:TextBox ID="txtPasswordLog" runat="server" TextMode="Password" />
            </div>
            
            <asp:Button ID="btnLogin" runat="server" Text="Đăng Nhập" OnClick="btnLogin_Click" CssClass="btn-primary" />
            
            <div class="message">
                <asp:Label ID="lblMessage" runat="server" CssClass="validation-error"></asp:Label>
            </div>
            
            <div class="link-footer">
                <p>Chưa có tài khoản? <a href="RegisterAccount.aspx">Đăng ký ngay</a></p>
            </div>
        </div>



    <style>
        /* File: Styles/style.css */

body {
    font-family: Arial, sans-serif;
    background-color: #f0f2f5;
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    margin: 0;
}

.container {
    background-color: #fff;
    padding: 30px 40px;
    border-radius: 10px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    width: 100%;
    max-width: 400px; /* Chiều rộng tối đa của Form */
}

h2 {
    color: #1877f2;
    text-align: center;
    margin-bottom: 25px;
    font-size: 24px;
}

.input-group {
    margin-bottom: 20px;
}

.input-group label {
    display: block;
    margin-bottom: 5px;
    font-weight: bold;
    color: #444;
    font-size: 14px;
}

.input-group input[type="text"], 
.input-group input[type="password"],
.input-group textarea { /* Áp dụng cho cả Textarea */
    width: 100%;
    padding: 12px;
    border: 1px solid #ddd;
    border-radius: 6px;
    box-sizing: border-box; 
    font-size: 16px;
    transition: border-color 0.3s;
}

.input-group input:focus, .input-group textarea:focus {
    border-color: #1877f2;
    outline: none;
}

/* Kiểu cho nút ASP:Button */
.btn-primary {
    background-color: #1877f2;
    color: white;
    padding: 12px 20px;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    width: 100%;
    font-size: 18px;
    font-weight: bold;
    transition: background-color 0.3s;
}

.btn-primary:hover {
    background-color: #166fe5;
}

.message {
    text-align: center;
    margin-top: 15px;
    font-weight: bold;
}

.link-footer {
    text-align: center;
    margin-top: 20px;
    padding-top: 15px;
    border-top: 1px solid #eee;
}

.link-footer a {
    color: #1877f2;
    text-decoration: none;
    font-weight: bold;
}

/* CSS cho ASP:Label báo lỗi */
.validation-error {
    color: #e53935;
    font-size: 13px;
    margin-top: 5px;
    display: block;
}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="server">
</asp:Content>
