<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RegisterUser.aspx.cs" Inherits="QL_BAN_HANG.RegisterUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/registeruser.css" rel="stylesheet" />
    </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
        <div class="register-box">
            <h2>Tạo Tài Khoản Khách Hàng</h2>

            <div class="input-group">
                <label for="<%= txtHoTen.ClientID %>">Họ và tên</label>
                <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="ReqHoTen" runat="server"
                    ControlToValidate="txtHoTen"
                    ErrorMessage="* Vui lòng nhập họ tên."
                    CssClass="validation-error" Display="Dynamic" />
            </div>

            <div class="input-group">
                <label for="<%= txtSoDienThoai.ClientID %>">Số điện thoại (Tên đăng nhập)</label>
                <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="ReqSDT" runat="server"
                    ControlToValidate="txtSoDienThoai"
                    ErrorMessage="* Vui lòng nhập số điện thoại."
                    CssClass="validation-error" Display="Dynamic" />
            </div>

            <div class="input-group">
                <label for="<%= txtDiaChi.ClientID %>">Địa chỉ (Tùy chọn)</label>
                <asp:TextBox ID="txtDiaChi" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
            </div>

            <div class="input-group">
                <label for="<%= txtMatKhau.ClientID %>">Mật khẩu</label>
                <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="ReqMatKhau" runat="server"
                    ControlToValidate="txtMatKhau"
                    ErrorMessage="* Vui lòng nhập mật khẩu."
                    CssClass="validation-error" Display="Dynamic" />
            </div>

            <div class="input-group">
                <label for="<%= txtXacNhanMatKhau.ClientID %>">Xác nhận mật khẩu</label>
                <asp:TextBox ID="txtXacNhanMatKhau" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:CompareValidator ID="CompareMatKhau" runat="server"
                    ControlToValidate="txtXacNhanMatKhau"
                    ControlToCompare="txtMatKhau"
                    Operator="Equal" Type="String"
                    ErrorMessage="* Mật khẩu không khớp."
                    CssClass="validation-error" Display="Dynamic" />
            </div>

            <asp:Button ID="btnDangKy" runat="server" Text="Đăng Ký Tài Khoản"
                OnClick="btnDangKy_Click" CssClass="btn-register" />

            <div class="message-label">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
            </div>

            <div class="login-link">
                Đã có tài khoản? <a href="Login.aspx">Đăng nhập ngay</a>
            </div>
        </div>
</asp:Content>