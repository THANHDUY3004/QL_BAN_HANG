using System;
using System.Linq;
using System.Web.UI;
using Cua_Hang_Tra_Sua; // namespace của LINQ to SQL .dbml

namespace QL_BAN_HANG
{
    public partial class HomePage : System.Web.UI.Page
    {
        private Cua_Hang_Tra_SuaDataContext db = new Cua_Hang_Tra_SuaDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSliderImages();
                LoadNews();
                LoadFeaturedProducts();
                LoadShopImages();
            }
        }

        private void LoadSliderImages()
        {
            var sliderImages = from s in db.Hinh_Anh_Sliders
                               where s.IsActive == true
                               orderby s.OrderKey
                               select new { s.ImageUrl, s.Title, s.Description };
            rptSlider.DataSource = sliderImages.ToList();
            rptSlider.DataBind();
            rptSliderDots.DataSource = sliderImages.ToList();
            rptSliderDots.DataBind();
        }

        private void LoadNews()
        {
            // Lấy số trang hiện tại từ QueryString, mặc định là 1
            int currentPage = 1;
            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out currentPage);
                if (currentPage <= 0) currentPage = 1;
            }

            int pageSize = 6; // số bài viết mỗi trang

            // Tổng số bài viết
            int totalItems = db.Bai_Viets.Count();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Lấy dữ liệu theo trang
            var newsList = (from b in db.Bai_Viets.
                            OrderBy(bv => bv.OrderKey)              // Thứ tự tăng dần
                            .ThenByDescending(bv => bv.ID_BV)
                            select new
                            {
                                b.ID_BV,
                                b.Tieu_de,
                                b.Tom_tac,
                                b.Hinh_anh_page
                            })
                            .Skip((currentPage - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            // Bind dữ liệu vào Repeater
            rptNews.DataSource = newsList;
            rptNews.DataBind();

            // Tạo liên kết phân trang
            lblPaging.Text = GeneratePagingLinks(totalPages, currentPage);
        }

        private string GeneratePagingLinks(int totalPages, int currentPage)
        {
            string links = "";
            for (int i = 1; i <= totalPages; i++)
            {
                if (i == currentPage)
                    links += $"<span class='px-3 py-1 bg-[#4c673d] text-white rounded-full mx-1'>{i}</span>";
                else
                    links += $"<a href='?page={i}' class='px-3 py-1 border border-[#4c673d] text-[#4c673d] rounded-full mx-1 hover:bg-[#4c673d] hover:text-white transition'>{i}</a>";
            }
            return links;
        }


        private void LoadFeaturedProducts()
        {
            var featuredProducts = db.San_Phams
                                     .Where(sp => sp.Trang_thai == "Còn Hàng") // chỉ lấy sản phẩm còn hàng
                                     .OrderByDescending(sp => sp.IsHot)        // ưu tiên Hot trước
                                     .ThenBy(sp => sp.OrderKey)                // sau đó sắp xếp theo OrderKey
                                     .Take(5)                                  // lấy 5 sản phẩm
                                     .Select(sp => new
                                     {
                                         TenSanPham = sp.Ten_san_pham,
                                         MoTa = sp.Mo_ta_san_pham,
                                         HinhAnh = sp.Hinh_anh,
                                         IsHot = sp.IsHot
                                     })
                                     .ToList();

            rptFeaturedProducts.DataSource = featuredProducts;
            rptFeaturedProducts.DataBind();
        }



        private void LoadShopImages()
        {
            var shopImages = from h in db.Hinh_Anh_Quans
                             where h.IsActive == true
                             orderby h.OrderKey
                             select new
                             {
                                 h.ImageUrl,
                                 h.AltText
                             };
            rptShopImages.DataSource = shopImages.ToList();
            rptShopImages.DataBind();
        }
    }
}
