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

        protected void btnAddCart_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra người dùng đã đăng nhập chưa
                string sdt = Session["LoggedInUser"]?.ToString();
                if (string.IsNullOrEmpty(sdt))
                {
                    lblMessage.Text = "❌ Vui lòng đăng nhập trước khi thêm sản phẩm vào giỏ hàng.";
                    return;
                }

                // Lấy ID sản phẩm từ CommandArgument của nút bấm
                Button btn = sender as Button;
                if (btn == null) return;

                int idSp;
                if (!int.TryParse(btn.CommandArgument, out idSp))
                {
                    lblMessage.Text = "❌ ID sản phẩm không hợp lệ.";
                    return;
                }

                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    // Kiểm tra sản phẩm có tồn tại không
                    var sanPham = context.San_Phams.FirstOrDefault(sp => sp.ID_SP == idSp);
                    if (sanPham == null)
                    {
                        lblMessage.Text = "❌ Không tìm thấy sản phẩm.";
                        return;
                    }

                    // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
                    var gioHangItem = context.Gio_Hangs.FirstOrDefault(g => g.So_dien_thoai == sdt && g.ID_SP == idSp);

                    if (gioHangItem != null)
                    {
                        // Nếu đã có thì tăng số lượng
                        gioHangItem.So_luong += 1;
                        lblMessage.Text = $"🔄 Đã cập nhật số lượng sản phẩm {sanPham.Ten_san_pham} trong giỏ hàng.";
                    }
                    else
                    {
                        // Nếu chưa có thì thêm mới
                        Gio_Hang newItem = new Gio_Hang
                        {
                            So_dien_thoai = sdt,
                            ID_SP = sanPham.ID_SP,
                            So_luong = 1,
                            Gia_tai_thoi_diem = sanPham.Gia_co_ban,
                            Ghi_chu = "",
                            Ngay_them = DateTime.Now // ✅ Bắt buộc phải gán để tránh lỗi SqlDateTime overflow
                        };
                        context.Gio_Hangs.InsertOnSubmit(newItem);
                        lblMessage.Text = $"✅ Đã thêm sản phẩm {sanPham.Ten_san_pham} vào giỏ hàng.";
                    }

                    // Lưu thay đổi vào DB
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
