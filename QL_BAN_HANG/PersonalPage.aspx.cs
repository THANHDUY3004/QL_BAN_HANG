using Cua_Hang_Tra_Sua;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace QL_BAN_HANG
{
    public partial class PersonalPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string soDienThoai = Session["LoggedInUser"]?.ToString();
                if (string.IsNullOrEmpty(soDienThoai))
                {
                    Response.Redirect("LoginUser.aspx");
                    return;
                }

                try
                {
                    using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                    {
                        var taiKhoan = context.Tai_Khoans.SingleOrDefault(tk => tk.So_dien_thoai == soDienThoai);

                        if (taiKhoan != null)
                        {
                            txtHoTen.Text = taiKhoan.Ho_va_ten;
                            txtSoDienThoai.Text = taiKhoan.So_dien_thoai;
                            txtDiaChi.Text = taiKhoan.Dia_chi;

                            // Không hiển thị mật khẩu đã mã hóa
                            txtMatKhau.Text = "";
                            txtXacNhanMatKhau.Text = "";

                            txtHoTen.ReadOnly = true;
                            txtSoDienThoai.ReadOnly = true;
                            txtDiaChi.ReadOnly = true;
                            txtMatKhau.ReadOnly = true;
                            txtXacNhanMatKhau.ReadOnly = true;

                            btnSuaThongTin.Visible = true;
                            btnLuuThongTin.Visible = false;
                            btnHuy.Visible = false;
                        }
                        else
                        {
                            lblMessage.Text = "⚠️ Không tìm thấy thông tin tài khoản.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "❌ Lỗi khi tải thông tin: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
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

        public void EditAccountByPhone(string soDienThoai, string hoTenMoi, string diaChiMoi, string matKhauMoi)
        {
            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var taiKhoan = context.Tai_Khoans.SingleOrDefault(tk => tk.So_dien_thoai == soDienThoai);

                    if (taiKhoan != null)
                    {
                        taiKhoan.Ho_va_ten = hoTenMoi;
                        taiKhoan.Dia_chi = string.IsNullOrWhiteSpace(diaChiMoi) ? null : diaChiMoi;

                        if (!string.IsNullOrWhiteSpace(matKhauMoi))
                        {
                            taiKhoan.Mat_khau = ToMD5(matKhauMoi);
                        }

                        context.SubmitChanges();

                        lblMessage.Text = "✅ Thông tin đã được cập nhật.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.Text = "⚠️ Không tìm thấy tài khoản để cập nhật.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi cập nhật: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnSuaThongTin_Click(object sender, EventArgs e)
        {
            txtHoTen.ReadOnly = false;
            txtDiaChi.ReadOnly = false;
            txtMatKhau.ReadOnly = false;
            txtXacNhanMatKhau.ReadOnly = false;

            // KHÔNG mở khóa số điện thoại vì là khóa chính
            txtSoDienThoai.ReadOnly = true;

            btnSuaThongTin.Visible = false;
            btnLuuThongTin.Visible = true;
            btnHuy.Visible = true;
        }


        protected void btnHuy_Click(object sender, EventArgs e)
        {
            txtHoTen.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtMatKhau.ReadOnly = true;
            txtXacNhanMatKhau.ReadOnly = true;

            btnSuaThongTin.Visible = true;
            btnLuuThongTin.Visible = false;
            btnHuy.Visible = false;

            lblMessage.Text = "Đã hủy chỉnh sửa.";
        }

        protected void btnLuuThongTin_Click(object sender, EventArgs e)
        {
            string sdt = txtSoDienThoai.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            string xacNhan = txtXacNhanMatKhau.Text.Trim();

            // ✅ Kiểm tra họ tên (không chứa ký tự đặc biệt hoặc số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(hoTen, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                lblMessage.Text = "❌ Họ tên không được chứa số hoặc ký tự đặc biệt.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // ✅ Kiểm tra số điện thoại (10 chữ số, bắt đầu bằng số 0)
            if (sdt.Length != 10 || !sdt.All(char.IsDigit) || !sdt.StartsWith("0"))
            {
                lblMessage.Text = "❌ Số điện thoại phải gồm 10 chữ số và bắt đầu bằng số 0.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // ✅ Kiểm tra mật khẩu (nếu có nhập)
            if (!string.IsNullOrEmpty(matKhau))
            {
                if (matKhau != xacNhan)
                {
                    lblMessage.Text = "❌ Mật khẩu xác nhận không khớp.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Có thể thêm kiểm tra độ mạnh mật khẩu (tối thiểu 8 ký tự, có chữ hoa, chữ thường, số)
                if (matKhau.Length < 8 ||
                    !matKhau.Any(char.IsUpper) ||
                    !matKhau.Any(char.IsLower) ||
                    !matKhau.Any(char.IsDigit))
                {
                    lblMessage.Text = "❌ Mật khẩu phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường và số.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }

            // ✅ Gọi hàm cập nhật
            EditAccountByPhone(sdt, hoTen, diaChi, matKhau);

            // Khóa lại các ô nhập
            txtHoTen.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtMatKhau.ReadOnly = true;
            txtXacNhanMatKhau.ReadOnly = true;

            btnSuaThongTin.Visible = true;
            btnLuuThongTin.Visible = false;
            btnHuy.Visible = false;

            lblMessage.Text = "✅ Thông tin đã được lưu thành công.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }


        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("LoginUser.aspx");
        }
    }
}
