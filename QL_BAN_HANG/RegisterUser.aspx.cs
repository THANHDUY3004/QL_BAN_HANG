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
    public partial class RegisterUser : System.Web.UI.Page
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

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                lblMessage.Text = "Vui lòng kiểm tra lại các trường thông tin bắt buộc.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string nhapLaiMatKhau = txtXacNhanMatKhau.Text;
            const string phanQuyenMacDinh = "Khách Hàng";

            // ✅ Kiểm tra số điện thoại
            if (soDienThoai.Length != 10 || !soDienThoai.All(char.IsDigit) || !soDienThoai.StartsWith("0"))
            {
                lblMessage.Text = "Số điện thoại phải gồm 10 số và bắt đầu bằng số 0.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (matKhau != nhapLaiMatKhau)
            {
                lblMessage.Text = "Mật khẩu và xác nhận mật khẩu không khớp.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(hoTen, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                lblMessage.Text = "Họ tên không được chứa ký tự đặc biệt hoặc số.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var existingAccount = context.Tai_Khoans.SingleOrDefault(tk => tk.So_dien_thoai == soDienThoai);

                    if (existingAccount != null)
                    {
                        lblMessage.Text = "Số điện thoại này đã được đăng ký. Vui lòng sử dụng số khác.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    Tai_Khoan newAccount = new Tai_Khoan
                    {
                        Ho_va_ten = hoTen,
                        So_dien_thoai = soDienThoai,
                        Dia_chi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi,
                        Mat_khau = ToMD5(matKhau),
                        Ngay_tao = DateTime.Now,
                        Phan_quyen = phanQuyenMacDinh
                    };

                    context.Tai_Khoans.InsertOnSubmit(newAccount);
                    context.SubmitChanges();

                    Session["SoDienThoai"] = soDienThoai;
                    Session["PhanQuyen"] = phanQuyenMacDinh;

                    Response.Redirect("LoginUser.aspx");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Đã xảy ra lỗi khi đăng ký. Vui lòng thử lại sau. Chi tiết: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

    }
}