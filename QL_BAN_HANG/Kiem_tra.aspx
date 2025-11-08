<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kiem_tra.aspx.cs" Inherits="QL_BAN_HANG.Kiem_tra" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
        <h2><i class="fas fa-user"></i> Thông Tin Tài Khoản</h2>
            <asp:Button ID="LogoutUser" runat="server" Text="Đăng xuất tài khoản" OnClick="LogoutUser_Click" />
            <br />
            <table style="width:100%;">
                <tr>
                    <td>
            <asp:Label AssociatedControlID="txtHoTen" Text="👤 Họ và tên" runat="server" />
                    </td>
                    <td>
            <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
            <asp:Label AssociatedControlID="txtSDT" Text="📱 Số điện thoại" runat="server" />
                    </td>
                    <td>
            <asp:TextBox ID="txtSDT" runat="server" CssClass="form-control" ReadOnly="true" OnTextChanged="txtSDT_TextChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
            <asp:Label AssociatedControlID="txtDiaChi" Text="🏠 Địa chỉ" runat="server" />
                    </td>
                    <td>
            <asp:TextBox ID="txtDiaChi" runat="server" CssClass="form-control" ReadOnly="true" />
                    </td>
                </tr>
                    <tr>
                    <td>

        <asp:Label ID="lblMessage" runat="server" CssClass="validation-error" />
                    </td>
                    <td>
            <asp:Button ID="btnSua" runat="server" Text="✏️ Sửa" CssClass="btn-secondary" OnClick="btnSua_Click" />
            <asp:Button ID="btnLuu" runat="server" Text="💾 Lưu" CssClass="btn-primary" OnClick="btnLuu_Click" Visible="false" />
            <asp:Button ID="btnHuy" runat="server" Text="❌ Hủy" CssClass="btn-danger" OnClick="btnHuy_Click" Visible="false" />

                    </td>
                </tr>
            </table>
    </div>
    </form>
</body>
</html>
