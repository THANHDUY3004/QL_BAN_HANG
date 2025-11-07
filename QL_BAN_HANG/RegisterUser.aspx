<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RegisterUser.aspx.cs" Inherits="QL_BAN_HANG.RegisterUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/registeruser.css" rel="stylesheet" />
    <link href="layout/loginuser.css" rel="stylesheet" />
    </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div id="login-form-wrapper" class="container">
        <h2><i class="fas fa-user-plus"></i> Tạo Tài Khoản Khách Hàng</h2>

        <div class="input-group">
            <asp:Label AssociatedControlID="txtHoTen" Text="👤 Họ và tên" runat="server" />
            <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="ReqHoTen" runat="server"
                ControlToValidate="txtHoTen"
                ErrorMessage="* Vui lòng nhập họ tên."
                CssClass="validation-error" Display="Dynamic" />
        </div>

        <div class="input-group">
            <asp:Label AssociatedControlID="txtSoDienThoai" Text="📱 Số điện thoại (Tên đăng nhập)" runat="server" />
            <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="ReqSDT" runat="server"
                ControlToValidate="txtSoDienThoai"
                ErrorMessage="* Vui lòng nhập số điện thoại."
                CssClass="validation-error" Display="Dynamic" />
        </div>

        <div class="input-group">
            <asp:Label AssociatedControlID="txtDiaChi" Text="🏠 Địa chỉ (Tùy chọn)" runat="server" />
            <asp:TextBox ID="txtDiaChi" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
        </div>

        <div class="input-group">
            <asp:Label AssociatedControlID="txtMatKhau" Text="🔒 Mật khẩu" runat="server" />
            <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="ReqMatKhau" runat="server"
                ControlToValidate="txtMatKhau"
                ErrorMessage="* Vui lòng nhập mật khẩu."
                CssClass="validation-error" Display="Dynamic" />
        </div>

        <div class="input-group">
            <asp:Label AssociatedControlID="txtXacNhanMatKhau" Text="🔁 Xác nhận mật khẩu" runat="server" />
            <asp:TextBox ID="txtXacNhanMatKhau" runat="server" TextMode="Password" CssClass="form-control" />
            <asp:CompareValidator ID="CompareMatKhau" runat="server"
                ControlToValidate="txtXacNhanMatKhau"
                ControlToCompare="txtMatKhau"
                Operator="Equal" Type="String"
                ErrorMessage="* Mật khẩu không khớp."
                CssClass="validation-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnDangKy" runat="server" Text="📝 Đăng Ký Tài Khoản"
            OnClick="btnDangKy_Click" CssClass="btn-primary" />

        <div class="message">
            <asp:Label ID="lblMessage" runat="server" CssClass="validation-error" EnableViewState="false" />
        </div>

        <div class="link-footer">
            <p>Đã có tài khoản? <a href="LoginUser.aspx">Đăng nhập ngay</a></p>
        </div>
    </div>
</asp:Content>