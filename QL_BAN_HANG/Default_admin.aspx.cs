using Cua_Hang_Tra_Sua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class Default_admin : System.Web.UI.Page
    {
        private Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Tải danh mục cho cả hai DropDownList
                LoadMenus();
                LoadAddMenus();

                // Tải danh sách bài viết lên GridView
                LoadDataBaiViet();
            }
        }
        // --- CÁC HÀM TẢI DỮ LIỆU (LOAD) ---

        /// <summary>
        /// Tải danh mục cho DropDownList LỌC (ddlFilterMenus)
        /// </summary>
        private void LoadMenus()
        {
            try
            {
                var menuList = context.Menus
                                      .Where(m => m.Parent == null)
                                      .OrderBy(m => m.OrderKey)
                                      .Select(m => new { ID_MN = m.ID_MN, Label = m.Label })
                                      .ToList();

                ddlFilterMenus.DataSource = menuList;
                ddlFilterMenus.DataTextField = "Label";
                ddlFilterMenus.DataValueField = "ID_MN";
                ddlFilterMenus.DataBind();

                ddlFilterMenus.Items.Insert(0, new ListItem("Tất cả Danh mục ", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải danh mục lọc: " + ex.Message;
            }
        }

        /// <summary>
        /// Tải danh mục cho DropDownList THÊM MỚI (ddlAddMenu)
        /// </summary>
        private void LoadAddMenus()
        {
            try
            {
                var categoryList = context.Menus
                                          .Where(m => m.Parent == 2)
                                          .OrderBy(m => m.Parent == null)
                                          .Select(m => new { ID_MN = m.ID_MN, Label = m.Label })
                                          .ToList();

                ddlAddMenu.DataSource = categoryList;
                ddlAddMenu.DataTextField = "Label";
                ddlAddMenu.DataValueField = "ID_MN";
                ddlAddMenu.DataBind();

                ddlAddMenu.Items.Insert(0, new ListItem("Chọn danh mục ", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải danh mục (thêm mới): " + ex.Message;
            }
        }

        /// <summary>
        /// Tải danh sách Bài Viết vào GridView, lọc theo danh mục được chọn
        /// </summary>
        private void LoadDataBaiViet()
        {
            try
            {
                // Lấy truy vấn cơ sở
                var query = context.Bai_Viets.AsQueryable();

                // Lấy ID_MN được chọn từ DropDownList LỌC
                string selectedMenuId = ddlFilterMenus.SelectedValue;

                // Nếu có chọn danh mục, thì lọc theo ID_MN
                if (!string.IsNullOrEmpty(selectedMenuId))
                {
                    int idMn = int.Parse(selectedMenuId);
                    query = query.Where(bv => bv.ID_MN == idMn);
                }

                // Gán dữ liệu cho GridView
                GridViewBaiViet.DataSource = query.OrderBy(bv => bv.OrderKey).ThenBy(bv => bv.Tieu_de).ToList();
                GridViewBaiViet.DataBind();
                lblMessage.Text = ""; // Xóa thông báo cũ
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải danh sách bài viết: " + ex.Message;
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi thay đổi DropDownList LỌC
        /// </summary>
        protected void ddlFilterMenus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Tải lại GridView theo danh mục mới
            LoadDataBaiViet();
        }

        // --- CÁC SỰ KIỆN THÊM (ADD) ---

        protected void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra dữ liệu nhập
                string selectedMenuId = ddlAddMenu.SelectedValue;
                if (string.IsNullOrEmpty(selectedMenuId))
                {
                    lblMessage.Text = "⚠️ Vui lòng chọn Danh mục (Menu).";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtAddTieuDe.Text) || string.IsNullOrWhiteSpace(txtAddNoiDung.Text))
                {
                    lblMessage.Text = "⚠️ Vui lòng nhập Tiêu đề và Nội dung đầy đủ.";
                    return;
                }
                if (!int.TryParse(txtAddOrderKey.Text, out int orderKey))
                {
                    lblMessage.Text = "❌ Thứ tự (OrderKey) phải là một con số.";
                    return;
                }
                int idMn = int.Parse(selectedMenuId);

                // 2. Xử lý File Upload
                string fileName = null;
                if (fileUploadHinhAnh.HasFile)
                {
                    try
                    {
                        fileName = Path.GetFileName(fileUploadHinhAnh.FileName);
                        string savePath = Server.MapPath("~/uploads/images/") + fileName;
                        fileUploadHinhAnh.SaveAs(savePath); // Lưu file vào thư mục 
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "❌ Lỗi khi tải lên hình ảnh: " + ex.Message;
                        return; // Dừng lại nếu lưu file lỗi
                    }
                }

                // 3. Tạo đối tượng Bai_Viet mới
                Bai_Viet newBaiViet = new Bai_Viet
                {
                    //ID_BV tự động tăng
                    ID_MN = idMn,
                    Tieu_de = txtAddTieuDe.Text.Trim(),
                    Tom_tac = txtAddTomTat.Text.Trim(),
                    Noi_dung = txtAddNoiDung.Text.Trim(),
                    Hinh_anh_page = fileName, // Tên file hoặc null
                    OrderKey = orderKey
                };

                // 4. Lưu vào CSDL
                context.Bai_Viets.InsertOnSubmit(newBaiViet);
                context.SubmitChanges();

                lblMessage.Text = $"✅ Thêm bài viết '{newBaiViet.Tieu_de}' thành công.";

                // 5. Xóa các ô nhập
                txtAddTieuDe.Text = string.Empty;
                txtAddTomTat.Text = string.Empty;
                txtAddNoiDung.Text = string.Empty;
                txtAddOrderKey.Text = "1";
                ddlAddMenu.SelectedIndex = 0;

                // 6. Tải lại GridView
                LoadDataBaiViet();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi thêm bài viết: " + ex.Message;
            }
        }


        // --- CÁC SỰ KIỆN XÓA (DELETE) ---

        protected void GridViewBaiViet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Lấy ID_BV từ DataKeys
                int idBv = Convert.ToInt32(GridViewBaiViet.DataKeys[e.RowIndex].Value);
                Bai_Viet baiViet = context.Bai_Viets.SingleOrDefault(bv => bv.ID_BV == idBv);

                if (baiViet != null)
                {
                    // Xóa file ảnh vật lý (nếu có)
                    DeleteImageFile(baiViet.Hinh_anh_page);

                    // Xóa khỏi CSDL
                    context.Bai_Viets.DeleteOnSubmit(baiViet);
                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Xóa bài viết ID={idBv} thành công.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Không tìm thấy bài viết cần xóa.";
                }

                LoadDataBaiViet(); // Tải lại GridView
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa bài viết: " + ex.Message;
            }
        }

        protected void butDeleteSelected_Click(object sender, EventArgs e)
        {
            try
            {
                List<Bai_Viet> baiVietsToDelete = new List<Bai_Viet>();
                int soBaiVietDaChon = 0;

                foreach (GridViewRow row in GridViewBaiViet.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkDelete");
                    if (chk != null && chk.Checked)
                    {
                        int idBv = Convert.ToInt32(GridViewBaiViet.DataKeys[row.RowIndex].Value);
                        Bai_Viet bv = context.Bai_Viets.SingleOrDefault(t => t.ID_BV == idBv);
                        if (bv != null)
                        {
                            baiVietsToDelete.Add(bv);
                            soBaiVietDaChon++;
                        }
                    }
                }

                if (soBaiVietDaChon > 0)
                {
                    // Xóa các file ảnh vật lý trước
                    foreach (var baiViet in baiVietsToDelete)
                    {
                        DeleteImageFile(baiViet.Hinh_anh_page);
                    }

                    // Xóa khỏi CSDL
                    context.Bai_Viets.DeleteAllOnSubmit(baiVietsToDelete);
                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Đã xóa thành công {soBaiVietDaChon} bài viết được chọn.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Vui lòng chọn ít nhất một bài viết để xóa.";
                }

                LoadDataBaiViet(); // Tải lại GridView
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa hàng loạt: " + ex.Message;
            }
        }

        /// <summary>
        /// Hàm hỗ trợ xóa file ảnh vật lý từ thư mục 
        /// </summary>
        private void DeleteImageFile(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string path = Server.MapPath("~/uploads/images/") + fileName;
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (IOException ex)
                    {
                        // Ghi log lỗi nếu cần, nhưng không dừng chương trình
                        System.Diagnostics.Debug.WriteLine("Lỗi xóa file ảnh: " + ex.Message);
                    }
                }
            }
        }


        // --- CÁC SỰ KIỆN SỬA/CẬP NHẬT (EDIT/UPDATE) ---

        protected void GridViewBaiViet_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewBaiViet.EditIndex = e.NewEditIndex;
            LoadDataBaiViet(); // Tải lại để hiển thị TextBox
        }

        protected void GridViewBaiViet_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewBaiViet.EditIndex = -1;
            LoadDataBaiViet(); // Tải lại để hiển thị Label
        }

        protected void GridViewBaiViet_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // 1. Lấy khóa chính (ID_BV)
                int idBv = Convert.ToInt32(GridViewBaiViet.DataKeys[e.RowIndex].Value);

                // 2. Tìm các điều khiển TextBox trong EditItemTemplate
                TextBox txtEditHinhAnh = (TextBox)GridViewBaiViet.Rows[e.RowIndex].FindControl("txtEditHinhAnh");
                TextBox txtEditTieuDe = (TextBox)GridViewBaiViet.Rows[e.RowIndex].FindControl("txtEditTieuDe");
                TextBox txtEditTomTat = (TextBox)GridViewBaiViet.Rows[e.RowIndex].FindControl("txtEditTomTat");
                TextBox txtEditIDMN = (TextBox)GridViewBaiViet.Rows[e.RowIndex].FindControl("txtEditIDMN");
                TextBox txtEditOrderKey = (TextBox)GridViewBaiViet.Rows[e.RowIndex].FindControl("txtEditOrderKey");

                // 3. Kiểm tra các điều khiển có tồn tại không
                if (txtEditHinhAnh == null || txtEditTieuDe == null || txtEditTomTat == null || txtEditIDMN == null || txtEditOrderKey == null)
                {
                    lblMessage.Text = "❌ Lỗi: Không tìm thấy các điều khiển (TextBox) trong chế độ sửa.";
                    return;
                }

                // 4. Kiểm tra và chuyển đổi kiểu dữ liệu
                if (!int.TryParse(txtEditIDMN.Text.Trim(), out int newIdMn) || !int.TryParse(txtEditOrderKey.Text.Trim(), out int newOrderKey))
                {
                    lblMessage.Text = "❌ ID Menu và Thứ tự phải là số hợp lệ.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtEditTieuDe.Text))
                {
                    lblMessage.Text = "❌ Tiêu đề không được để trống.";
                    return;
                }

                // 5. Cập nhật vào Database
                Bai_Viet baiVietToUpdate = context.Bai_Viets.SingleOrDefault(bv => bv.ID_BV == idBv);

                if (baiVietToUpdate != null)
                {
                    baiVietToUpdate.Tieu_de = txtEditTieuDe.Text.Trim();
                    baiVietToUpdate.Tom_tac = txtEditTomTat.Text.Trim();
                    baiVietToUpdate.ID_MN = newIdMn;
                    baiVietToUpdate.OrderKey = newOrderKey;
                    baiVietToUpdate.Hinh_anh_page = txtEditHinhAnh.Text.Trim(); // Cập nhật tên file (nếu sửa)

                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Cập nhật bài viết ID={idBv} thành công.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Không tìm thấy bài viết cần cập nhật.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi cập nhật dữ liệu: " + ex.Message;
            }

            // 6. Thoát chế độ Edit và tải lại GridView
            GridViewBaiViet.EditIndex = -1;
            LoadDataBaiViet();
        }
        protected void butDelete_Click1(object sender, EventArgs e)
        {
            // Giả định 'context' là biến thành viên đã khởi tạo (private Cua_Hang_Tra_SuaDataContext context)
            // Nếu context KHÔNG phải là biến thành viên, bạn cần khởi tạo nó lại tại đây:
            // Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();

            try
            {
                List<Bai_Viet> baiVietsToDelete = new List<Bai_Viet>();
                int soBaiVietDaChon = 0;

                for (int i = 0; i < GridViewBaiViet.Rows.Count; i++)
                {
                    GridViewRow row = GridViewBaiViet.Rows[i];
                    // Tìm CheckBox trong hàng. Giả định ID là "chkDelete" (thường dùng trong ASP.NET)
                    // Nếu ID CheckBox là "ckhDelete" như trong mã cũ, hãy thay đổi lại.
                    CheckBox chk = (CheckBox)row.FindControl("chkDelete");

                    if (chk != null && chk.Checked)
                    {
                        // Lấy khóa chính ID_BV từ DataKeys (Cách lấy ID an toàn hơn)
                        int idBv = Convert.ToInt32(GridViewBaiViet.DataKeys[i].Value);

                        // Tìm đối tượng Bài Viết trong CSDL
                        Bai_Viet bv = context.Bai_Viets.SingleOrDefault(t => t.ID_BV == idBv);

                        if (bv != null)
                        {
                            baiVietsToDelete.Add(bv);
                            soBaiVietDaChon++;
                        }
                    }
                }

                if (soBaiVietDaChon > 0)
                {
                    // Bước 1: Xóa file ảnh vật lý trước
                    foreach (var baiViet in baiVietsToDelete)
                    {
                        // Gọi hàm hỗ trợ xóa file ảnh đã định nghĩa trong class Bai_viet.aspx.cs
                        DeleteImageFile(baiViet.Hinh_anh_page);
                    }

                    // Bước 2: Xóa khỏi CSDL
                    context.Bai_Viets.DeleteAllOnSubmit(baiVietsToDelete);
                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Đã xóa thành công {soBaiVietDaChon} bài viết được chọn.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Vui lòng chọn ít nhất một bài viết để xóa.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa hàng loạt bài viết: " + ex.Message;
            }

            // Tải lại danh sách Bài Viết
            LoadDataBaiViet();
        }


    }
}