using Cua_Hang_Tra_Sua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class ProductList : System.Web.UI.Page
    {
        public Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();
        // Định nghĩa đường dẫn lưu trữ hình ảnh
        private const string ImageUploadPath = "~/uploads/images/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdown(ddlMenus);
                LoadDataMenus();
                LoadAddCategoryDropDown();
                LoadStatusDropDown();
                LoadEditCategoryDropDown(); // Tải DropDown cho form SỬA
                LoadDataProducts(0);
            }
        }

        //=== TẢI DỮ LIỆU CHUNG ===

        private void LoadStatusDropDown()
        {
            try
            {
                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem("Còn hàng", "Còn hàng"));
                ddlStatus.Items.Add(new ListItem("Hết hàng", "Hết hàng"));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải trạng thái: " + ex.Message;
            }
        }

        private void LoadDropdown(DropDownList ddl, bool includeAllOption = false)
        {
            try
            {
                var menuList = context.Danh_Mucs
                    .Where(m => m.Parent == 2) // Chỉ lấy danh mục con của "Sản phẩm"
                    .OrderBy(m => m.OrderKey)
                    .Select(m => new { ID_MN = m.ID_DM, Label = m.Label })
                    .ToList();

                ddl.DataSource = menuList;
                ddl.DataTextField = "Label";
                ddl.DataValueField = "ID_MN";
                ddl.DataBind();

                if (includeAllOption)
                {
                    ddl.Items.Insert(0, new ListItem("-- Tất cả Danh mục --", ""));
                }
                else
                {
                    ddl.Items.Insert(0, new ListItem("-- Chọn danh mục --", ""));
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"❌ Lỗi tải danh mục {(includeAllOption ? "(lọc)" : "(thêm/sửa)")}: " + ex.Message;
            }
        }

        private void LoadDataMenus() => LoadDropdown(ddlMenus, true);
        private void LoadAddCategoryDropDown() => LoadDropdown(ddlAddCategory);
        private void LoadEditCategoryDropDown() => LoadDropdown(ddlEditCategory); // Tải DropDown cho form sửa

        private void LoadDataProducts(int page)
        {
            try
            {
                var query = context.San_Phams.AsQueryable();
                string selectedMenuId = ddlMenus.SelectedValue;

                if (!string.IsNullOrEmpty(selectedMenuId))
                {
                    int idMn = int.Parse(selectedMenuId);
                    query = query.Where(sp => sp.ID_DM == idMn);
                }

                GridViewProducts.DataSource = query.OrderBy(sp => sp.OrderKey).ToList();
                GridViewProducts.PageIndex = page;
                GridViewProducts.PageSize = 10; // Tăng PageSize lên 10
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
            LoadDataProducts(0);
        }

        protected void GridViewProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadDataProducts(e.NewPageIndex);
        }

        // --- === THÊM MỚI SẢN PHẨM ===-------------------------------------------------------------------------------


        protected void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Ẩn form sửa nếu đang hiển thị
                pnlEditProduct.Visible = false;

                string selectedCategoryId = ddlAddCategory.SelectedValue;
                string giaNhap = txtGia.Text.Trim().Replace(",", "");

                if (string.IsNullOrWhiteSpace(txtTenSP.Text) || string.IsNullOrWhiteSpace(giaNhap) || string.IsNullOrEmpty(selectedCategoryId))
                {
                    lblMessage.Text = "⚠️ Vui lòng nhập đủ Tên SP, Giá và chọn Danh mục.";
                    return;
                }

                if (!decimal.TryParse(giaNhap, out decimal giaCoBan))
                {
                    lblMessage.Text = "❌ Giá tiền phải là số hợp lệ.";
                    return;
                }
                if (!int.TryParse(selectedCategoryId, out int idMn))
                {
                    lblMessage.Text = "❌ ID Danh mục không hợp lệ.";
                    return;
                }

                // Xử lý file Upload
                string fileName = "0.jpg"; // Mặc định
                if (fileUploadHinhAnh.HasFile)
                {
                    try
                    {
                        string extension = Path.GetExtension(fileUploadHinhAnh.FileName).ToLower();
                        if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                        {
                            lblMessage.Text = "❌ Chỉ chấp nhận file hình ảnh (.jpg, .png, .jpeg).";
                            return;
                        }
                        // Tạo tên file duy nhất để tránh trùng lặp
                        fileName = Guid.NewGuid().ToString() + extension;
                        string savePath = Server.MapPath(ImageUploadPath) + fileName;

                        // Đảm bảo thư mục tồn tại
                        if (!Directory.Exists(Server.MapPath(ImageUploadPath)))
                            Directory.CreateDirectory(Server.MapPath(ImageUploadPath));

                        fileUploadHinhAnh.SaveAs(savePath);
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "❌ Lỗi khi tải lên hình ảnh: " + ex.Message;
                        return;
                    }
                }

                // Lấy OrderKey lớn nhất hiện tại để đặt cho sản phẩm mới
                int maxOrderKey = context.San_Phams.Any() ? context.San_Phams.Max(sp => sp.OrderKey) ?? 0 : 0;

                // Tạo đối tượng sản phẩm mới
                San_Pham newProduct = new San_Pham
                {
                    Ten_san_pham = txtTenSP.Text.Trim(),
                    Gia_co_ban = giaCoBan,
                    Mo_ta_san_pham = txtMoTa.Text.Trim(),
                    ID_DM = idMn,
                    Trang_thai = ddlStatus.SelectedValue,
                    IsHot = false,
                    OrderKey = maxOrderKey + 1, // Đặt OrderKey tiếp theo
                    Hinh_anh = fileName
                };

                context.San_Phams.InsertOnSubmit(newProduct);
                context.SubmitChanges();

                lblMessage.Text = $"✅ Thêm sản phẩm '{newProduct.Ten_san_pham}' thành công. Mã SP: {newProduct.ID_SP}";

                // Reset form
                txtTenSP.Text = string.Empty;
                txtGia.Text = string.Empty;
                txtMoTa.Text = string.Empty;
                ddlAddCategory.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;

                LoadDataProducts(0);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi thêm sản phẩm: " + ex.Message;
            }
        }
        // ------- === SỬA VÀ XÓA SẢN PHẨM (Sử dụng GridView RowCommand) ===---------
        protected void GridViewProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!int.TryParse(e.CommandArgument.ToString(), out int idSp)) return;

            if (e.CommandName == "EditProduct")
            {
                // Hành động SỬA: Load dữ liệu vào Panel
                LoadProductToEditPanel(idSp);
            }
            else if (e.CommandName == "UpdateOrderKey")
            {
                // Hành động CẬP NHẬT ORDER KEY
                UpdateOrderKey(idSp, e.CommandSource as LinkButton);
            }
        }

        private void LoadProductToEditPanel(int idSp)
        {
            var product = context.San_Phams.SingleOrDefault(sp => sp.ID_SP == idSp);

            if (product != null)
            {
                // 1. Load dữ liệu vào các control
                hfProductID.Value = idSp.ToString();
                lblEditProductName.Text = product.Ten_san_pham;
                txtEditTenSP.Text = product.Ten_san_pham;
                txtEditGia.Text = product.Gia_co_ban.ToString();
                txtEditMoTa.Text = product.Mo_ta_san_pham;
                txtEditOrderKey.Text = product.OrderKey.HasValue ? product.OrderKey.Value.ToString() : "";
                chkEditHot.Checked = product.IsHot;
                hfCurrentHinhAnh.Value = product.Hinh_anh;

                // Set DropDownList
                ddlEditCategory.SelectedValue = product.ID_DM.ToString();
                ddlEditStatus.SelectedValue = product.Trang_thai;

                // Load ảnh hiện tại
                imgEditCurrent.ImageUrl = string.IsNullOrEmpty(product.Hinh_anh) ? "" : Page.ResolveUrl(ImageUploadPath + product.Hinh_anh);

                // 2. Hiển thị Panel và cuộn đến đó
                pnlEditProduct.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ScrollEdit", "window.scrollTo(0, document.getElementById('" + pnlEditProduct.ClientID + "').offsetTop);", true);
                lblMessage.Text = $"⚠️ Đang chỉnh sửa sản phẩm ID: {idSp}";
            }
            else
            {
                lblMessage.Text = "⚠️ Không tìm thấy sản phẩm cần sửa.";
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlEditProduct.Visible = false;
            lblMessage.Text = "";
        }

        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(hfProductID.Value, out int idSp))
                {
                    lblMessage.Text = "❌ Lỗi: Không tìm thấy ID sản phẩm cần cập nhật.";
                    return;
                }

                var sanPhamToUpdate = context.San_Phams.SingleOrDefault(sp => sp.ID_SP == idSp);
                if (sanPhamToUpdate == null)
                {
                    lblMessage.Text = "⚠️ Không tìm thấy sản phẩm cần cập nhật trong CSDL.";
                    return;
                }

                string giaCapNhat = txtEditGia.Text.Trim().Replace(",", "");
                if (!decimal.TryParse(giaCapNhat, out decimal newGiaCoBan) ||
                    !int.TryParse(ddlEditCategory.SelectedValue, out int newIdMn) ||
                    !int.TryParse(txtEditOrderKey.Text.Trim(), out int newOrderKey))
                {
                    lblMessage.Text = "❌ Giá, ID Danh mục và Thứ tự phải là số hợp lệ.";
                    return;
                }

                // Xử lý File Upload (Nếu có file mới)
                string fileName = hfCurrentHinhAnh.Value;
                if (fileUploadEditHinhAnh.HasFile)
                {
                    try
                    {
                        string extension = Path.GetExtension(fileUploadEditHinhAnh.FileName).ToLower();
                        if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                        {
                            lblMessage.Text = "❌ Chỉ chấp nhận file hình ảnh (.jpg, .png, .jpeg).";
                            return;
                        }

                        // Xóa file cũ nếu không phải là ảnh mặc định
                        string oldFileName = sanPhamToUpdate.Hinh_anh;
                        if (!string.IsNullOrEmpty(oldFileName) && oldFileName != "0.jpg")
                        {
                            string oldFilePath = Server.MapPath(ImageUploadPath + oldFileName);
                            if (File.Exists(oldFilePath))
                                File.Delete(oldFilePath);
                        }

                        // Lưu file mới
                        fileName = Guid.NewGuid().ToString() + extension;
                        string savePath = Server.MapPath(ImageUploadPath) + fileName;
                        fileUploadEditHinhAnh.SaveAs(savePath);
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "❌ Lỗi khi tải lên hình ảnh mới: " + ex.Message;
                        return;
                    }
                }

                // Cập nhật dữ liệu
                sanPhamToUpdate.Ten_san_pham = txtEditTenSP.Text.Trim();
                sanPhamToUpdate.Gia_co_ban = newGiaCoBan;
                sanPhamToUpdate.Mo_ta_san_pham = txtEditMoTa.Text.Trim();
                sanPhamToUpdate.ID_DM = newIdMn;
                sanPhamToUpdate.Trang_thai = ddlEditStatus.SelectedValue;
                sanPhamToUpdate.IsHot = chkEditHot.Checked;
                sanPhamToUpdate.OrderKey = newOrderKey;
                sanPhamToUpdate.Hinh_anh = fileName;

                context.SubmitChanges();

                lblMessage.Text = $"✅ Cập nhật sản phẩm '{sanPhamToUpdate.Ten_san_pham}' (ID={idSp}) thành công.";
                pnlEditProduct.Visible = false; // Ẩn form sau khi cập nhật
                LoadDataProducts(0);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi cập nhật dữ liệu: " + ex.Message;
            }
        }

        private void UpdateOrderKey(int idSp, LinkButton sender)
        {
            try
            {
                // Lấy TextBox chứa OrderKey nằm trong cùng Row
                GridViewRow row = (GridViewRow)sender.NamingContainer;
                TextBox txtOrderKey = (TextBox)row.FindControl("txtOrderKey");

                if (txtOrderKey == null || !int.TryParse(txtOrderKey.Text, out int newOrderKey))
                {
                    lblMessage.Text = $"⚠️ ID={idSp}: Thứ tự phải là số hợp lệ.";
                    return;
                }

                San_Pham sanPhamToUpdate = context.San_Phams.SingleOrDefault(sp => sp.ID_SP == idSp);
                if (sanPhamToUpdate != null)
                {
                    sanPhamToUpdate.OrderKey = newOrderKey;
                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Cập nhật Thứ tự (ID={idSp}) thành công: {newOrderKey}.";
                    LoadDataProducts(GridViewProducts.PageIndex); // Tải lại trang hiện tại
                }
                else
                {
                    lblMessage.Text = $"⚠️ Không tìm thấy sản phẩm có ID={idSp}.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi cập nhật Thứ tự: " + ex.Message;
            }
        }


        protected void GridViewProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Ẩn form sửa nếu đang hiển thị
                pnlEditProduct.Visible = false;

                int idSp = Convert.ToInt32(GridViewProducts.DataKeys[e.RowIndex].Value);
                San_Pham sanPham = context.San_Phams.SingleOrDefault(sp => sp.ID_SP == idSp);

                if (sanPham != null)
                {
                    // Xóa file ảnh (tránh xóa nếu là ảnh mặc định '0.jpg')
                    if (!string.IsNullOrEmpty(sanPham.Hinh_anh) && sanPham.Hinh_anh != "0.jpg")
                    {
                        string filePath = Server.MapPath(ImageUploadPath + sanPham.Hinh_anh);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }

                    context.San_Phams.DeleteOnSubmit(sanPham);
                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Xóa sản phẩm '{sanPham.Ten_san_pham}' (ID={idSp}) thành công.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Không tìm thấy sản phẩm cần xóa.";
                }
                LoadDataProducts(0);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa sản phẩm: " + ex.Message;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // Ẩn form sửa nếu đang hiển thị
            pnlEditProduct.Visible = false;

            try
            {
                List<San_Pham> productsToDelete = new List<San_Pham>();
                int soSpDaChon = 0;

                for (int i = 0; i < GridViewProducts.Rows.Count; i++)
                {
                    CheckBox chk = (CheckBox)GridViewProducts.Rows[i].FindControl("chkDelete");

                    if (chk != null && chk.Checked)
                    {
                        int idSp = Convert.ToInt32(GridViewProducts.DataKeys[i].Value);
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
                    foreach (var sp in productsToDelete)
                    {
                        // Xóa file ảnh trước
                        if (!string.IsNullOrEmpty(sp.Hinh_anh) && sp.Hinh_anh != "0.jpg")
                        {
                            string filePath = Server.MapPath(ImageUploadPath + sp.Hinh_anh);
                            if (File.Exists(filePath))
                                File.Delete(filePath);
                        }
                    }

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

            LoadDataProducts(0);
        }
    }
}