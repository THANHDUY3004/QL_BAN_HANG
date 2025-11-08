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

            if (!string.IsNullOrEmpty(matKhau) && matKhau != xacNhan)
            {
                lblMessage.Text = "❌ Mật khẩu xác nhận không khớp.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            EditAccountByPhone(sdt, hoTen, diaChi, matKhau);

            txtHoTen.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtMatKhau.ReadOnly = true;
            txtXacNhanMatKhau.ReadOnly = true;

            btnSuaThongTin.Visible = true;
            btnLuuThongTin.Visible = false;
            btnHuy.Visible = false;
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("LoginUser.aspx");
        }
    }
}
