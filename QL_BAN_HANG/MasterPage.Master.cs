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
                phUserTab.Text = "Thông tin tài khoản";
                phUserTab.NavigateUrl = "~/PersonalPage.aspx";
                }
                else
                {
                lnkGioHang.NavigateUrl = "LoginUser.aspx";
                // Chưa đăng nhập
                phUserTab.Text = "Đăng nhập";
                phUserTab.NavigateUrl = "~/LoginUser.aspx";
                } 
        }
    }
}