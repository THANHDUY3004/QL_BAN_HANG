using Cua_Hang_Tra_Sua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class LoginUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public class UserAccount
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            string username = txtUsernameLog.Text.Trim(); // Số điện thoại
            string password = txtPasswordLog.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
                return;
            }

            try
            {
                // Sử dụng Cua_Hang_Tra_SuaDataContext để kết nối Database
                Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();

                // Tìm tài khoản khớp trong bảng Tai_Khoan
                var foundUser = context.Tai_Khoans.SingleOrDefault(t =>
                    t.So_dien_thoai == username &&
                    t.Mat_khau == password); // Cần đảm bảo tên cột là chính xác

                if (foundUser != null)
                {
                    // Đăng nhập thành công
                    Session["LoggedInUser"] = foundUser.Ho_va_ten; // Lấy Họ và tên từ Database
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    // Đăng nhập thất bại: Tài khoản không tồn tại hoặc mật khẩu sai
                    lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Lỗi kết nối hoặc xử lý dữ liệu." + ex.Message;
                // Ghi log lỗi (ex.Message)
            }
        }
    }
}