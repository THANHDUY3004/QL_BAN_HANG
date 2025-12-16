using Cua_Hang_Tra_Sua;
using System;
using System.Linq; // Cần có để dùng SingleOrDefault
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace QL_BAN_HANG
{
    public partial class LoginUser : System.Web.UI.Page
    {
        // Định nghĩa hằng số
        private const int MAX_ATTEMPTS = 3;
        private const int LOCKOUT_DURATION_SECONDS = 3;
        private const string SESSION_KEY_ATTEMPTS = "LoginAttempts";
        private const string SESSION_KEY_LOCKOUT_TIME = "LockoutTime";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Luôn kiểm tra trạng thái khóa khi page load để đảm bảo nút đăng nhập bị vô hiệu hóa
            CheckLockoutStatus();
        }

        /// <summary>
        /// Mã hóa mật khẩu bằng MD5.
        /// </summary>
        public string ToMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // "x2" để giữ định dạng 2 chữ số hex
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái khóa (lockout) và cập nhật giao diện, đồng thời khởi động JS timer.
        /// </summary>
        private void CheckLockoutStatus()
        {
            if (Session[SESSION_KEY_LOCKOUT_TIME] != null)
            {
                DateTime lockoutTime = (DateTime)Session[SESSION_KEY_LOCKOUT_TIME];
                TimeSpan timeElapsed = DateTime.Now - lockoutTime;

                if (timeElapsed.TotalSeconds < LOCKOUT_DURATION_SECONDS)
                {
                    // Vẫn đang trong thời gian khóa
                    int remainingSeconds = LOCKOUT_DURATION_SECONDS - (int)Math.Floor(timeElapsed.TotalSeconds);

                    // Vô hiệu hóa nút và hiển thị thông báo
                    btnLogin.Enabled = false;
                    lblMessage.Text = $"⚠️ Đã nhập sai {MAX_ATTEMPTS} lần. Vui lòng chờ {remainingSeconds} giây để thử lại.";

                    // Khởi động JavaScript timer (tái sử dụng từ giải pháp trước)
                    // Cần có ScriptManager nếu sử dụng MasterPage hoặc UpdatePanel
                    if (ScriptManager.GetCurrent(this) != null)
                    {
                        string script = $"startLockoutTimer({remainingSeconds});";
                        ScriptManager.RegisterStartupScript(this, GetType(), "LockoutTimer", script, true);
                    }
                    else
                    {
                        // Nếu không có ScriptManager, ta dùng ClientScript
                        ClientScript.RegisterStartupScript(this.GetType(), "LockoutTimer", $"startLockoutTimer({remainingSeconds});", true);
                    }
                }
                else
                {
                    // Hết thời gian khóa, reset counter và thông báo sẵn sàng
                    Session[SESSION_KEY_ATTEMPTS] = 0;
                    Session[SESSION_KEY_LOCKOUT_TIME] = null;
                    btnLogin.Enabled = true;
                    lblMessage.Text = "✅ Có thể thử lại. Vui lòng nhập lại thông tin.";
                }
            }
            else
            {
                // Không bị khóa
                btnLogin.Enabled = true;
                // Nếu không phải postback, xóa thông báo cũ
                if (!IsPostBack)
                {
                    lblMessage.Text = string.Empty;
                }
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            // 1. Kiểm tra trạng thái khóa trước khi xử lý đăng nhập
            if (Session[SESSION_KEY_LOCKOUT_TIME] != null && ((DateTime)Session[SESSION_KEY_LOCKOUT_TIME]).AddSeconds(LOCKOUT_DURATION_SECONDS) > DateTime.Now)
            {
                // Vẫn đang bị khóa
                CheckLockoutStatus();
                return;
            }

            string username = txtUsernameLog.Text.Trim(); // Số điện thoại
            string password = txtPasswordLog.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ số điện thoại và mật khẩu.";
                return;
            }

            try
            {
                // Kết nối CSDL
                Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();

                // Mã hóa mật khẩu nếu đang dùng MD5
                string hashedPassword = ToMD5(password);

                // Tìm tài khoản khớp
                var foundUser = context.Tai_Khoans.SingleOrDefault(t =>
                    t.So_dien_thoai == username &&
                    t.Mat_khau == hashedPassword);

                if (foundUser != null)
                {
                    // Đăng nhập thành công: Reset counter
                    Session[SESSION_KEY_ATTEMPTS] = 0;
                    Session[SESSION_KEY_LOCKOUT_TIME] = null;

                    // Lưu thông tin đăng nhập
                    Session["LoggedInUser"] = foundUser.So_dien_thoai;
                    Session["UserRole"] = foundUser.Phan_quyen;

                    // Điều hướng theo phân quyền
                    if (foundUser.Phan_quyen == "Khách Hàng")
                    {
                        Response.Redirect("HomePage.aspx");
                    }
                    else if (foundUser.Phan_quyen == "Quản Trị")
                    {
                        Response.Redirect("AccountList.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "⚠️ Phân quyền không hợp lệ.";
                    }
                }
                else
                {
                    // Đăng nhập thất bại: Tăng counter
                    int attempts = (int)(Session[SESSION_KEY_ATTEMPTS] ?? 0);
                    attempts++;
                    Session[SESSION_KEY_ATTEMPTS] = attempts;

                    if (attempts >= MAX_ATTEMPTS)
                    {
                        // Khóa tài khoản
                        Session[SESSION_KEY_LOCKOUT_TIME] = DateTime.Now;
                        CheckLockoutStatus(); // Kích hoạt lockout timer và vô hiệu hóa nút
                    }
                    else
                    {
                        // Hiển thị thông báo sai mật khẩu
                        int remaining = MAX_ATTEMPTS - attempts;
                        lblMessage.Text = $"Tên đăng nhập hoặc mật khẩu không đúng. Bạn còn {remaining} lần thử.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi kết nối hoặc xử lý dữ liệu: " + ex.Message;
            }
        }
    }
}