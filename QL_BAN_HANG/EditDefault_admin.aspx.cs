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
                NoiDung.Text = bv.Noi_dung;
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
            // Kiểm tra rỗng trước
            if (string.IsNullOrWhiteSpace(txtTieuDe.Text))
            {
                lblMessage.Text = "❌ Tiêu đề không được để trống.";
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTomTat.Text))
            {
                lblMessage.Text = "❌ Tóm tắt không được để trống.";
                return;
            }
            if (string.IsNullOrWhiteSpace(NoiDung.Text))
            {
                lblMessage.Text = "❌ Nội dung không được để trống.";
                return;
            }
            if (string.IsNullOrWhiteSpace(txtOrderKey.Text))
            {
                lblMessage.Text = "❌ OrderKey không được để trống.";
                return;
            }

            var bv = context.Bai_Viets.SingleOrDefault(b => b.ID_BV == idBv);
            if (bv != null)
            {
                // Kiểm tra OrderKey phải là số
                if (!int.TryParse(txtOrderKey.Text.Trim(), out int orderKeyValue))
                {
                    lblMessage.Text = "⚠️ OrderKey phải là số. Vui lòng nhập lại.";
                    return;
                }

                bv.OrderKey = orderKeyValue;
                bv.Tieu_de = txtTieuDe.Text.Trim();
                bv.Tom_tac = txtTomTat.Text.Trim();
                bv.Noi_dung = NoiDung.Text.Trim();

                // Upload ảnh nếu có (giữ nguyên phần kiểm tra định dạng/dung lượng như đã viết)
                if (fileUploadHinhAnh.HasFile)
                {
                    try
                    {
                        string extension = Path.GetExtension(fileUploadHinhAnh.FileName).ToLower();
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                        if (!allowedExtensions.Contains(extension))
                        {
                            lblMessage.Text = "⚠️ Chỉ được phép upload file ảnh (.jpg, .jpeg, .png, .gif).";
                            return;
                        }

                        int fileSize = fileUploadHinhAnh.PostedFile.ContentLength;
                        if (fileSize > 5 * 1024 * 1024)
                        {
                            lblMessage.Text = "⚠️ Dung lượng ảnh vượt quá 5MB.";
                            return;
                        }

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

                        fileUploadHinhAnh.SaveAs(savePath);
                        bv.Hinh_anh_page = fileName;

                        imgPreview.ImageUrl = "~/uploads/images/" + fileName + "?v=" + DateTime.Now.Ticks;
                        imgPreview.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "⚠️ Lỗi khi tải lên hình ảnh: " + ex.Message;
                        return;
                    }
                }

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