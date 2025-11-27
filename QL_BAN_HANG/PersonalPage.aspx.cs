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
        // ... (Khai báo các Controls đã có)

        // Cần đảm bảo txtMatKhauCu được khai báo trong Designer.cs hoặc thêm thủ công
        // protected System.Web.UI.WebControls.TextBox txtMatKhauCu; 

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

                            // BỔ SUNG: Xóa giá trị mật khẩu cũ
                            txtMatKhauCu.Text = "";
                            txtMatKhau.Text = "";
                            txtXacNhanMatKhau.Text = "";

                            txtHoTen.ReadOnly = true;
                            txtSoDienThoai.ReadOnly = true;
                            txtDiaChi.ReadOnly = true;

                            // BỔ SUNG: Khóa mật khẩu cũ
                            txtMatKhauCu.ReadOnly = true;
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
            // ... (Hàm MD5 giữ nguyên)
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

                        // Chỉ cập nhật mật khẩu khi có mật khẩu mới (đã được xác thực Mật khẩu cũ trước đó)
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

            // BỔ SUNG: Mở khóa mật khẩu cũ và mới
            txtMatKhauCu.ReadOnly = false;
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

            // BỔ SUNG: Khóa lại và xóa nội dung mật khẩu cũ và mới
            txtMatKhauCu.ReadOnly = true;
            txtMatKhauCu.Text = "";
            txtMatKhau.ReadOnly = true;
            txtMatKhau.Text = "";
            txtXacNhanMatKhau.ReadOnly = true;
            txtXacNhanMatKhau.Text = "";

            btnSuaThongTin.Visible = true;
            btnLuuThongTin.Visible = false;
            btnHuy.Visible = false;

            lblMessage.Text = "Đã hủy chỉnh sửa.";
            lblMessage.ForeColor = System.Drawing.Color.Black; // Đặt lại màu cho thông báo hủy
        }

        protected void btnLuuThongTin_Click(object sender, EventArgs e)
        {
            string sdt = txtSoDienThoai.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            // BỔ SUNG: Lấy giá trị mật khẩu cũ
            string matKhauCu = txtMatKhauCu.Text.Trim();

            string matKhauMoi = txtMatKhau.Text.Trim();
            string xacNhanMoi = txtXacNhanMatKhau.Text.Trim();

            // 1. Kiểm tra họ tên và số điện thoại (Giữ nguyên)
            if (!System.Text.RegularExpressions.Regex.IsMatch(hoTen, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                lblMessage.Text = "❌ Họ tên không được chứa số hoặc ký tự đặc biệt.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (sdt.Length != 10 || !sdt.All(char.IsDigit) || !sdt.StartsWith("0"))
            {
                lblMessage.Text = "❌ Số điện thoại phải gồm 10 chữ số và bắt đầu bằng số 0.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // 2. LOGIC ĐỔI MẬT KHẨU:
            // Chỉ tiến hành đổi mật khẩu nếu Mật khẩu MỚI được nhập
            if (!string.IsNullOrEmpty(matKhauMoi))
            {
                // 2a. Bắt buộc nhập mật khẩu cũ
                if (string.IsNullOrEmpty(matKhauCu))
                {
                    lblMessage.Text = "❌ Vui lòng nhập Mật khẩu cũ để xác nhận việc thay đổi mật khẩu.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // 2b. Xác thực mật khẩu cũ
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var taiKhoan = context.Tai_Khoans.SingleOrDefault(tk => tk.So_dien_thoai == sdt);
                    if (taiKhoan != null)
                    {
                        // Kiểm tra MD5 của mật khẩu cũ người dùng nhập với mật khẩu trong DB
                        if (taiKhoan.Mat_khau != ToMD5(matKhauCu))
                        {
                            lblMessage.Text = "❌ Mật khẩu cũ không chính xác.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }

                // 2c. Kiểm tra mật khẩu mới và xác nhận
                if (matKhauMoi != xacNhanMoi)
                {
                    lblMessage.Text = "❌ Mật khẩu mới và Xác nhận mật khẩu mới không khớp.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (matKhauMoi.Length < 8 ||
                    !matKhauMoi.Any(char.IsUpper) ||
                    !matKhauMoi.Any(char.IsLower) ||
                    !matKhauMoi.Any(char.IsDigit))
                {
                    lblMessage.Text = "❌ Mật khẩu mới phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường và số.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
            // Nếu không nhập mật khẩu mới, thì không làm gì với mật khẩu cũ và mới.

            // 3. Gọi hàm cập nhật (chỉ truyền matKhauMoi nếu nó đã được xác thực)
            EditAccountByPhone(sdt, hoTen, diaChi, matKhauMoi); // EditAccountByPhone tự kiểm tra matKhauMoi có rỗng không

            // 4. Khóa lại các ô nhập
            txtHoTen.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtMatKhauCu.ReadOnly = true;
            txtMatKhau.ReadOnly = true;
            txtXacNhanMatKhau.ReadOnly = true;

            btnSuaThongTin.Visible = true;
            btnLuuThongTin.Visible = false;
            btnHuy.Visible = false;

            // 5. Clear mật khẩu sau khi lưu để tránh hiển thị trên trang
            txtMatKhauCu.Text = "";
            txtMatKhau.Text = "";
            txtXacNhanMatKhau.Text = "";
        }


        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("LoginUser.aspx");
        }
    }
}