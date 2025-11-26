using Cua_Hang_Tra_Sua;
using System;
using System.IO;
using System.Linq;
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
                LoadDataBaiViet(0);
            }
        }

        // --- Tải dữ liệu ---
        private void LoadDataBaiViet(int page)
        {
            var query = context.Bai_Viets.AsQueryable();

            GridViewBaiViet.DataSource = query
                .OrderBy(bv => bv.OrderKey)              // Thứ tự tăng dần
                .ThenByDescending(bv => bv.ID_BV)        // Nếu trùng OrderKey thì ID_BV giảm dần
                .ToList();
            GridViewBaiViet.PageIndex = page;
            GridViewBaiViet.PageSize = 5;         // số bài mỗi trang
            GridViewBaiViet.DataBind();
        }

        protected void GridViewBaiViet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadDataBaiViet(e.NewPageIndex); // gọi lại hàm load dữ liệu từ DB
        }

        // --- Thêm bài viết ---
        protected void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtAddOrderKey.Text, out int orderKey))
                {
                    lblMessage.Text = "❌ OrderKey phải là số.";
                    return;
                }

                string fileName = null;
                if (fileUploadHinhAnh.HasFile)
                {
                    fileName = Path.GetFileName(fileUploadHinhAnh.FileName);
                    string savePath = Server.MapPath("~/uploads/images/") + fileName;
                    fileUploadHinhAnh.SaveAs(savePath);
                }
                else
                {
                    fileName = "0.jpg"; // Ảnh mặc định nếu không tải lên
                }

                    // Lấy nội dung từ Rich Text Editor và đảm bảo không NULL
                    // Sử dụng toán tử ?? string.Empty để tránh lỗi "Cannot insert the value NULL"
                    string noiDung = Request.Unvalidated["EditorAddNoiDung"] ?? string.Empty;

                Bai_Viet newBaiViet = new Bai_Viet
                {
                    ID_MN = 1, // mặc định luôn là danh mục ID=1
                    Tieu_de = txtAddTieuDe.Text.Trim(),
                    Tom_tac = txtAddTomTat.Text.Trim(),
                    Noi_dung = noiDung, // ĐÃ CẬP NHẬT
                    Hinh_anh_page = fileName,
                    OrderKey = orderKey
                };

                context.Bai_Viets.InsertOnSubmit(newBaiViet);
                context.SubmitChanges();

                lblMessage.Text = $"✅ Thêm bài viết '{newBaiViet.Tieu_de}' thành công.";

                // Reset form
                txtAddTieuDe.Text = string.Empty;
                txtAddTomTat.Text = string.Empty;
                EditorAddNoiDung.Text = string.Empty; // Reset Editor
                txtAddOrderKey.Text = "1";

                LoadDataBaiViet(0);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi thêm bài viết: " + ex.Message;
            }
        }
        protected void GridViewBaiViet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateOrderKey")
            {
                int idBv = Convert.ToInt32(e.CommandArgument);

                // Lấy dòng hiện tại
                GridViewRow row = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                TextBox txtOrderKey = row.FindControl("txtOrderKey") as TextBox;

                int newOrderKey;
                if (!int.TryParse(txtOrderKey.Text.Trim(), out newOrderKey))
                {
                    // Báo lỗi nếu nhập không phải số
                    lblMessage.Text = "⚠️ OrderKey phải là số. Vui lòng nhập lại.";
                    return;
                }

                // Cập nhật DB
                var bv = context.Bai_Viets.SingleOrDefault(b => b.ID_BV == idBv);
                if (bv != null)
                {
                    bv.OrderKey = int.Parse(newOrderKey.ToString());
                    context.SubmitChanges();
                    lblMessage.Text = "✅ Đã cập nhật OrderKey thành công.";
                }

                // Refresh lại GridView
                LoadDataBaiViet(0);
            }
        }

        // --- Xóa bài viết ---
        protected void GridViewBaiViet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int idBv = Convert.ToInt32(GridViewBaiViet.DataKeys[e.RowIndex].Value);
                Bai_Viet baiViet = context.Bai_Viets.SingleOrDefault(bv => bv.ID_BV == idBv);

                if (baiViet != null)
                {
                    DeleteImageFile(baiViet.Hinh_anh_page);
                    context.Bai_Viets.DeleteOnSubmit(baiViet);
                    context.SubmitChanges();
                    lblMessage.Text = $"✅ Xóa bài viết ID={idBv} thành công.";
                }
                else
                {
                    lblMessage.Text = "⚠️ Không tìm thấy bài viết cần xóa.";
                }

                LoadDataBaiViet(0);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi xóa bài viết: " + ex.Message;
            }
        }

        // --- Hỗ trợ xóa file ảnh ---
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
                        System.Diagnostics.Debug.WriteLine("Lỗi xóa file ảnh: " + ex.Message);
                    }
                }
            }
        }
    }
}