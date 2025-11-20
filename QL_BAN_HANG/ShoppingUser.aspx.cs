using Cua_Hang_Tra_Sua;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class ShoppingUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblMessage == null)
            {
                // Tạo một Label giả lập nếu nó chưa tồn tại trên trang ASPX
                lblMessage = new Label() { ID = "lblMessage", ForeColor = System.Drawing.Color.Red };
                this.Controls.Add(lblMessage);
            }
            if (lblThongBaoTrong == null)
            {
                lblThongBaoTrong = new Label() { ID = "lblThongBaoTrong", Text = "Giỏ hàng trống!" };
                this.Controls.Add(lblThongBaoTrong);
            }
            if (!IsPostBack)
            {
                // Giả định UserID và Số điện thoại được lấy từ Session/Database khi đăng nhập
                // TODO: Thay thế bằng dữ liệu người dùng thực tế
                txtSoDienThoai.Text = (string)Session["LoggedInUser"];
                txtDiaChiGiaoHang.Text = "";
                LoadGioHang();
                // Bổ sung: Tải lịch sử đơn hàng
                BindPendingOrders();
                BindCompletedOrders();
            }

            // Tính toán tổng tiền mỗi lần load trang (ĐẢM BẢO TÍNH TOÁN LẠI SAU MỖI POSTBACK)
            CalculateTotal();
        }

        public Cua_Hang_Tra_SuaDataContext db = new Cua_Hang_Tra_SuaDataContext();

        private void LoadGioHang()
        {
            string sdt = Session["LoggedInUser"]?.ToString();
            if (string.IsNullOrEmpty(sdt))
            {
                lblMessage.Text = "Bạn chưa đăng nhập!";
                return;
            }

            var gioHangQuery = from gh in db.Gio_Hangs
                               join sp in db.San_Phams on gh.ID_SP equals sp.ID_SP
                               where gh.So_dien_thoai == sdt
                               select new
                               {
                                   gh.ID_GH,
                                   sp.ID_SP,  // Thêm để kiểm tra trạng thái
                                   sp.Ten_san_pham,
                                   sp.Hinh_anh,
                                   gh.So_luong,
                                   gh.Gia_tai_thoi_diem,
                                   gh.Ghi_chu,
                                   Trang_thai = sp.Trang_thai  // Thêm trạng thái sản phẩm
                               };

            var gioHangList = gioHangQuery.ToList();

            // Kiểm tra sản phẩm đã hết và hiển thị thông báo
            string message = "";
            foreach (var item in gioHangList)
            {
                if (item.Trang_thai == "Hết hàng")  // Sử dụng "Hết hàng"
                {
                    message += $"Sản phẩm '{item.Ten_san_pham}' đã hết vui lòng chọn món khác. ";
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                lblMessage.Text = message;
            }
            else
            {
                lblMessage.Text = "";  // Xóa thông báo nếu không có sản phẩm hết
            }

            if (gioHangList.Count == 0)
            {
                gvGioHang.DataSource = null;
                gvGioHang.DataBind();
                lblThongBaoTrong.Visible = true;
                lblTongTien.Text = "0 VNĐ";
            }
            else
            {
                gvGioHang.DataSource = gioHangList;
                gvGioHang.DataKeyNames = new string[] { "ID_GH" };
                gvGioHang.DataBind();
                lblThongBaoTrong.Visible = false;
            }
        }

        // HÀM QUAN TRỌNG: Chỉ tính tổng tiền của các mục đã được Check
        private void CalculateTotal()
        {
            decimal totalAmount = 0;

            foreach (GridViewRow row in gvGioHang.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkChonThanhToan");
                if (chk != null && chk.Checked)
                {
                    int idGh = (int)gvGioHang.DataKeys[row.RowIndex].Value;
                    var item = db.Gio_Hangs.FirstOrDefault(g => g.ID_GH == idGh);
                    if (item != null)
                    {
                        totalAmount += (decimal)(item.So_luong * item.Gia_tai_thoi_diem);
                    }
                }
            }

            lblTongTien.Text = string.Format("{0:N0} VNĐ", totalAmount);
        }

        // Sự kiện xảy ra khi người dùng click vào LinkButton trong cột Hành Động
        protected void gvGioHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Lấy ID_GH của sản phẩm được chọn
            int idGh = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "XoaItem")
            {
                XoaSanPham(idGh);
            }
            else if (e.CommandName == "CapNhatItem")
            {
                // Lấy chỉ số hàng (Row Index)
                int rowIndex = -1;
                for (int i = 0; i < gvGioHang.Rows.Count; i++)
                {
                    if (Convert.ToInt32(gvGioHang.DataKeys[i].Value) == idGh)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex >= 0)
                {
                    CapNhatSanPham(rowIndex, idGh);
                }
            }

            // Tải lại giỏ hàng và tính toán tổng tiền sau khi thao tác
            LoadGioHang();
            CalculateTotal();
        }

        private void XoaSanPham(int idGh)
        {
            var item = db.Gio_Hangs.FirstOrDefault(g => g.ID_GH == idGh);
            if (item != null)
            {
                db.Gio_Hangs.DeleteOnSubmit(item);
                db.SubmitChanges();
                lblMessage.Text = $"Đã xóa sản phẩm ID {idGh} khỏi giỏ hàng.";
            }
        }

        private void CapNhatSanPham(int rowIndex, int idGh)
        {
            GridViewRow row = gvGioHang.Rows[rowIndex];
            TextBox txtSoLuong = (TextBox)row.FindControl("txtSoLuong");
            TextBox txtGhiChu = (TextBox)row.FindControl("txtGhiChuItem");

            if (txtSoLuong != null && int.TryParse(txtSoLuong.Text, out int newSoLuong) && newSoLuong > 0)
            {
                var item = db.Gio_Hangs.FirstOrDefault(g => g.ID_GH == idGh);
                if (item != null)
                {
                    item.So_luong = newSoLuong;
                    item.Ghi_chu = txtGhiChu?.Text;
                    db.SubmitChanges();
                    lblMessage.Text = $"Đã cập nhật sản phẩm ID {idGh}.";
                }
            }
            else
            {
                lblMessage.Text = "Số lượng không hợp lệ.";
            }
        }

        // Xử lý sự kiện Đặt Hàng
        protected void btnDatHang_Click(object sender, EventArgs e)
        {
            string sdt = Session["LoggedInUser"]?.ToString();
            if (string.IsNullOrEmpty(sdt))
            {
                lblMessage.Text = "Bạn chưa đăng nhập!";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChiGiaoHang.Text))
            {
                lblMessage.Text = "Vui lòng nhập Địa chỉ giao hàng.";
                return;
            }

            // Kiểm tra sản phẩm đã hết trước khi đặt hàng
            List<string> hetHang = new List<string>();
            foreach (GridViewRow row in gvGioHang.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkChonThanhToan");
                if (chk != null && chk.Checked)
                {
                    int idGh = (int)gvGioHang.DataKeys[row.RowIndex].Value;
                    var item = db.Gio_Hangs.FirstOrDefault(g => g.ID_GH == idGh);
                    if (item != null)
                    {
                        var sp = db.San_Phams.FirstOrDefault(s => s.ID_SP == item.ID_SP);
                        if (sp != null && sp.Trang_thai == "Hết hàng")  // Sử dụng "Hết hàng"
                        {
                            hetHang.Add(sp.Ten_san_pham);
                        }
                    }
                }
            }
            if (hetHang.Any())
            {
                lblMessage.Text = "Các sản phẩm sau đã hết: " + string.Join(", ", hetHang) + ". Vui lòng chọn món khác.";
                return;
            }

            decimal tongTienDonHang = 0;
            int soSanPham = 0;

            // Tạo đơn hàng tổng thể
            Chi_Tiet_Don_Hang donHang = new Chi_Tiet_Don_Hang
            {
                So_dien_thoai = sdt,
                Hinh_thuc_dat_don = ddlHinhThucDatDon.SelectedValue,
                Trang_thai_don = "Đang xử lý",
                Tong_tien = 0,
                Ghi_chu = txtGhiChuChung.Text
            };
            db.Chi_Tiet_Don_Hangs.InsertOnSubmit(donHang);
            db.SubmitChanges();

            // Duyệt qua giỏ hàng
            foreach (GridViewRow row in gvGioHang.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkChonThanhToan");
                if (chk != null && chk.Checked)
                {
                    int idGh = (int)gvGioHang.DataKeys[row.RowIndex].Value;
                    var item = db.Gio_Hangs.FirstOrDefault(g => g.ID_GH == idGh);

                    if (item != null)
                    {
                        SP_Don_Hang ct = new SP_Don_Hang
                        {
                            ID_CTDH = donHang.ID_CTDH,
                            ID_SP = item.ID_SP,
                            So_luong = item.So_luong,
                            Gia_Ban = (decimal)item.Gia_tai_thoi_diem,
                            Ghi_chu_item = item.Ghi_chu
                        };
                        db.SP_Don_Hangs.InsertOnSubmit(ct);

                        tongTienDonHang += (decimal)(item.So_luong * item.Gia_tai_thoi_diem);
                        soSanPham++;

                        db.Gio_Hangs.DeleteOnSubmit(item);
                    }
                }
            }

            if (soSanPham == 0)
            {
                lblMessage.Text = "Vui lòng chọn ít nhất một sản phẩm để đặt hàng.";
                return;
            }

            donHang.Tong_tien = tongTienDonHang;
            db.SubmitChanges();

            lblMessage.Text = $"ĐẶT HÀNG THÀNH CÔNG! Đơn hàng gồm {soSanPham} sản phẩm, Tổng tiền: {string.Format("{0:N0} VNĐ", tongTienDonHang)}.";
            txtDiaChiGiaoHang.Text = "";
            txtGhiChuChung.Text = "";

            LoadGioHang();
            CalculateTotal();
            // Bổ sung: Tải lại lịch sử đơn hàng sau khi đặt hàng
            BindPendingOrders();
            BindCompletedOrders();
        }

        // Sự kiện này giúp tính toán lại tổng tiền khi trạng thái CheckBox thay đổi
        protected void gvGioHang_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvGioHang.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkChonThanhToan");
                if (chk != null)
                {
                    chk.AutoPostBack = true;
                    chk.CheckedChanged += ChkChonThanhToan_CheckedChanged;
                }
            }
        }

        protected void ChkChonThanhToan_CheckedChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        // Bổ sung: Hàm tải đơn hàng đang xử lý
        private void BindPendingOrders()
        {
            string sdt = Session["LoggedInUser"]?.ToString();
            if (string.IsNullOrEmpty(sdt))
            {
                lblNoPendingOrders.Text = "Bạn chưa đăng nhập.";
                lblNoPendingOrders.Visible = true;
                return;
            }

            var query = from ctdh in db.Chi_Tiet_Don_Hangs
                        join tk in db.Tai_Khoans on ctdh.So_dien_thoai equals tk.So_dien_thoai
                        join lsdh in db.Lich_Su_Don_Hangs on ctdh.ID_CTDH equals lsdh.ID_DH into lsdhGroup
                        from lsdh in lsdhGroup.DefaultIfEmpty()
                        where ctdh.So_dien_thoai == sdt && (ctdh.Trang_thai_don == "Đang xử lý" || ctdh.Trang_thai_don == "Đang giao")
                        select new
                        {
                            ID_DH = ctdh.ID_CTDH,
                            Ten_khach_hang = tk.Ho_va_ten,
                            Dia_chi = tk.Dia_chi,
                            Tong_tien = ctdh.Tong_tien,
                            Trang_thai = ctdh.Trang_thai_don,
                            So_dien_thoai = tk.So_dien_thoai,
                            Thoi_gian_dat = lsdh != null ? lsdh.Ngay_gio_ghi_nhan : DateTime.Now
                        };

            var result = query.OrderByDescending(q => q.Thoi_gian_dat).ToList();

            if (result.Count == 0)
            {
                gvPendingOrders.DataSource = null;
                gvPendingOrders.DataBind();
                lblNoPendingOrders.Visible = true;
            }
            else
            {
                gvPendingOrders.DataSource = result;
                gvPendingOrders.DataBind();
                lblNoPendingOrders.Visible = false;
            }
        }

        // Bổ sung: Hàm tải đơn hàng đã hoàn thành
        private void BindCompletedOrders()
        {
            string sdt = Session["LoggedInUser"]?.ToString();
            if (string.IsNullOrEmpty(sdt))
            {
                lblNoCompletedOrders.Text = "Bạn chưa đăng nhập.";
                lblNoCompletedOrders.Visible = true;
                return;
            }

            var query = from ctdh in db.Chi_Tiet_Don_Hangs
                        join tk in db.Tai_Khoans on ctdh.So_dien_thoai equals tk.So_dien_thoai
                        join lsdh in db.Lich_Su_Don_Hangs on ctdh.ID_CTDH equals lsdh.ID_DH into lsdhGroup
                        from lsdh in lsdhGroup.DefaultIfEmpty()
                        where ctdh.So_dien_thoai == sdt && (ctdh.Trang_thai_don == "Hoàn thành" || ctdh.Trang_thai_don == "Đã hủy")
                        select new
                        {
                            ID_DH = ctdh.ID_CTDH,
                            Ten_khach_hang = tk.Ho_va_ten,
                            Dia_chi = tk.Dia_chi,
                            Tong_tien = ctdh.Tong_tien,
                            Trang_thai = ctdh.Trang_thai_don,
                            So_dien_thoai = tk.So_dien_thoai,
                            Thoi_gian_dat = lsdh != null ? lsdh.Ngay_gio_ghi_nhan : DateTime.Now
                        };

            var result = query.OrderByDescending(q => q.Thoi_gian_dat).ToList();

            if (result.Count == 0)
            {
                gvCompletedOrders.DataSource = null;
                gvCompletedOrders.DataBind();
                lblNoCompletedOrders.Visible = true;
            }
            else
            {
                gvCompletedOrders.DataSource = result;
                gvCompletedOrders.DataBind();
                lblNoCompletedOrders.Visible = false;
            }
        }

        // Bổ sung: Tải chi tiết đơn hàng lên modal
        private void LoadDetailModal(int idCtdh)
        {
            string sdt = Session["LoggedInUser"]?.ToString();
            if (string.IsNullOrEmpty(sdt))
            {
                lblMessage.Text = "Bạn chưa đăng nhập.";
                return;
            }

            // Lấy thông tin chung đơn hàng
            var orderQuery = from ctdh in db.Chi_Tiet_Don_Hangs
                             join tk in db.Tai_Khoans on ctdh.So_dien_thoai equals tk.So_dien_thoai
                             join lsdh in db.Lich_Su_Don_Hangs on ctdh.ID_CTDH equals lsdh.ID_DH into lsdhGroup
                             from lsdh in lsdhGroup.DefaultIfEmpty()
                             where ctdh.ID_CTDH == idCtdh && ctdh.So_dien_thoai == sdt  // Bảo mật: Chỉ xem đơn của mình
                             select new
                             {
                                 ID_CTDH = ctdh.ID_CTDH,
                                 Ho_va_ten = tk.Ho_va_ten,
                                 So_dien_thoai = tk.So_dien_thoai,
                                 Dia_chi = tk.Dia_chi,
                                 Trang_thai_don = ctdh.Trang_thai_don,
                                 Tong_tien = ctdh.Tong_tien,
                                 Ghi_chu = ctdh.Ghi_chu,
                                 Thoi_gian_dat = lsdh != null ? lsdh.Ngay_gio_ghi_nhan : DateTime.Now
                             };

            var order = orderQuery.FirstOrDefault();
            if (order != null)
            {
                lblOrderID.Text = order.ID_CTDH.ToString();
                lblCustomerName.Text = order.Ho_va_ten;
                lblPhone.Text = order.So_dien_thoai;
                lblAddress.Text = order.Dia_chi;
                lblOrderTime.Text = order.Thoi_gian_dat.ToString("g");
                lblStatusDetail.Text = order.Trang_thai_don;
                lblNote.Text = order.Ghi_chu ?? string.Empty;
                lblTotalDetail.Text = order.Tong_tien.ToString("N0") + " VNĐ";
            }

            // Lấy chi tiết sản phẩm
            var detailQuery = from spdh in db.SP_Don_Hangs
                              join sp in db.San_Phams on spdh.ID_SP equals sp.ID_SP
                              where spdh.ID_CTDH == idCtdh
                              select new
                              {
                                  Ten_san_pham = sp.Ten_san_pham,
                                  So_luong = spdh.So_luong,
                                  Gia_tai_thoi_diem = spdh.Gia_Ban, // Giả sử Gia_Ban là giá tại thời điểm; đổi nếu cần
                                  Ghi_chu_item = spdh.Ghi_chu_item ?? string.Empty
                              };

            var details = detailQuery.ToList();
            gvOrderDetail.DataSource = details;
            gvOrderDetail.DataBind();
        }

        // Bổ sung: Xử lý lệnh từ GridView đơn hàng (ViewDetail)
        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCtdh;
            if (!int.TryParse(e.CommandArgument.ToString(), out idCtdh))
            {
                lblMessage.Text = "Lỗi: ID đơn hàng không hợp lệ.";
                return;
            }

            if (e.CommandName == "ViewDetail")
            {
                LoadDetailModal(idCtdh);
                pnlDetailModal.Visible = true;
                // Đăng ký script để hiển thị modal
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal",
                    $"document.getElementById('{pnlDetailModal.ClientID}').style.display = 'block';", true);
            }
        }

        // Bổ sung: Hàm đóng modal (gọi từ JavaScript hoặc event)
        protected void btnCloseDetailModal_Click(object sender, EventArgs e)
        {
            pnlDetailModal.Visible = false;
            // Đăng ký script để ẩn modal (đảm bảo đồng bộ)
            ScriptManager.RegisterStartupScript(this, GetType(), "HideModal",
                $"document.getElementById('{pnlDetailModal.ClientID}').style.display = 'none';", true);
        }
    }
}
