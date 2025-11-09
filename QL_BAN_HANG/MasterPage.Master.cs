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
                if (Session["LoggedInUser"] == null)
                {
                    phUserTab.Controls.Add(new Literal
                    {
                        Text = "<li><a href='LoginUser.aspx'>Đăng nhập</a></li>"
                    });
                }
                else
                {
                    phUserTab.Controls.Add(new Literal
                    {
                        Text = "<li><a href='PersonalPage.aspx'>Thông tin tài khoản</a></li>"
                    });
                }
            
        }
    }
}