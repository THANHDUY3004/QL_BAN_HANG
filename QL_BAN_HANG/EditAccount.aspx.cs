using Cua_Hang_Tra_Sua;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace QL_BAN_HANG
{
    public partial class EditAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Kiểm tra xem có tham số "sdt" trên URL không
                string sdt = Request.QueryString["ID"];
                if (!string.IsNullOrEmpty(sdt))
                {
                    // Lưu giá trị sdt vào Session
                    Session["sdt"] = sdt;

                    // Gọi hàm LoadAccount để xử lý dữ liệu tương ứng
                    LoadAccount();
                }
                else
                {
                    // Nếu không có sdt, hiển thị thông báo
                    lblMessage.Text = "❌ Thiếu tham số số điện thoại trong URL!";
                    btnLuuThongTin.Enabled = false;
                }
            }
        }

        private void LoadAccount()
        {
            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    // Lấy số điện thoại từ Session (fallback sang QueryString nếu Session null)
                    string sdt = Session["sdt"]?.ToString() ?? Request.QueryString["sdt"];
                    if (string.IsNullOrEmpty(sdt)) return;

                    // Trim để tránh lỗi khoảng trắng
                    sdt = sdt.Trim();

                    // Tìm tài khoản có số điện thoại tương ứng
                    Tai_Khoan tk = context.Tai_Khoans.SingleOrDefault(t => t.So_dien_thoai == sdt);

                    if (tk != null)
                    {
                        // Gán dữ liệu lên các TextBox (sử dụng ?? để tránh null)
                        txtHoTen.Text = tk.Ho_va_ten ?? "";
                        txtSoDienThoai.Text = tk.So_dien_thoai ?? "";
                        txtDiaChi.Text = tk.Dia_chi ?? "";

                        txtMatKhau.Text = "";
                        txtXacNhanMatKhau.Text = "";

                        // Khóa không cho sửa số điện thoại
                        txtSoDienThoai.ReadOnly = true;
                    }
                    else
                    {
                        lblMessage.Text = "❌ Không tìm thấy tài khoản với số điện thoại: " + sdt;
                        btnLuuThongTin.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi tải dữ liệu: " + ex.Message;
                btnLuuThongTin.Enabled = false;
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

        protected void BtnLuuThongTin_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra Session
                if (Session["sdt"] == null)
                {
                    lblMessage.Text = "❌ Không tìm thấy số điện thoại để cập nhật!";
                    return;
                }

                string sdt = Session["sdt"].ToString();
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    Tai_Khoan taiKhoan = context.Tai_Khoans.SingleOrDefault(tk => tk.So_dien_thoai == sdt);

                    if (taiKhoan != null)
                    {
                        string hoTen = txtHoTen.Text.Trim();
                        string diaChi = txtDiaChi.Text.Trim();
                        string matKhau = txtMatKhau.Text.Trim();
                        string nhapLaiMatKhau = txtXacNhanMatKhau.Text.Trim();

                        // ✅ Kiểm tra họ tên (không chứa ký tự đặc biệt hoặc số)
                        if (!System.Text.RegularExpressions.Regex.IsMatch(hoTen, @"^[a-zA-ZÀ-ỹ\s]+$"))
                        {
                            lblMessage.Text = "❌ Họ tên không được chứa số hoặc ký tự đặc biệt.";
                            return;
                        }

                        // Gán lại dữ liệu từ form
                        taiKhoan.Ho_va_ten = hoTen;
                        // KHÔNG cho sửa số điện thoại
                        taiKhoan.Dia_chi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi;

                        // ✅ Kiểm tra mật khẩu (nếu có nhập)
                        if (!string.IsNullOrEmpty(matKhau))
                        {
                            if (matKhau != nhapLaiMatKhau)
                            {
                                lblMessage.Text = "❌ Mật khẩu xác nhận không khớp.";
                                return;
                            }
                            taiKhoan.Mat_khau = ToMD5(matKhau);
                        }

                        // Lưu thay đổi
                        context.SubmitChanges();

                        lblMessage.Text = "✅ Lưu thành công!";
                    }
                    else
                    {
                        lblMessage.Text = "❌ Lưu không thành công! Không tìm thấy bản ghi.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lưu không thành công! Lỗi: " + ex.Message;
            }
        }


        protected void BtnHuy_Click(object sender, EventArgs e)
        {
            Response.Redirect("AccountList.aspx");
        }

        protected void BtnQuayLai_Click(object sender, EventArgs e)
        {
            Response.Redirect("AccountList.aspx");
        }
    }
}
