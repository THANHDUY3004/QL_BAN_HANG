using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cua_Hang_Tra_Sua; // namespace của LINQ to SQL .dbml
using System.IO;

namespace QL_BAN_HANG
{
    public partial class HomePage_admin : System.Web.UI.Page
    {
        private Cua_Hang_Tra_SuaDataContext db = new Cua_Hang_Tra_SuaDataContext();

        // ĐỊNH NGHĨA CHUNG ĐƯỜNG DẪN LƯU TRỮ HÌNH ẢNH
        private const string ImageUploadPath = "~/uploads/images/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSliderImages();
                LoadShopImages();
            }
        }

        #region === SLIDER IMAGES MANAGEMENT ===

        private void LoadSliderImages()
        {
            var sliderImages = from s in db.Hinh_Anh_Sliders
                               orderby s.OrderKey
                               select s;
            gvSlider.DataSource = sliderImages.ToList();
            gvSlider.DataBind();
        }

        protected void btnCancelSlider_Click(object sender, EventArgs e)
        {
            ClearSliderForm();
        }

        protected void btnShowSliderForm_Click(object sender, EventArgs e)
        {
            ClearSliderForm();
            pnlSliderForm.Visible = true;
        }

        protected void gvSlider_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!int.TryParse(e.CommandArgument.ToString(), out int id)) return;

            if (e.CommandName == "EditSlider")
            {
                var slider = db.Hinh_Anh_Sliders.SingleOrDefault(s => s.ID == id);
                if (slider != null)
                {
                    txtSliderTitle.Text = slider.Title;
                    txtSliderDesc.Text = slider.Description;
                    txtSliderOrder.Text = slider.OrderKey.HasValue ? slider.OrderKey.Value.ToString() : "";
                    chkSliderActive.Checked = slider.IsActive;
                    hfSliderID.Value = id.ToString();
                    pnlSliderForm.Visible = true;
                    btnAddSlider.Text = "Cập Nhật";
                }
            }
            else if (e.CommandName == "DeleteSlider")
            {
                var slider = db.Hinh_Anh_Sliders.SingleOrDefault(s => s.ID == id);
                if (slider != null)
                {
                    // DÙNG CHUNG ImageUploadPath
                    string filePath = Server.MapPath(ImageUploadPath + slider.ImageUrl);
                    if (!string.IsNullOrEmpty(slider.ImageUrl) && File.Exists(filePath))
                        File.Delete(filePath);

                    db.Hinh_Anh_Sliders.DeleteOnSubmit(slider);
                    db.SubmitChanges();
                    LoadSliderImages();
                }
            }
        }

        protected void btnAddSlider_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSliderOrder.Text, out int orderKey))
            {
                // TODO: Thêm thông báo lỗi cho OrderKey không hợp lệ
                return;
            }

            string currentSliderID = hfSliderID.Value;
            bool isUpdating = !string.IsNullOrEmpty(currentSliderID);

            if (!isUpdating && !fuSlider.HasFile)
            {
                // TODO: Thêm thông báo lỗi: Chưa chọn file
                return;
            }

            string fileName = null;

            // Xử lý Upload File
            if (fuSlider.HasFile)
            {
                fileName = Guid.NewGuid() + Path.GetExtension(fuSlider.FileName);
                // DÙNG CHUNG ImageUploadPath
                string savePath = Server.MapPath(ImageUploadPath + fileName);

                if (!Directory.Exists(Server.MapPath(ImageUploadPath)))
                    Directory.CreateDirectory(Server.MapPath(ImageUploadPath));

                try
                {
                    fuSlider.SaveAs(savePath);
                }
                catch (Exception)
                {
                    // TODO: Ghi log lỗi và thông báo cho người dùng
                    return;
                }
            }

            if (isUpdating)
            {
                // Update
                int id = Convert.ToInt32(currentSliderID);
                var slider = db.Hinh_Anh_Sliders.SingleOrDefault(s => s.ID == id);
                if (slider != null)
                {
                    if (fileName != null) // Có file mới được upload
                    {
                        // Xóa file cũ trước (DÙNG CHUNG ImageUploadPath)
                        string oldFilePath = Server.MapPath(ImageUploadPath + slider.ImageUrl);
                        if (!string.IsNullOrEmpty(slider.ImageUrl) && File.Exists(oldFilePath))
                            File.Delete(oldFilePath);

                        slider.ImageUrl = fileName;
                    }

                    slider.Title = txtSliderTitle.Text;
                    slider.Description = txtSliderDesc.Text;
                    slider.OrderKey = orderKey;
                    slider.IsActive = chkSliderActive.Checked;
                    db.SubmitChanges();
                }
            }
            else
            {
                // Insert
                var newSlider = new Hinh_Anh_Slider
                {
                    ImageUrl = fileName,
                    Title = txtSliderTitle.Text,
                    Description = txtSliderDesc.Text,
                    OrderKey = orderKey,
                    IsActive = chkSliderActive.Checked
                };
                db.Hinh_Anh_Sliders.InsertOnSubmit(newSlider);
                db.SubmitChanges();
            }

            ClearSliderForm();
            LoadSliderImages();
        }

        private void ClearSliderForm()
        {
            txtSliderTitle.Text = "";
            txtSliderDesc.Text = "";
            txtSliderOrder.Text = "";
            chkSliderActive.Checked = false;
            hfSliderID.Value = "";
            pnlSliderForm.Visible = false;
            btnAddSlider.Text = "Thêm Mới";
        }

        #endregion

        // ----------------------------------------------------------------------------------

        #region === SHOP IMAGES MANAGEMENT ===

        private void LoadShopImages()
        {
            var shopImages = from h in db.Hinh_Anh_Quans
                             orderby h.OrderKey
                             select h;
            gvShop.DataSource = shopImages.ToList();
            gvShop.DataBind();
        }

        protected void btnCancelShop_Click(object sender, EventArgs e)
        {
            ClearShopForm();
        }

        protected void btnShowShopForm_Click(object sender, EventArgs e)
        {
            ClearShopForm();
            pnlShopForm.Visible = true;
        }

        protected void gvShop_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!int.TryParse(e.CommandArgument.ToString(), out int id)) return;

            if (e.CommandName == "EditShop")
            {
                var shop = db.Hinh_Anh_Quans.SingleOrDefault(h => h.ID == id);
                if (shop != null)
                {
                    txtShopAlt.Text = shop.AltText;
                    txtShopOrder.Text = shop.OrderKey.HasValue ? shop.OrderKey.Value.ToString() : "";
                    chkShopActive.Checked = shop.IsActive;
                    hfShopID.Value = id.ToString();
                    pnlShopForm.Visible = true;
                    btnAddShop.Text = "Cập Nhật";
                }
            }
            else if (e.CommandName == "DeleteShop")
            {
                var shop = db.Hinh_Anh_Quans.SingleOrDefault(h => h.ID == id);
                if (shop != null)
                {
                    // DÙNG CHUNG ImageUploadPath
                    string filePath = Server.MapPath(ImageUploadPath + shop.ImageUrl);
                    if (!string.IsNullOrEmpty(shop.ImageUrl) && File.Exists(filePath))
                        File.Delete(filePath);

                    db.Hinh_Anh_Quans.DeleteOnSubmit(shop);
                    db.SubmitChanges();
                    LoadShopImages();
                }
            }
        }

        protected void btnAddShop_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtShopOrder.Text, out int orderKey))
            {
                // TODO: Thêm thông báo lỗi cho OrderKey không hợp lệ
                return;
            }

            string currentShopID = hfShopID.Value;
            bool isUpdating = !string.IsNullOrEmpty(currentShopID);

            if (!isUpdating && !fuShop.HasFile)
            {
                // TODO: Thêm thông báo lỗi: Chưa chọn file
                return;
            }

            string fileName = null;
            if (fuShop.HasFile)
            {
                fileName = Guid.NewGuid() + Path.GetExtension(fuShop.FileName);
                // DÙNG CHUNG ImageUploadPath
                string savePath = Server.MapPath(ImageUploadPath + fileName);

                if (!Directory.Exists(Server.MapPath(ImageUploadPath)))
                    Directory.CreateDirectory(Server.MapPath(ImageUploadPath));

                try
                {
                    fuShop.SaveAs(savePath);
                }
                catch (Exception)
                {
                    // TODO: Ghi log lỗi và thông báo cho người dùng
                    return;
                }
            }

            if (isUpdating)
            {
                // Update
                int id = Convert.ToInt32(currentShopID);
                var shop = db.Hinh_Anh_Quans.SingleOrDefault(h => h.ID == id);
                if (shop != null)
                {
                    if (fileName != null) // Có file mới được upload
                    {
                        // Xóa file cũ trước (DÙNG CHUNG ImageUploadPath)
                        string oldFilePath = Server.MapPath(ImageUploadPath + shop.ImageUrl);
                        if (!string.IsNullOrEmpty(shop.ImageUrl) && File.Exists(oldFilePath))
                            File.Delete(oldFilePath);

                        shop.ImageUrl = fileName;
                    }

                    shop.AltText = txtShopAlt.Text;
                    shop.OrderKey = orderKey;
                    shop.IsActive = chkShopActive.Checked;
                    db.SubmitChanges();
                }
            }
            else
            {
                // Insert
                var newShop = new Hinh_Anh_Quan
                {
                    ImageUrl = fileName,
                    AltText = txtShopAlt.Text,
                    OrderKey = orderKey,
                    IsActive = chkShopActive.Checked
                };
                db.Hinh_Anh_Quans.InsertOnSubmit(newShop);
                db.SubmitChanges();
            }

            ClearShopForm();
            LoadShopImages();
        }

        private void ClearShopForm()
        {
            txtShopAlt.Text = "";
            txtShopOrder.Text = "";
            chkShopActive.Checked = false;
            hfShopID.Value = "";
            pnlShopForm.Visible = false;
            btnAddShop.Text = "Thêm Mới";
        }

        #endregion
    }
}