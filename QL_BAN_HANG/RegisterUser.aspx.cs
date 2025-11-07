using Cua_Hang_Tra_Sua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class RegisterUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                lblMessage.Text = "Vui lòng kiểm tra lại các trường thông tin bắt buộc.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string matKhau = txtMatKhau.Text;

            const string phanQuyenMacDinh = "Khách Hàng";

            try
            {
                using (Cua_Hang_Tra_SuaDataContext context = new Cua_Hang_Tra_SuaDataContext())
                {
                    var existingAccount = context.Tai_Khoans.SingleOrDefault(tk => tk.So_dien_thoai == soDienThoai);

                    if (existingAccount != null)
                    {
                        lblMessage.Text = "Số điện thoại này đã được đăng ký. Vui lòng sử dụng số khác.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    Tai_Khoan newAccount = new Tai_Khoan
                    {
                        Ho_va_ten = hoTen,
                        So_dien_thoai = soDienThoai,
                        Dia_chi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi,
                        Mat_khau = matKhau,
                        Phan_quyen = phanQuyenMacDinh
                    };

                    context.Tai_Khoans.InsertOnSubmit(newAccount);
                    context.SubmitChanges();

                    Session["SoDienThoai"] = soDienThoai;
                    Session["PhanQuyen"] = phanQuyenMacDinh;

                    Response.Redirect("LoginUser.aspx");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Đã xảy ra lỗi khi đăng ký. Vui lòng thử lại sau. Chi tiết: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}