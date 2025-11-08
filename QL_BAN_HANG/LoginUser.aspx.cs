using Cua_Hang_Tra_Sua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        public string ToMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi sang chuỗi hex
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // "x2" để giữ định dạng 2 chữ số hex
                }

                return sb.ToString();
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

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
                    // Lưu thông tin đăng nhập
                    Session["LoggedInUser"] = foundUser.So_dien_thoai;
                    Session["UserRole"] = foundUser.Phan_quyen;

                    // Điều hướng theo phân quyền
                    if (foundUser.Phan_quyen == "Khách Hàng")
                    {
                        Response.Redirect("PersonalPage.aspx");
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
                    lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi kết nối hoặc xử lý dữ liệu: " + ex.Message;
            }
        }
    }
}