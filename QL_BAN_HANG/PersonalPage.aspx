<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PersonalPage.aspx.cs" Inherits="QL_BAN_HANG.PersonalPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/loginuser.css" rel="stylesheet" />
    <link href="layout/registeruser.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div id="login-form-wrapper" class="container">
    <h2><i class="fas fa-user-edit"></i> Thông Tin Cá Nhân</h2>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtHoTen" Text="👤 Họ và tên" runat="server" />
        <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control" ReadOnly="True" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtSoDienThoai" Text="📱 Số điện thoại (Tên đăng nhập)" runat="server" />
        <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-control" ReadOnly="True" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtDiaChi" Text="🏠 Địa chỉ (Tùy chọn)" runat="server" />
        <asp:TextBox ID="txtDiaChi" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" ReadOnly="True" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtMatKhau" Text="🔒 Mật khẩu" runat="server" />
        <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" CssClass="form-control" ReadOnly="True" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtXacNhanMatKhau" Text="🔁 Xác nhận mật khẩu" runat="server" />
        <asp:TextBox ID="txtXacNhanMatKhau" runat="server" TextMode="Password" CssClass="form-control" ReadOnly="True" />
    </div>

    <!-- Nút sửa -->
    <asp:Button ID="btnSuaThongTin" runat="server" Text="✏️ Sửa thông tin cá nhân"
        OnClick="btnSuaThongTin_Click" CssClass="btn-primary" />

    <!-- Nút lưu và hủy (ẩn ban đầu) -->
    <asp:Button ID="btnLuuThongTin" runat="server" Text="💾 Lưu"
        OnClick="btnLuuThongTin_Click" CssClass="btn-primary" Visible="False" />

    <asp:Button ID="btnHuy" runat="server" Text="❌ Hủy"
        OnClick="btnHuy_Click" CssClass="btn-primary" Visible="False" />

    <!-- Nút đăng xuất -->
    <asp:Button ID="btnDangXuat" runat="server" Text="🚪 Đăng xuất"
        OnClick="btnDangXuat_Click" CssClass="btn-primary" />

    <div class="message">
        <asp:Label ID="lblMessage" runat="server" CssClass="validation-error" EnableViewState="false" />
    </div>
</div>

</asp:Content>
