using System;
using System.Linq;
using System.Web.UI.WebControls;
using Cua_Hang_Tra_Sua;
using System.Collections.Generic;

namespace QL_BAN_HANG
{
    public partial class ProductUser : System.Web.UI.Page
    {
        // --- Cấu hình Phân trang & Trạng thái ---
        private const int PageSize = 9; // 9 sản phẩm mỗi trang

        // Trang hiện tại. Lưu trữ trong ViewState
        public int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                    return 1;
                return (int)ViewState["CurrentPage"];
            }
            set { ViewState["CurrentPage"] = value; }
        }

        // Từ khóa tìm kiếm. Lưu trữ trong ViewState
        public string SearchTerm
        {
            get
            {
                if (ViewState["SearchTerm"] == null)
                    return string.Empty;
                return (string)ViewState["SearchTerm"];
            }
            set { ViewState["SearchTerm"] = value; }
        }

        // ID_DM hiện tại, cần thiết để duy trì trạng thái lọc khi chuyển trang/tìm kiếm
        public int CurrentIdDm
        {
            get
            {
                if (ViewState["CurrentIdDm"] == null)
                    return 0;
                return (int)ViewState["CurrentIdDm"];
            }
            set { ViewState["CurrentIdDm"] = value; }
        }

        // --- Sự kiện Page_Load Cập nhật ---
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Chỉ thiết lập CurrentIdDm từ QueryString trong lần tải đầu tiên
                if (int.TryParse(Request.QueryString["ID_DM"], out int idDm))
                {
                    CurrentIdDm = idDm;
                }
                else
                {
                    CurrentIdDm = 0;
                }

                CurrentPage = 1; // Thiết lập trang mặc định
                SearchTerm = string.Empty; // Thiết lập từ khóa mặc định
                txtSearch.Text = string.Empty; // Đảm bảo TextBox trống khi tải lần đầu

                BindProductData();
            }
        }

        // --- Phương thức Lấy và Liên kết Dữ liệu Tổng hợp ---
        private void BindProductData()
        {
            List<San_Pham> allProducts;

            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var query = context.San_Phams.AsQueryable();

                    // 1. Áp dụng Lọc theo Danh mục (nếu ID_DM > 0)
                    if (CurrentIdDm > 0)
                    {
                        query = query.Where(sp => sp.ID_DM == CurrentIdDm);
                    }

                    // 2. Áp dụng Tìm kiếm (nếu SearchTerm không rỗng)
                    if (!string.IsNullOrWhiteSpace(SearchTerm))
                    {
                        query = query.Where(sp => sp.Ten_san_pham.ToLower().Contains(SearchTerm.ToLower()));
                    }

                    // 3. Sắp xếp và thực hiện truy vấn
                    allProducts = query.OrderBy(sp => sp.Ten_san_pham).ToList();
                }

                // 4. Kiểm tra dữ liệu và thông báo
                if (allProducts.Count == 0)
                {
                    RepeaterProducts.DataSource = null;
                    RepeaterProducts.DataBind();
                    lblMessage.Text = $"Không tìm thấy sản phẩm nào theo yêu cầu.";
                    RepeaterPaging.DataSource = null;
                    RepeaterPaging.DataBind();
                    // Đã loại bỏ lblPagingInfo.Text = string.Empty;
                    return;
                }

                // 5. Thiết lập Phân trang
                PagedDataSource pds = new PagedDataSource
                {
                    DataSource = allProducts,
                    AllowPaging = true,
                    PageSize = PageSize,
                    CurrentPageIndex = CurrentPage - 1
                };

                // 6. Liên kết dữ liệu sản phẩm cho trang hiện tại
                RepeaterProducts.DataSource = pds;
                RepeaterProducts.DataBind();

                // 7. Liên kết dữ liệu cho các nút phân trang
                BindPaging(pds.PageCount);

                // 8. Cập nhật thông báo
                UpdateStatusLabel(allProducts.Count); // Xóa tham số categoryName
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Lỗi tải sản phẩm hoặc kết nối CSDL: " + ex.Message;
                RepeaterProducts.DataSource = null;
                RepeaterProducts.DataBind();
            }
        }

        // Phương thức liên kết dữ liệu cho Repeater phân trang
        private void BindPaging(int totalPages)
        {
            List<int> pages = new List<int>();
            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(i);
            }

            RepeaterPaging.DataSource = pages;
            RepeaterPaging.DataBind();
        }

        // Phương thức cập nhật nhãn thông báo (đã loại bỏ thông tin phân trang)
        private void UpdateStatusLabel(int totalCount)
        {
            string searchString = string.IsNullOrWhiteSpace(SearchTerm) ? "" : $" (tìm kiếm: '{SearchTerm}')";

            lblMessage.Text = $"📦 Đã tải {totalCount} sản phẩm thuộc {searchString}.";
            // Đã loại bỏ dòng cập nhật lblPagingInfo
        }

        // --- Xử lý Sự kiện Tìm kiếm và Phân trang ---

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            SearchTerm = txtSearch.Text.Trim();
            BindProductData();
        }

        protected void RepeaterPaging_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int pageIndex))
                {
                    CurrentPage = pageIndex;
                    BindProductData();
                }
            }
        }

        // --- Giữ lại Logic Thêm Giỏ Hàng (btnAddCart_Click) ---

        protected void BtnAddCart_Click(object sender, EventArgs e)
        {
            try
            {
                string sdt = Session["LoggedInUser"]?.ToString();
                if (string.IsNullOrEmpty(sdt))
                {
                    lblMessage.Text = "❌ Vui lòng đăng nhập trước khi thêm sản phẩm vào giỏ hàng.";
                    return;
                }

                if (!(sender is Button btn)) return;

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