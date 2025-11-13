using Cua_Hang_Tra_Sua;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class ProductUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Lấy tham số ID_MN từ URL
                string idMn = Request.QueryString["ID_MN"];
                if (!string.IsNullOrEmpty(idMn))
                {
                    Session["ID_MN"] = idMn.Trim();
                    LoadDataProducts();
                }
                else
                {
                    LoadAllProducts();
                }
            }
        }
        private void LoadAllProducts()
        {
            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var allProducts = context.San_Phams
                                             .OrderBy(sp => sp.Ten_san_pham)
                                             .ToList();

                    GridViewProducts.DataSource = allProducts;
                    GridViewProducts.DataBind();
                    lblMessage.Text = $"📦 Đã tải toàn bộ {allProducts.Count} sản phẩm";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải toàn bộ sản phẩm: " + ex.Message;
            }
        }

        // 📌 READ: Hiển thị danh sách sản phẩm theo ID_MN
        private void LoadDataProducts()
        {
            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    string idMn = Session["ID_MN"]?.ToString();
                    if (string.IsNullOrEmpty(idMn)) return;

                    // ép kiểu sang int vì ID_MN là khóa số
                    if (int.TryParse(idMn, out int menuId))
                    {
                        var query = context.San_Phams
                                           .Where(sp => sp.ID_MN == menuId)
                                           .OrderBy(sp => sp.Ten_san_pham)
                                           .ToList();

                        GridViewProducts.DataSource = query;
                        GridViewProducts.DataBind();
                        lblMessage.Text = $"✅ Đã tải {query.Count} sản phẩm theo menu ID = {menuId}";
                    }
                    else
                    {
                        lblMessage.Text = "❌ ID_MN không hợp lệ!";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải danh sách sản phẩm: " + ex.Message;
            }
        }
    }
}
