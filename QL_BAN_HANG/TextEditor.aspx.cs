using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class TextEditor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Lấy toàn bộ nội dung HTML từ Editor
            string htmlContent = Request.Unvalidated["Editor1"];
            //hiện thị nội dung html
            Label1.Text = htmlContent;
            // Hiển thị nguyên bản HTML ra Literal (giữ nguyên hình ảnh, bảng, in đậm...)
            litResult.Text = "<h3>Nội dung đã nhập:</h3>" + htmlContent;
        }
    }
}