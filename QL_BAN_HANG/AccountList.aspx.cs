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
            if (!IsPostBack)
            {
                LoadDataDropDownList();
            }
            LoadDataAccount();

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
                                       .OrderBy(q => q.Quyen); // Sắp xếp A-Z

            // 3. Gán dữ liệu cho DropDownList (Giả sử ID là ddlPhanQuyen)
            ddlPhanQuyen.DataSource = phanQuyenList.ToList();

            // 4. Chỉ định cột để hiển thị Text và cột chứa Value
            // (Trong trường hợp này, cả hai đều là tên phân quyền)
            ddlPhanQuyen.DataTextField = "Quyen";
            ddlPhanQuyen.DataValueField = "Quyen";

            // 5. Liên kết dữ liệu
            ddlPhanQuyen.DataBind();

            // 6. (Tùy chọn) Thêm một mục "Chọn" hoặc "Tất cả" ở đầu danh sách
            ddlPhanQuyen.Items.Insert(0, new ListItem("-- Chọn phân quyền --", ""));
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

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Attributes.Add("onclick", "javascript:return confirm('Xóa thiệt không?');");

        }

        protected void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Khởi tạo kết nối CSDL
                Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();
                if(ddlPhanQuyen.SelectedValue == "-- Chọn phân quyền --")
                {
                    lblMessage.Text = "⚠️ Vui lòng chọn phân quyền muốn thêm";

                }
                // Tạo đối tượng tài khoản mới
                Tai_Khoan tk = new Tai_Khoan
                {
                    Ho_va_ten = txtht.Text.Trim(),
                    So_dien_thoai = txtsdt.Text.Trim(),
                    Dia_chi = txtdchi.Text.Trim(),
                    Mat_khau = ToMD5(txtmk.Text.Trim()),
                    Phan_quyen = ddlPhanQuyen.SelectedValue
                };

                // Kiểm tra trùng số điện thoại
                var check = context.Tai_Khoans.SingleOrDefault(x => x.So_dien_thoai == tk.So_dien_thoai);
                if (check != null)
                {
                    lblMessage.Text = "⚠️ Số điện thoại đã tồn tại.";
                    return;
                }

                // Thêm vào CSDL
                context.Tai_Khoans.InsertOnSubmit(tk);
                context.SubmitChanges();

                lblMessage.Text = "✅ Thêm tài khoản thành công.";
                ClearForm();
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

        }

        protected void butDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();

                for (int i = 0; i < GridViewAccounts.Rows.Count; i++)
                {
                    // Tìm checkbox trong cột thứ 5 (index = 5)
                    // (Lưu ý: Index 5 là cột thứ 6)
                    CheckBox chk = (CheckBox)GridViewAccounts.Rows[i].Cells[5].FindControl("ckhDelete");

                    if (chk != null && chk.Checked)
                    {
                        // Lỗi đã được sửa ở đây: Thay GridView1 bằng GridViewAccounts
                        string soDienThoai = GridViewAccounts.Rows[i].Cells[0].Text.Trim();

                        // Tìm tài khoản theo số điện thoại
                        Tai_Khoan tk = context.Tai_Khoans.SingleOrDefault(t => t.So_dien_thoai == soDienThoai);

                        if (tk != null)
                        {
                            context.Tai_Khoans.DeleteOnSubmit(tk);
                        }
                    }
                }

                // Lưu thay đổi
                context.SubmitChanges();

                // Tải lại danh sách
                LoadDataAccount();

                lblMessage.Text = "✅ Đã xóa các tài khoản được chọn.";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa: " + ex.Message;
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