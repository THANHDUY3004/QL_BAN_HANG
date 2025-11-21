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
    public partial class AccountList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = ""; // Xóa thông báo cũ mỗi lần tải trang
            if (!IsPostBack)
            {
                // Chỉ nạp dữ liệu DropDownList lần đầu
                LoadDataDropDownList();
                // Nạp dữ liệu GridView lần đầu
                LoadDataAccount();
            }
            // BỎ nạp LoadDataAccount() ở đây. Việc này sẽ được xử lý trong các sự kiện PostBack khác. 

            // Nếu bạn muốn nạp lại GridView khi ddlPhanQuyen thay đổi, hãy làm trong sự kiện của nó:
            // Protected void DropDownList1_SelectedIndexChanged(...) { LoadDataAccount(); }

        }
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();
            string selectedQuyen = ddlPhanQuyen.SelectedValue;

            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var query = context.Tai_Khoans.AsQueryable();

                    // Lọc theo từ khóa (nếu có)
                    if (!string.IsNullOrEmpty(tuKhoa))
                    {
                        query = query.Where(tk => tk.Ho_va_ten.Contains(tuKhoa) || tk.So_dien_thoai.Contains(tuKhoa));
                    }

                    // Lọc thêm theo phân quyền nếu được chọn
                    if (!string.IsNullOrEmpty(selectedQuyen))
                    {
                        query = query.Where(tk => tk.Phan_quyen == selectedQuyen);
                    }

                    var ketQua = query.OrderBy(tk => tk.Ho_va_ten).ToList();

                    GridViewAccounts.DataSource = ketQua;
                    GridViewAccounts.DataBind();

                    lblMessage.Text = $"🔍 Tìm thấy {ketQua.Count} kết quả.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi tìm kiếm: " + ex.Message;
            }
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

        /// <summary>
        /// Tải các phân quyền duy nhất (ví dụ: Quản Trị, Khách Hàng) 
        /// từ bảng Tai_Khoan vào DropDownList.
        /// </summary>
        private void LoadDataDropDownList()
        {
            // 1. Khởi tạo DataContext
            // (Giả sử bạn đang dùng NEWSDataContext như các ví dụ trước)
            Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();

            // 2. Truy vấn để lấy các phân quyền duy nhất (Distinct)
            // Chúng ta tạo một đối tượng tạm thời 'new { Quyen = ... }' 
            // để dễ dàng gán DataTextField và DataValueField.
            var phanQuyenList = context.Tai_Khoans
                           .Select(tk => new { Quyen = tk.Phan_quyen })
                           .Distinct()
                           .OrderBy(q => q.Quyen)
                           .ToList();


            // 3. Gán dữ liệu cho DropDownList (Giả sử ID là ddlPhanQuyen)
            ddlPhanQuyen.DataSource = phanQuyenList;

            // 4. Chỉ định cột để hiển thị Text và cột chứa Value
            // (Trong trường hợp này, cả hai đều là tên phân quyền)
            ddlPhanQuyen.DataTextField = "Quyen";
            ddlPhanQuyen.DataValueField = "Quyen";

            // 5. Liên kết dữ liệu
            ddlPhanQuyen.DataBind();
        }
        /// <summary>
        /// Tải danh sách tài khoản vào GridViewAccounts, 
        /// lọc theo phân quyền được chọn trong ddlPhanQuyen.
        /// </summary>
        /// <summary>
        /// Tải danh sách tài khoản vào GridViewAccounts, 
        /// lọc theo phân quyền được chọn trong ddlPhanQuyen.
        /// </summary>
        private void LoadDataAccount()
        {
            try
            {
                // 1. Lấy phân quyền được chọn từ DropDownList
                // (Giả sử ID DropDownList là ddlPhanQuyen)
                string selectedQuyen = Convert.ToString(ddlPhanQuyen.SelectedValue);

                // 2. Khởi tạo DataContext
                Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();

                // 3. Lấy IQueryable cơ sở của bảng Tai_Khoan
                // (Giả định tên bảng trong DBML là Tai_Khoans)
                var query = context.Tai_Khoans.AsQueryable();

                // 4. Áp dụng bộ lọc NẾU một phân quyền cụ thể được chọn
                // (Nếu SelectedValue = "", nghĩa là người dùng chọn "-- Chọn phân quyền --")
                if (!string.IsNullOrEmpty(selectedQuyen))
                {
                    query = query.Where(tk => tk.Phan_quyen == selectedQuyen);
                }

                // 5. Gán dữ liệu cho GridView (Giả sử ID là GridViewAccounts)
                // Sắp xếp theo Họ và Tên (thay vì Số điện thoại)
                GridViewAccounts.DataSource = query.OrderBy(tk => tk.Ho_va_ten).ToList();
                GridViewAccounts.DataBind();
            }
            catch (Exception ex)
            {
                // Bạn nên có một Label (ví dụ: lblMessage) để hiển thị lỗi nếu có
                lblMessage.Text = "Lỗi tải danh sách tài khoản: " + ex.Message;
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Lấy số điện thoại từ khóa chính của hàng được chọn
                string soDienThoai = e.Keys["So_dien_thoai"].ToString();

                // Khởi tạo kết nối đến DB
                Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();

                // Tìm tài khoản cần xóa theo số điện thoại
                Tai_Khoan taiKhoan = context.Tai_Khoans.SingleOrDefault(tk => tk.So_dien_thoai == soDienThoai);

                if (taiKhoan != null)
                {
                    // Xóa và lưu thay đổi
                    context.Tai_Khoans.DeleteOnSubmit(taiKhoan);
                    context.SubmitChanges();

                    // Tải lại danh sách sau khi xóa
                    LoadDataAccount();
                    lblMessage.Text = "✅ Xóa tài khoản thành công.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Không tìm thấy tài khoản cần xóa.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa tài khoản: " + ex.Message;
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Khi DropDownList thay đổi, ta nạp lại GridView theo lựa chọn mới
            LoadDataAccount();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Attributes.Add("onclick", "javascript:return confirm('Xóa thiệt không?');");

        }
        protected void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string hoTen = txtht.Text.Trim();
                string soDienThoai = txtsdt.Text.Trim();
                string diaChi = txtdchi.Text.Trim();
                string matKhau = txtmk.Text.Trim();
                string phanQuyen = ddlPhanQuyen.SelectedValue.ToString().Trim();

                // ✅ Kiểm tra họ tên (không chứa ký tự đặc biệt hoặc số)
                if (!System.Text.RegularExpressions.Regex.IsMatch(hoTen, @"^[a-zA-ZÀ-ỹ\s]+$"))
                {
                    lblMessage.Text = "⚠️ Họ tên không được chứa số hoặc ký tự đặc biệt.";
                    return;
                }

                // ✅ Kiểm tra số điện thoại (10 ký tự, toàn số, bắt đầu bằng 0)
                if (soDienThoai.Length != 10 || !soDienThoai.All(char.IsDigit) || !soDienThoai.StartsWith("0"))
                {
                    lblMessage.Text = "⚠️ Số điện thoại phải gồm 10 chữ số và bắt đầu bằng số 0.";
                    return;
                }

                // Khởi tạo kết nối CSDL
                Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();

                // Kiểm tra trùng số điện thoại
                var check = context.Tai_Khoans.SingleOrDefault(x => x.So_dien_thoai == soDienThoai);
                if (check != null)
                {
                    lblMessage.Text = "⚠️ Số điện thoại đã tồn tại.";
                    return;
                }

                // Tạo đối tượng tài khoản mới
                Tai_Khoan tk = new Tai_Khoan
                {
                    Ho_va_ten = hoTen,
                    So_dien_thoai = soDienThoai,
                    Dia_chi = diaChi,
                    Mat_khau = ToMD5(matKhau),
                    Phan_quyen = phanQuyen
                };

                // Thêm vào CSDL
                context.Tai_Khoans.InsertOnSubmit(tk);
                context.SubmitChanges();

                lblMessage.Text = "✅ Thêm tài khoản thành công.";
                ClearForm();
                LoadDataAccount();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi: " + ex.Message;
            }
        }

        // Hàm xóa dữ liệu form sau khi thêm
        private void ClearForm()
        {
            txtht.Text = "";
            txtsdt.Text = "";
            txtdchi.Text = "";
            txtmk.Text = "";
            ddlPhanQuyen.SelectedIndex = 0;
        }
        protected void GridViewAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataAccount();

        }
        protected void butDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new Cua_Hang_Tra_SuaDataContext())
                {
                    int soTaiKhoanDaXoa = 0;

                    for (int i = 0; i < GridViewAccounts.Rows.Count; i++)
                    {
                        GridViewRow row = GridViewAccounts.Rows[i];
                        CheckBox chkDelete = row.FindControl("ckhDelete") as CheckBox;

                        if (chkDelete != null && chkDelete.Checked)
                        {
                            object dataKey = GridViewAccounts.DataKeys[i]?.Value;
                            if (dataKey != null)
                            {
                                string soDienThoai = dataKey.ToString();
                                var taiKhoan = context.Tai_Khoans.SingleOrDefault(t => t.So_dien_thoai == soDienThoai);

                                if (taiKhoan != null)
                                {
                                    // Xóa các bản ghi con trong Gio_Hang trước
                                    var gioHangs = context.Gio_Hangs.Where(g => g.So_dien_thoai == soDienThoai);
                                    context.Gio_Hangs.DeleteAllOnSubmit(gioHangs);

                                    // Sau đó xóa tài khoản
                                    context.Tai_Khoans.DeleteOnSubmit(taiKhoan);
                                    soTaiKhoanDaXoa++;
                                }
                            }
                        }
                    }

                    if (soTaiKhoanDaXoa > 0)
                    {
                        context.SubmitChanges();
                        lblMessage.Text = $"✅ Đã xóa thành công {soTaiKhoanDaXoa} tài khoản.";
                    }
                    else
                    {
                        lblMessage.Text = "⚠️ Bạn chưa chọn tài khoản nào để xóa.";
                    }

                    LoadDataAccount();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"❌ Có lỗi xảy ra khi xóa: {ex.Message}";
            }
        }
        // Phương thức xử lý sự kiện cho nút "Đăng Nhập Tài Khoản"
        protected void btnLoginPage_Click(object sender, EventArgs e)
        {
            // Chuyển hướng đến trang Đăng nhập (Login.aspx)
            Session.Clear();
            Response.Redirect("LoginUser.aspx");

        }
    }
}