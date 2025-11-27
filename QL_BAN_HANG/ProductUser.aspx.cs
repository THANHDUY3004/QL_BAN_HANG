using System;
using System.Linq;
using System.Web.UI.WebControls;
using Cua_Hang_Tra_Sua;

namespace QL_BAN_HANG
{
    public partial class ProductUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idDm = 0;
                if (int.TryParse(Request.QueryString["ID_DM"], out idDm))
                {
                    LoadProductsByMenu(idDm);
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

                    RepeaterProducts.DataSource = allProducts;
                    RepeaterProducts.DataBind();
                    lblMessage.Text = $"📦 Đã tải toàn bộ {allProducts.Count} sản phẩm";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải sản phẩm: " + ex.Message;
            }
        }

        private void LoadProductsByMenu(int idDm)
        {
            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var products = context.San_Phams
                                          .Where(sp => sp.ID_DM == idDm)
                                          .OrderBy(sp => sp.Ten_san_pham)
                                          .ToList();

                    RepeaterProducts.DataSource = products;
                    RepeaterProducts.DataBind();
                    if (idDm == 3) 
                    {
                        lblMessage.Text = $"📦 Đã tải {products.Count} sản phẩm thuộc menu Trà Sữa";
                    }
                    else if(idDm == 4) 
                    {
                        lblMessage.Text = $"📦 Đã tải {products.Count} sản phẩm thuộc menu Trà Trái Cây";
                    }
                    else if(idDm == 5)
                    {
                        lblMessage.Text = $"📦 Đã tải {products.Count} sản phẩm thuộc menu Bánh Ngọt";
                    }
                    else
                    {
                        lblMessage.Text = $"📦 Đã tải toàn bộ {products.Count} sản phẩm";

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi tải sản phẩm theo menu: " + ex.Message;
            }
        }

        protected void btnAddCart_Click(object sender, EventArgs e)
        {
            try
            {
                string sdt = Session["LoggedInUser"]?.ToString();
                if (string.IsNullOrEmpty(sdt))
                {
                    lblMessage.Text = "❌ Vui lòng đăng nhập trước khi thêm sản phẩm vào giỏ hàng.";
                    return;
                }

                Button btn = sender as Button;
                if (btn == null) return;

                if (!int.TryParse(btn.CommandArgument, out int idSp))
                {
                    lblMessage.Text = "❌ ID sản phẩm không hợp lệ.";
                    return;
                }

                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var sanPham = context.San_Phams.FirstOrDefault(sp => sp.ID_SP == idSp);
                    if (sanPham == null)
                    {
                        lblMessage.Text = "❌ Không tìm thấy sản phẩm.";
                        return;
                    }

                    var gioHangItem = context.Gio_Hangs.FirstOrDefault(g => g.So_dien_thoai == sdt && g.ID_SP == idSp);

                    if (gioHangItem != null)
                    {
                        gioHangItem.So_luong += 1;
                        lblMessage.Text = $"🔄 Đã cập nhật số lượng sản phẩm {sanPham.Ten_san_pham} trong giỏ hàng.";
                    }
                    else
                    {
                        Gio_Hang newItem = new Gio_Hang
                        {
                            So_dien_thoai = sdt,
                            ID_SP = sanPham.ID_SP,
                            So_luong = 1,
                            Gia_luc_them = sanPham.Gia_co_ban,
                            Ghi_chu = "",
                            Ngay_them = DateTime.Now
                        };
                        context.Gio_Hangs.InsertOnSubmit(newItem);
                        lblMessage.Text = $"✅ Đã thêm sản phẩm {sanPham.Ten_san_pham} vào giỏ hàng.";
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi khi thêm sản phẩm vào giỏ hàng: " + ex.Message;
            }
        }
    }
}
