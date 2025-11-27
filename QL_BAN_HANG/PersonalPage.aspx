<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PersonalPage.aspx.cs" Inherits="QL_BAN_HANG.PersonalPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
    /* ================================
   LOGINUSER.ASPX: Giao diện đăng nhập
   ================================ */

#login-form-wrapper {
    margin: 40px auto;
    max-width: 420px;
    background-color: #ffffff;
    padding: 40px;
    border-radius: 12px;
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1);
}

    #login-form-wrapper h2 {
        color: #0d6efd;
        text-align: center;
        margin-bottom: 30px;
        font-size: 26px;
    }

    #login-form-wrapper .input-group {
        margin-bottom: 20px;
    }

        #login-form-wrapper .input-group label {
            display: block;
            margin-bottom: 6px;
            font-weight: 600;
            color: #333;
        }

    #login-form-wrapper .form-control {
        width: 100%;
        padding: 12px;
        border: 1px solid #ccc;
        border-radius: 8px;
        font-size: 15px;
        transition: border-color 0.3s;
    }

        #login-form-wrapper .form-control:focus {
            border-color: #0d6efd;
            outline: none;
        }

    #login-form-wrapper .btn-primary {
        background-color: #0d6efd;
        color: white;
        padding: 12px;
        border: none;
        border-radius: 8px;
        cursor: pointer;
        width: 100%;
        font-size: 17px;
        font-weight: bold;
        transition: background-color 0.3s;
    }

        #login-form-wrapper .btn-primary:hover {
            background-color: #0b5ed7;
        }

    #login-form-wrapper .message {
        text-align: center;
        margin-top: 15px;
        font-weight: bold;
    }

    #login-form-wrapper .link-footer {
        text-align: center;
        margin-top: 25px;
        font-size: 14px;
    }

        #login-form-wrapper .link-footer a {
            color: #0d6efd;
            text-decoration: none;
            font-weight: bold;
        }

            #login-form-wrapper .link-footer a:hover {
                text-decoration: underline;
            }

    #login-form-wrapper .validation-error {
        color: #dc3545;
        font-size: 13px;
        margin-top: 5px;
        display: block;
    }

#Content {
    width: 1000px;
    min-height: 300px;
    height: auto;
    float: left;
    background-color: #ffffff;
}
</style>
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
        <asp:Label AssociatedControlID="txtMatKhauCu" Text="🔑 Mật khẩu cũ (Để trống nếu không đổi)" runat="server" />
        <asp:TextBox ID="txtMatKhauCu" runat="server" TextMode="Password" CssClass="form-control" ReadOnly="True" />
    </div>
    <div class="input-group">
        <asp:Label AssociatedControlID="txtMatKhau" Text="🔒 Mật khẩu mới" runat="server" />
        <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" CssClass="form-control" ReadOnly="True" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtXacNhanMatKhau" Text="🔁 Xác nhận mật khẩu mới" runat="server" />
        <asp:TextBox ID="txtXacNhanMatKhau" runat="server" TextMode="Password" CssClass="form-control" ReadOnly="True" />
    </div>

    <asp:Button ID="btnSuaThongTin" runat="server" Text="✏️ Sửa thông tin cá nhân"
        OnClick="btnSuaThongTin_Click" CssClass="btn-primary" />

    <asp:Button ID="btnLuuThongTin" runat="server" Text="💾 Lưu"
        OnClick="btnLuuThongTin_Click" CssClass="btn-primary" Visible="False" />

    <asp:Button ID="btnHuy" runat="server" Text="❌ Hủy"
        OnClick="btnHuy_Click" CssClass="btn-primary" Visible="False" />

    <asp:Button ID="btnDangXuat" runat="server" Text="🚪 Đăng xuất"
        OnClick="btnDangXuat_Click" CssClass="btn-primary" />

    <div class="message">
        <asp:Label ID="lblMessage" runat="server" CssClass="validation-error" EnableViewState="false" />
    </div>
</div>
</asp:Content>