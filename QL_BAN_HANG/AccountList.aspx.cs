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
                    Phan_quyen = ddlPhanQuyen.SelectedValue.ToString().Trim()
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

        }

        protected void butDelete_Click(object sender, EventArgs e)
        {
            int countDeleted = 0;

            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    foreach (GridViewRow row in GridViewAccounts.Rows)
                    {
                        CheckBox chkDelete = row.FindControl("ckhDelete") as CheckBox;

                        if (chkDelete != null && chkDelete.Checked)
                        {
                            // Lấy khóa chính từ DataKeys
                            string soDienThoai = GridViewAccounts.DataKeys[row.RowIndex].Value.ToString();

                            // Truy vấn tài khoản cần xóa
                            var taiKhoan = context.Tai_Khoans.SingleOrDefault(tk => tk.So_dien_thoai == soDienThoai);

                            if (taiKhoan != null)
                            {
                                context.Tai_Khoans.DeleteOnSubmit(taiKhoan);
                                countDeleted++;
                            }
                        }
                    }

                    // Nếu có tài khoản bị xóa thì lưu thay đổi
                    if (countDeleted > 0)
                    {
                        context.SubmitChanges();
                        lblMessage.Text = $"✅ Đã xóa {countDeleted} tài khoản.";
                    }
                    else
                    {
                        lblMessage.Text = "⚠️ Vui lòng chọn ít nhất một tài khoản để xóa.";
                    }

                    // Tải lại dữ liệu
                    LoadDataAccount();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"❌ Đã xảy ra lỗi: {ex.Message}";
            }
        }




        // Phương thức xử lý sự kiện cho nút "Đăng Nhập Tài Khoản"
        protected void btnLoginPage_Click(object sender, EventArgs e)
        {
            // Chuyển hướng đến trang Đăng nhập (Login.aspx)
            Session.Clear();
            Response.Redirect("LoginUser.aspx");
            
        }

        protected void GridViewAccounts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewAccounts.EditIndex = -1;
            LoadDataAccount(); // Thoát chế độ chỉnh sửa và tải lại dữ liệu
        }


        protected void GridViewAccounts_DataBound(object sender, EventArgs e)
        {

        }

        protected void GridViewAccounts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewAccounts.EditIndex = e.NewEditIndex;
            LoadDataAccount(); // Tải lại dữ liệu để hiển thị dòng đang chỉnh sửa
        }
        protected void GridViewAccounts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();
                // Lấy dòng đang chỉnh sửa
                GridViewRow row = GridViewAccounts.Rows[e.RowIndex];
                String soDienThoai = Convert.ToString(GridViewAccounts.Rows[e.RowIndex].Cells[1].Text);

                Tai_Khoan ac = context.Tai_Khoans.SingleOrDefault(a => a.So_dien_thoai == soDienThoai);


                if (ac != null)
                {
                    // Lấy giá trị Label từ ô nhập trong cột thứ 2
                    TextBox txtHvT = (TextBox)GridViewAccounts.Rows[e.RowIndex].Cells[0].Controls[1];
                    ac.Ho_va_ten = txtHvT.Text;

                    // Lấy giá trị Pos từ ô nhập trong cột thứ 3
                    TextBox txtDc = (TextBox)GridViewAccounts.Rows[e.RowIndex].Cells[2].Controls[1];
                    ac.Dia_chi = txtDc.Text;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    context.SubmitChanges();
                    lblMessage.Text = "✅ Cập nhật tài khoản thành công.";


                    // Thoát chế độ chỉnh sửa và tải lại dữ liệu
                    GridViewAccounts.EditIndex = -1;
                    LoadDataAccount();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi cập nhật: " + ex.Message;
            }
        }
    }
}