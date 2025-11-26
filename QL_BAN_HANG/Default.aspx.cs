using Cua_Hang_Tra_Sua;
using System;
using System.Linq;

namespace QL_BAN_HANG
{
    public partial class Default : System.Web.UI.Page
    {
        private int currentIdBv = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["ID_BV"], out currentIdBv))
                {
                    LoadBaiVietTheoId(currentIdBv);
                }
                else
                {
                    var baiVietHot = LoadBaiVietHot();
                    if (baiVietHot != null)
                        currentIdBv = baiVietHot.ID_BV;
                }

                LoadDanhSachBaiViet(currentIdBv, 0); // loại bỏ bài viết đang hiển thị
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

        private void LoadDanhSachBaiViet(int excludeId, int page)
        {
            using (var db = new Cua_Hang_Tra_SuaDataContext())
            {
                var ds = db.Bai_Viets
                           .Where(b => b.ID_BV != excludeId)
                           .OrderByDescending(b => b.ID_BV)
                           .ToList();

                GridView1.AllowPaging = true;   // bật phân trang
                GridView1.PageSize = 5;         // số bài mỗi trang
                GridView1.PageIndex = page;     // trang hiện tại
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
        }


        private void HienThiBaiViet(Bai_Viet baiViet)
        {
            if (baiViet != null)
            {
                Tieu_de_hot.Text = baiViet.Tieu_de;
                Tom_tac_hot.Text = baiViet.Tom_tac;
                Noi_dung_hot.Text = baiViet.Noi_dung;
                Image_hot.ImageUrl = string.IsNullOrEmpty(baiViet.Hinh_anh_page)
                    ? ""
                    : "~/uploads/images/" + baiViet.Hinh_anh_page;
            }
            else
            {
                Tieu_de_hot.Text = "Không tìm thấy bài viết";
                Tom_tac_hot.Text = "";
                Noi_dung_hot.Text = "";
                Image_hot.ImageUrl = "";
            }
        }

        protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadDanhSachBaiViet(currentIdBv, e.NewPageIndex);
        }
    }
}
