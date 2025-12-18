using System;
using System.Web;
using System.Web.UI;

namespace QL_BAN_HANG
{
    public partial class TestEdittor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Khởi tạo giá trị mặc định nếu cần
                // txtContent.Text = "<p>Chào mừng bạn đến với CKEditor!</p>";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Cách 1: Lấy trực tiếp từ TextBox (Yêu cầu ValidateRequest="false" ở thẻ @Page)
            string data = txtContent.Text;

            // Cách 2: Lấy từ Request.Unvalidated (An toàn hơn nếu bạn dùng .NET 4.5+)
            // string data = Request.Unvalidated.Form[txtContent.UniqueID];

            // Hiển thị kết quả ra màn hình
            if (!string.IsNullOrEmpty(data))
            {
                litResult.Text = data;
            }
            else
            {
                litResult.Text = "<i style='color:red;'>Nội dung trống!</i>";
            }
        }
    }
}