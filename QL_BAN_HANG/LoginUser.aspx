<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoginUser.aspx.cs" Inherits="QL_BAN_HANG.LoginUser" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="layout/loginuser.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div id="login-form-wrapper" class="container">
    <h2><i class="fas fa-sign-in-alt"></i> Đăng Nhập Tài Khoản</h2>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtUsernameLog" Text="📱 Số điện thoại" runat="server" />
        <asp:TextBox ID="txtUsernameLog" runat="server" CssClass="form-control" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtPasswordLog" Text="🔒 Mật khẩu" runat="server" />
        <asp:TextBox ID="txtPasswordLog" runat="server" TextMode="Password" CssClass="form-control" />
    </div>

    <asp:Button ID="btnLogin" runat="server" Text="🚪 Đăng Nhập" OnClick="btnLogin_Click" CssClass="btn-primary" />

    <div class="message">
        <asp:Label ID="lblMessage" runat="server" CssClass="validation-error"></asp:Label>
    </div>

    <div class="link-footer">
        <p>Chưa có tài khoản? <a href="RegisterUser.aspx">Đăng ký ngay</a></p>
    </div>
</div>
</asp:Content>
