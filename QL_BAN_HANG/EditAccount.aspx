<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="EditAccount.aspx.cs" Inherits="QL_BAN_HANG.EditAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/loginuser.css" rel="stylesheet" />
    <link href="layout/registeruser.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMainMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div id="login-form-wrapper" class="container">
    <h2><i class="fas fa-user-edit"></i> Thông Tin Cá Nhân</h2>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtHoTen" Text="👤 Họ và tên" runat="server" />
        <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control" />
    </div>

    <div class="input-group">
    <asp:Label AssociatedControlID="txtSoDienThoai" Text="📱 Số điện thoại (Số điện thoại)" runat="server" />
    <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-control" ReadOnly="True" />
</div>


    <div class="input-group">
        <asp:Label AssociatedControlID="txtDiaChi" Text="🏠 Địa chỉ (Tùy chọn)" runat="server" />
        <asp:TextBox ID="txtDiaChi" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtMatKhau" Text="🔒 Mật khẩu" runat="server" />
        <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" CssClass="form-control" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtXacNhanMatKhau" Text="🔁 Xác nhận mật khẩu" runat="server" />
        <asp:TextBox ID="txtXacNhanMatKhau" runat="server" TextMode="Password" CssClass="form-control" />
    </div>

    <!-- Nút lưu và hủy (hiển thị ngay khi load) -->
    <asp:Button ID="btnLuuThongTin" runat="server" Text="💾 Lưu"
        OnClick="BtnLuuThongTin_Click" CssClass="btn-primary" />

    <asp:Button ID="btnHuy" runat="server" Text="❌ Hủy"
        OnClick="BtnHuy_Click" CssClass="btn-primary" />

    <!-- Nút quay lại thay cho đăng xuất -->
    <asp:Button ID="btnQuayLai" runat="server" Text="↩️ Quay Lại"
        OnClick="BtnQuayLai_Click" CssClass="btn-primary" />

    <div class="message">
        <asp:Label ID="lblMessage" runat="server" CssClass="validation-error" EnableViewState="false" />
    </div>
</div>

</asp:Content>
