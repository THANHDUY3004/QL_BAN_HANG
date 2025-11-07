using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LoggedInUser"] != null)
                {
                    // Hiển thị tên người dùng đã lưu (FullName)
                    string fullName = Session["LoggedInUser"].ToString();
                    lblWelcomeMessage.Text = $"Đăng nhập thành công! Chào mừng, {fullName}!";
                }
                else
                {
                    // Nếu không có session, chuyển về trang đăng nhập
                    //Response.Redirect("Login.aspx");
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Xóa session và chuyển hướng về trang đăng nhập
            Session.Abandon();
            //Response.Redirect("Login.aspx");
        }
    }
}