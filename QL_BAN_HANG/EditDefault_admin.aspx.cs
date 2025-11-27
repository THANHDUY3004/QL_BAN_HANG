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
    public partial class EditDefault_admin : System.Web.UI.Page
    {
        private readonly Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext();
        private int idBv;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Luôn gán ID_BV từ query string
            int.TryParse(Request.QueryString["ID_BV"], out idBv);

            if (!IsPostBack)
            {
                if (idBv > 0)
                {
                    LoadBaiViet(idBv);
                }
                else
                {
                    lblMessage.Text = "⚠️ Thiếu ID_BV trên đường dẫn.";
                }
            }
        }


        private void LoadBaiViet(int id)
        {
            var bv = context.Bai_Viets.SingleOrDefault(b => b.ID_BV == id);
            if (bv != null)
            {
                txtOrderKey.Text = bv.OrderKey.ToString();
                txtTieuDe.Text = bv.Tieu_de;
                txtTomTat.Text = bv.Tom_tac;
                EditorNoiDung.Text = bv.Noi_dung;
                if (!string.IsNullOrEmpty(bv.Hinh_anh_page))
                {
                    imgPreview.ImageUrl = "~/uploads/images/" + bv.Hinh_anh_page;
                    imgPreview.Visible = true;
                }
                else
                {
                    imgPreview.Visible = false;
                }
            }
            else
            {
                lblMessage.Text = "⚠️ Không tìm thấy bài viết với ID_BV đã cho.";
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            var bv = context.Bai_Viets.SingleOrDefault(b => b.ID_BV == idBv);
            if (bv != null)
            {
                // Kiểm tra OrderKey phải là số
                if (!int.TryParse(txtOrderKey.Text.Trim(), out int orderKeyValue))
                {
                    lblMessage.Text = "⚠️ OrderKey phải là số. Vui lòng nhập lại.";
                    return; // Dừng không cập nhật
                }

                // Nếu hợp lệ thì gán vào DB
                bv.OrderKey = orderKeyValue;  // Không cần int.Parse lại, đã có orderKeyValue

                // Cập nhật nội dung text
                bv.Tieu_de = txtTieuDe.Text.Trim();
                bv.Tom_tac = txtTomTat.Text.Trim();

                // Lấy nội dung từ Rich Text Editor (dùng .Text thay vì Request.Unvalidated)
                bv.Noi_dung = EditorNoiDung.Text.Trim();  // Đảm bảo lấy giá trị đã cập nhật từ control

                // Upload ảnh nếu có (phần này không thay đổi)
                if (fileUploadHinhAnh.HasFile)
                {
                    try
                    {
                        string fileName = Path.GetFileName(fileUploadHinhAnh.FileName);
                        string folderPath = Server.MapPath("~/uploads/images/");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string savePath = Path.Combine(folderPath, fileName);

                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(bv.Hinh_anh_page))
                        {
                            string oldPath = Path.Combine(folderPath, bv.Hinh_anh_page);
                            if (File.Exists(oldPath))
                            {
                                try { File.Delete(oldPath); } catch { }
                            }
                        }

                        // Lưu ảnh mới
                        fileUploadHinhAnh.SaveAs(savePath);

                        // Cập nhật tên file vào DB
                        bv.Hinh_anh_page = fileName;

                        // Hiển thị lại ảnh mới (chống cache)
                        imgPreview.ImageUrl = "~/uploads/images/" + fileName + "?v=" + DateTime.Now.Ticks;
                        imgPreview.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "⚠️ Lỗi khi tải lên hình ảnh: " + ex.Message;
                        return;
                    }
                }

                // Lưu thay đổi vào DB
                context.SubmitChanges();
                lblMessage.Text = "✅ Đã cập nhật bài viết thành công.";
            }
            else
            {
                lblMessage.Text = "⚠️ Không tìm thấy bài viết cần cập nhật.";
            }
        }



        protected void BtnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default_admin.aspx");
        }
    }
}