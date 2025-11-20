using Cua_Hang_Tra_Sua;
using System;
using System.Linq;

namespace QL_BAN_HANG
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idBv = 0;
                if (int.TryParse(Request.QueryString["ID_BV"], out idBv))
                {
                    LoadBaiVietTheoId(idBv);
                }
                else
                {
                    LoadBaiVietHot();
                }
            }
        }


        private void LoadBaiVietTheoId(int idBv)
        {
            using (Cua_Hang_Tra_SuaDataContext db = new Cua_Hang_Tra_SuaDataContext())
            {
                var baiViet = db.Bai_Viets.SingleOrDefault(b => b.ID_BV == idBv);
                HienThiBaiViet(baiViet);
            }
        }

        private void LoadBaiVietHot()
        {
            using (Cua_Hang_Tra_SuaDataContext db = new Cua_Hang_Tra_SuaDataContext())
            {
                // Lấy bài có OrderKey = 1, nếu nhiều thì chọn ID_BV cao nhất
                Bai_Viet baiViet = db.Bai_Viets
                                .Where(b => b.OrderKey == 1)
                                .OrderByDescending(b => b.ID_BV)
                                .FirstOrDefault();

                HienThiBaiViet(baiViet);
            }
        }


        private void HienThiBaiViet(Bai_Viet baiViet)
        {
            if (baiViet != null)
            {
                Tieu_de_hot.Text = baiViet.Tieu_de;
                Tom_tac_hot.Text = baiViet.Tom_tac;
                Noi_dung_hot.Text = baiViet.Noi_dung;

                if (!string.IsNullOrEmpty(baiViet.Hinh_anh_page))
                {
                    Image_hot.ImageUrl = "~/uploads/images/" + baiViet.Hinh_anh_page;
                }
            }
            else
            {
                Tieu_de_hot.Text = "Không tìm thấy bài viết";
                Tom_tac_hot.Text = "";
                Noi_dung_hot.Text = "";
                Image_hot.ImageUrl = "";
            }
        }
    }
}
