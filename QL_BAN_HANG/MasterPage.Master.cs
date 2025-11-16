using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
                if (Session["LoggedInUser"] != null)
                {
                lnkGioHang.NavigateUrl = "ShoppingUser.aspx";
                // Đã đăng nhập
                phUserTab.Text = ""; // Ẩn liên kết "Đăng nhập"
                phUserTab.NavigateUrl = "";

                phUserTab1.Text = "Thông tin tài khoản";
                phUserTab1.NavigateUrl = "~/PersonalPage.aspx";
                }
                else
                {
                lnkGioHang.NavigateUrl = "LoginUser.aspx";
                // Chưa đăng nhập
                phUserTab.Text = "Đăng nhập";
                phUserTab.NavigateUrl = "~/LoginUser.aspx";

                phUserTab1.Text = ""; // Ẩn liên kết "Thông tin tài khoản"
                phUserTab1.NavigateUrl = "";
                } 
        }
    }
}