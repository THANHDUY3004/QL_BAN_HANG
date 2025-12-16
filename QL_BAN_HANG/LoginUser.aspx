<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoginUser.aspx.cs" Inherits="QL_BAN_HANG.LoginUser" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
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
    <h2><i class="fas fa-sign-in-alt"></i> Đăng Nhập Tài Khoản</h2>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtUsernameLog" Text="📱 Số điện thoại" runat="server" />
        <asp:TextBox ID="txtUsernameLog" runat="server" CssClass="form-control" />
    </div>

    <div class="input-group">
        <asp:Label AssociatedControlID="txtPasswordLog" Text="🔒 Mật khẩu" runat="server" />
        <asp:TextBox ID="txtPasswordLog" runat="server" TextMode="Password" CssClass="form-control" />
    </div>



    <asp:Button ID="btnLogin" runat="server" Text="🚪 Đăng Nhập" OnClick="BtnLogin_Click" CssClass="btn-primary" />

    <div class="message">
        <asp:Label ID="lblMessage" runat="server" CssClass="validation-error"></asp:Label>
    </div>
    <asp:HiddenField ID="HiddenFieldIsLocked" runat="server" Value="false" />
    <asp:HiddenField ID="HiddenFieldRemainingTime" runat="server" Value="0" />
    <div class="link-footer">
        <p>Chưa có tài khoản? <a href="RegisterUser.aspx">Đăng ký ngay</a></p>
    </div>
</div>
    <script type="text/javascript">
    function startLockoutTimer(duration) {
        // Sử dụng lblMessage để hiển thị thông báo và đếm ngược
        var timerDisplay = document.getElementById('<%= lblMessage.ClientID %>');
        var loginButton = document.getElementById('<%= btnLogin.ClientID %>');
        var remaining = duration;

        // Vô hiệu hóa nút Đăng nhập
        loginButton.disabled = true;
        loginButton.value = "⏳ Vui lòng chờ..."; 

        // Đảm bảo thông báo hiển thị lỗi (màu đỏ)
        timerDisplay.style.color = '#dc3545';
        timerDisplay.style.fontWeight = 'bold';


        var timerInterval = setInterval(function () {
            // Hiển thị thông báo và đếm ngược
            timerDisplay.innerHTML = "⚠️ Đã nhập sai 3 lần. Vui lòng chờ **" + remaining + " giây để thử lại.";
            remaining--;

            if (remaining < 0) {
                clearInterval(timerInterval);
                
                // Kích hoạt lại nút
                loginButton.disabled = false;
                loginButton.value = "🚪 Đăng Nhập";
                timerDisplay.innerHTML = "✅ Đã hết thời gian khóa. Vui lòng thử lại.";
                timerDisplay.style.color = '#28a745'; // Đổi màu thông báo thành xanh lá
            }
        }, 1000);
    }

    // Kiểm tra và khởi động lại đồng hồ đếm ngược nếu cần (sau PostBack)
    window.onload = function () {
        var isLockedField = document.getElementById('<%= HiddenFieldIsLocked.ClientID %>');
        var remainingTimeField = document.getElementById('<%= HiddenFieldRemainingTime.ClientID %>');
        
        if (isLockedField && remainingTimeField) {
            var isLocked = isLockedField.value;
            var remainingTime = parseInt(remainingTimeField.value);

            if (isLocked === 'true' && remainingTime > 0) {
                startLockoutTimer(remainingTime);
            }
        }
    };
    </script>
</asp:Content>
