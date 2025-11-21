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
    public partial class ProductList : System.Web.UI.Page
    {
        public Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDataMenus();
                LoadAddCategoryDropDown();
                LoadStatusDropDown(); // <-- THÊM MỚI: Tải DropDownList trạng thái
                LoadDataProducts();
            }
        }

        // <-- THÊM MỚI: Hàm tải dữ liệu cho DropDownList Trạng Thái
        private void LoadStatusDropDown()
        {
            try
            {
                DropDownList1.Items.Clear();
                DropDownList1.Items.Add(new ListItem("Còn hàng", "Còn hàng"));
                DropDownList1.Items.Add(new ListItem("Hết hàng", "Hết hàng"));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải trạng thái: " + ex.Message;
            }
        }

        private void LoadDataMenus()
        {
            try
            {
                var menuList = context.Menus
                                        .Where(m => m.Parent == 2) // Chỉ lấy danh mục con của "Sản phẩm"
                                      .OrderBy(m => m.OrderKey)
                                      .Select(m => new { ID_MN = m.ID_MN, Label = m.Label })
                                      .ToList();

                ddlMenus.DataSource = menuList;
                ddlMenus.DataTextField = "Label";
                ddlMenus.DataValueField = "ID_MN";
                ddlMenus.DataBind();

                ddlMenus.Items.Insert(0, new ListItem("-- Tất cả Danh mục --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải danh mục lọc: " + ex.Message;
            }
        }

        private void LoadAddCategoryDropDown()
        {
            try
            {
                var categoryList = context.Menus
                                           .Where(m => m.Parent ==2) // Chỉ lấy danh mục con của "Sản phẩm"
                                          .OrderBy(m => m.Label)
                                          .Select(m => new { ID_MN = m.ID_MN, Label = m.Label })
                                          .ToList();

                ddlAddCategory.DataSource = categoryList;
                ddlAddCategory.DataTextField = "Label";
                ddlAddCategory.DataValueField = "ID_MN";
                ddlAddCategory.DataBind();

                ddlAddCategory.Items.Insert(0, new ListItem("-- Chọn danh mục --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải danh mục (thêm mới): " + ex.Message;
            }
        }

        private void LoadDataProducts()
        {
            try
            {
                var query = context.San_Phams.AsQueryable();
                string selectedMenuId = ddlMenus.SelectedValue;

                if (!string.IsNullOrEmpty(selectedMenuId))
                {
                    int idMn = int.Parse(selectedMenuId);
                    query = query.Where(sp => sp.ID_MN == idMn);
                }

                GridViewProducts.DataSource = query.OrderBy(sp => sp.Ten_san_pham).ToList();
                GridViewProducts.DataBind();
                lblMessage.Text = "";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải danh sách sản phẩm: " + ex.Message;
            }
        }

        protected void ddlMenus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataProducts();
        }

        // -------------------------------------------------------------
        // XỬ LÝ THÊM SẢN PHẨM MỚI (Đã sửa logic Trạng thái)
        // -------------------------------------------------------------
        protected void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedCategoryId = ddlAddCategory.SelectedValue;
                string giaNhap = txtGia.Text.Trim().Replace(",", "");

                // ✅ Kiểm tra bắt buộc nhập
                if (string.IsNullOrWhiteSpace(txtTenSP.Text) || string.IsNullOrWhiteSpace(giaNhap))
                {
                    lblMessage.Text = "⚠️ Vui lòng nhập đủ Tên SP và Giá.";
                    return;
                }
                if (string.IsNullOrEmpty(selectedCategoryId))
                {
                    lblMessage.Text = "⚠️ Vui lòng chọn Danh mục.";
                    return;
                }

                // ✅ Kiểm tra giá tiền phải là số
                if (!decimal.TryParse(giaNhap, out decimal giaCoBan))
                {
                    lblMessage.Text = "❌ Giá tiền phải là số hợp lệ.";
                    return;
                }

                // ✅ Kiểm tra ID danh mục phải là số
                if (!int.TryParse(selectedCategoryId, out int idMn))
                {
                    lblMessage.Text = "❌ ID Danh mục không hợp lệ.";
                    return;
                }

                // (BƯỚC MỚI) Xử lý file Upload
                string fileName = null;
                if (fileUploadHinhAnh.HasFile)
                {
                    try
                    {
                        fileName = Path.GetFileName(fileUploadHinhAnh.FileName);
                        string savePath = Server.MapPath("~/uploads/images/") + fileName;
                        fileUploadHinhAnh.SaveAs(savePath);
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "❌ Lỗi khi tải lên hình ảnh: " + ex.Message;
                        return;
                    }
                }

                // ✅ Tạo đối tượng sản phẩm mới
                San_Pham newProduct = new San_Pham
                {
                    Ten_san_pham = txtTenSP.Text.Trim(),
                    Gia_co_ban = giaCoBan,
                    Mo_ta_san_pham = txtMoTa.Text.Trim(),
                    ID_MN = idMn,
                    Trang_thai = DropDownList1.SelectedValue,
                    Hinh_anh = fileName
                };

                context.San_Phams.InsertOnSubmit(newProduct);
                context.SubmitChanges();

                lblMessage.Text = $"✅ Thêm sản phẩm '{newProduct.Ten_san_pham}' thành công.";

                // Reset form
                txtTenSP.Text = string.Empty;
                txtGia.Text = string.Empty;
                txtMoTa.Text = string.Empty;
                ddlAddCategory.SelectedIndex = 0;
                DropDownList1.SelectedIndex = 0;

                LoadDataProducts();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi thêm sản phẩm: " + ex.Message;
            }
        }


        // -------------------------------------------------------------
        // Xử lý Xóa (Giữ nguyên)
        // -------------------------------------------------------------

        protected void GridViewProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int idSp = Convert.ToInt32(GridViewProducts.DataKeys[e.RowIndex].Value);
                San_Pham sanPham = context.San_Phams.SingleOrDefault(sp => sp.ID_SP == idSp);

                if (sanPham != null)
                {
                    context.San_Phams.DeleteOnSubmit(sanPham);
                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Xóa sản phẩm có ID={idSp} thành công.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Không tìm thấy sản phẩm cần xóa.";
                }
                LoadDataProducts();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa sản phẩm: " + ex.Message;
            }
        }


        // -------------------------------------------------------------
        // Xử lý Cập nhật (Đã sửa logic Trạng thái)
        // -------------------------------------------------------------

        protected void GridViewProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewProducts.EditIndex = e.NewEditIndex;
            LoadDataProducts();
        }

        protected void GridViewProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewProducts.EditIndex = -1;
            LoadDataProducts();
        }

        protected void GridViewProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int idSp = Convert.ToInt32(GridViewProducts.DataKeys[e.RowIndex].Value);

                // Tìm tất cả các điều khiển
                TextBox txtEditTenSP = (TextBox)GridViewProducts.Rows[e.RowIndex].FindControl("txtEditTenSP");
                TextBox txtEditGia = (TextBox)GridViewProducts.Rows[e.RowIndex].FindControl("txtEditGia");
                TextBox txtEditIDMN = (TextBox)GridViewProducts.Rows[e.RowIndex].FindControl("txtEditIDMN");
                CheckBox chkEditTrangThai = (CheckBox)GridViewProducts.Rows[e.RowIndex].FindControl("chkEditTrangThai"); // <-- SỬA LỖI 2: Tìm CheckBox
                TextBox txtEditHinhAnh = (TextBox)GridViewProducts.Rows[e.RowIndex].FindControl("txtEditHinhAnh");

                string giaCapNhat = txtEditGia.Text.Trim().Replace(",", "");

                // <-- SỬA LỖI 2: Cập nhật lại điều kiện kiểm tra
                if (txtEditTenSP == null || txtEditGia == null || txtEditIDMN == null || chkEditTrangThai == null || txtEditHinhAnh == null)
                {
                    lblMessage.Text = "❌ Lỗi: Không tìm thấy các điều khiển nhập liệu trong chế độ sửa.";
                    return;
                }

                if (!decimal.TryParse(giaCapNhat, out decimal newGiaCoBan) || !int.TryParse(txtEditIDMN.Text.Trim(), out int newIdMn))
                {
                    lblMessage.Text = "❌ Giá và ID Danh mục phải là số hợp lệ.";
                    return;
                }

                San_Pham sanPhamToUpdate = context.San_Phams.SingleOrDefault(sp => sp.ID_SP == idSp);

                if (sanPhamToUpdate != null)
                {
                    sanPhamToUpdate.Ten_san_pham = txtEditTenSP.Text.Trim();
                    sanPhamToUpdate.Gia_co_ban = newGiaCoBan;
                    sanPhamToUpdate.ID_MN = newIdMn;

                    // <-- SỬA LỖI 2: Lấy giá trị từ CheckBox
                    sanPhamToUpdate.Trang_thai = chkEditTrangThai.Checked ? "Còn hàng" : "Hết hàng";

                    sanPhamToUpdate.Hinh_anh = txtEditHinhAnh.Text.Trim();

                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Cập nhật sản phẩm ID={idSp} thành công.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Không tìm thấy sản phẩm cần cập nhật.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi cập nhật dữ liệu: " + ex.Message;
            }

            GridViewProducts.EditIndex = -1;
            LoadDataProducts();
        }

        protected void GridViewProducts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Bạn CẦN đảm bảo file .aspx của GridView1 có thuộc tính này:
        // <asp:GridView ID="GridView1" runat="server" DataKeyNames="ID_MN" ... >
        //                                             ^^^^^^^^^^^^^^^^^^

        // Đảm bảo tên hàm này khớp với sự kiện OnClick của nút
        protected void Button2_Click(object sender, EventArgs e)
        {
            // Giả sử 'context' là biến thành viên đã khởi tạo
            try
            {
                List<San_Pham> productsToDelete = new List<San_Pham>();
                int soSpDaChon = 0;

                for (int i = 0; i < GridViewProducts.Rows.Count; i++)
                {
                    // SỬA 1: Dùng đúng ID control là "chkDelete"
                    CheckBox chk = (CheckBox)GridViewProducts.Rows[i].FindControl("chkDelete");

                    if (chk != null && chk.Checked)
                    {
                        // SỬA 2: DataKey của GridViewProducts là ID_SP (Mã Sản Phẩm)
                        int idSp = Convert.ToInt32(GridViewProducts.DataKeys[i].Value);

                        // SỬA 3: Logic là tìm SẢN PHẨM (San_Pham) theo ID_SP
                        San_Pham sp = context.San_Phams.SingleOrDefault(t => t.ID_SP == idSp);
                        if (sp != null)
                        {
                            productsToDelete.Add(sp);
                            soSpDaChon++;
                        }
                    }
                }
                if (soSpDaChon > 0)
                {
                    // Xóa tất cả các sản phẩm đã chọn
                    context.San_Phams.DeleteAllOnSubmit(productsToDelete);
                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Đã xóa thành công {soSpDaChon} sản phẩm được chọn.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Vui lòng chọn ít nhất một sản phẩm để xóa.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa: " + ex.Message;
            }

            // Tải lại danh sách SẢN PHẨM
            LoadDataProducts();
        }
    }
}