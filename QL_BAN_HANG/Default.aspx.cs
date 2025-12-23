using Cua_Hang_Tra_Sua;
using System;
using System.Linq;
using System.Web;

namespace QL_BAN_HANG
{
    public partial class Default : System.Web.UI.Page
    {
        private int currentIdBv = 0;
        private int currentPage = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Xử lý query string cho ID_BV
                if (int.TryParse(Request.QueryString["ID_BV"], out currentIdBv))
                {
                    LoadBaiVietTheoId(currentIdBv);
                    Session["ID_BV"] = currentIdBv; // Lưu vào Session
                }
                else
                {
                    var baiVietHot = LoadBaiVietHot();
                    if (baiVietHot != null)
                    {
                        currentIdBv = baiVietHot.ID_BV;
                        Session["ID_BV"] = currentIdBv; // Lưu vào Session
                        baiVietHot.ID_BV = currentIdBv;
                    }
                }

                // Xử lý phân trang cho Repeater
                if (int.TryParse(Request.QueryString["page"], out currentPage) && currentPage < 1)
                    currentPage = 1;

                // Nếu không có ID_BV trong query thì lấy từ Session
                if (currentIdBv == 0 && Session["ID_BV"] != null)
                    currentIdBv = (int)Session["ID_BV"];

                LoadNews(currentPage, currentIdBv);
            }
        }


        private void LoadBaiVietTheoId(int idBv)
        {
            using (var db = new Cua_Hang_Tra_SuaDataContext())
            {
                var baiViet = db.Bai_Viets.SingleOrDefault(b => b.ID_BV == idBv);
                HienThiBaiViet(baiViet);
            }
        }

        private Bai_Viet LoadBaiVietHot()
        {
            using (var db = new Cua_Hang_Tra_SuaDataContext())
            {
                var baiViet = db.Bai_Viets
                    .Where(b => b.OrderKey == 1)
                    .OrderByDescending(b => b.ID_BV)
                    .FirstOrDefault();
                HienThiBaiViet(baiViet);
                return baiViet;
            }
        }

        private void HienThiBaiViet(Bai_Viet baiViet)
        {
            if (baiViet != null)
            {
                Label1.Text = baiViet.Tieu_de;
                Label2.Text = baiViet.Tom_tac;
                if (!string.IsNullOrEmpty(baiViet.Noi_dung))
                {
                    litResult.Text = baiViet.Noi_dung;
                }
                else
                {
                    litResult.Text = "<i style='color:red;'>Nội dung trống!</i>";
                }
                Image1.ImageUrl = string.IsNullOrEmpty(baiViet.Hinh_anh_page)
                    ? "https://placehold.co/600x400/e2e8f0/64748b?text=No+Image"
                    : "~/uploads/images/" + baiViet.Hinh_anh_page;
            }
            else
            {
                Label1.Text = "Không tìm thấy bài viết";
                Label2.Text = "";
                litResult.Text = "";
                Image1.ImageUrl = "https://placehold.co/600x400/e2e8f0/64748b?text=No+Image";
            }
        }

        private void LoadNews(int currentPage, int excludeId)
        {
            using (var db = new Cua_Hang_Tra_SuaDataContext())
            {
                if (currentPage <= 0) currentPage = 1;

                int pageSize = 6; // Số bài viết mỗi trang

                // Tổng số bài viết (loại trừ bài viết đang hiển thị)
                int totalItems = db.Bai_Viets
                    .Where(b => b.ID_BV != excludeId)
                    .Count();

                // Tính tổng số trang
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                // Lấy dữ liệu theo trang
                var newsList = db.Bai_Viets
                    .Where(b => b.ID_BV != excludeId) // Loại trừ bài viết đang hiển thị
                    .OrderBy(bv => bv.OrderKey)       // Thứ tự tăng dần
                    .ThenByDescending(bv => bv.ID_BV)
                    .Select(b => new
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
        }

        private string GeneratePagingLinks(int totalPages, int currentPage)
        {
            // 1. Ưu tiên lấy ID từ Session vì Page_Load đã đảm bảo Session luôn có dữ liệu (ID_BV hot hoặc ID từ URL)
            int idToLink = 0;
            if (Session["ID_BV"] != null)
            {
                idToLink = (int)Session["ID_BV"];
            }
            else
            {
                // 2. Backup: Nếu Session trống, thử lấy từ QueryString
                int.TryParse(Request.QueryString["ID_BV"], out idToLink);
            }

            string links = "";
            for (int i = 1; i <= totalPages; i++)
            {
                if (i == currentPage)
                {
                    links += $"<span class='px-3 py-1 bg-[#4c673d] text-white rounded-full mx-1'>{i}</span>";
                }
                else
                {
                    // Nếu idToLink vẫn bằng 0 (trường hợp hy hữu), link sẽ gọn hơn, 
                    // nhưng thường Session sẽ luôn có giá trị sau khi Page_Load chạy.
                    string url = idToLink > 0 ? $"?ID_BV={idToLink}&page={i}" : $"?page={i}";

                    links += $"<a href='{url}' class='px-3 py-1 border border-[#4c673d] text-[#4c673d] rounded-full mx-1 hover:bg-[#4c673d] hover:text-white transition'>{i}</a>";
                }
            }
            return links;
        }

    }
}
