using Cua_Hang_Tra_Sua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class ShoppingUser : System.Web.UI.Page
    {
        // Khởi tạo DataContext
        public Cua_Hang_Tra_SuaDataContext db = new Cua_Hang_Tra_SuaDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Xóa thông báo khi load trang mới (tránh hiển thị lại lỗi cũ)
            if (lblMessage != null)
            {
                lblMessage.Text = "";
            }

            if (!IsPostBack)
            {
                // Giả định UserID và Số điện thoại được lấy từ Session/Database khi đăng nhập
                txtSoDienThoai.Text = Session["LoggedInUser"]?.ToString();
                // txtDiaChiGiaoHang.Text = ""; 

                LoadGioHang();
                BindPendingOrders();
                BindCompletedOrders();
            }

            // Tính toán tổng tiền mỗi lần load trang, bao gồm cả PostBack (do CheckBox AutoPostBack)
            CalculateTotal();
        }

        // ----------------------------------------------------------------------------------
        // Xử lý Giỏ Hàng (Cart)
        // ----------------------------------------------------------------------------------

        private void LoadGioHang()
        {
            string sdt = Session["LoggedInUser"]?.ToString();
            if (string.IsNullOrEmpty(sdt))
            {
                lblMessage.Text = "Bạn chưa đăng nhập. Vui lòng đăng nhập để xem giỏ hàng.";
                gvGioHang.DataSource = null;
                gvGioHang.DataBind();
                return;
            }

            // JOIN giữa Gio_Hang và San_Pham để lấy Tên, Hình ảnh và Trạng thái
            var gioHangQuery = from gh in db.Gio_Hangs
                               join sp in db.San_Phams on gh.ID_SP equals sp.ID_SP
                               where gh.So_dien_thoai == sdt
                               select new
                               {
                                   gh.ID_GH,
                                   gh.ID_SP,
                                   sp.Ten_san_pham,
                                   sp.Hinh_anh,
                                   gh.So_luong,
                                   // Lấy Gia_co_ban để tính toán (giả định dùng giá hiện tại)
                                   Gia_co_ban = sp.Gia_co_ban,
                                   gh.Ghi_chu,
                                   sp.Trang_thai // Trạng thái sản phẩm
                               };

            var gioHangList = gioHangQuery.ToList();

            // Kiểm tra sản phẩm đã hết hàng
            List<string> hetHang = gioHangList.Where(item => item.Trang_thai == "Hết hàng").Select(item => item.Ten_san_pham).ToList();
            if (hetHang.Any())
            {
                lblMessage.Text = $"Cảnh báo: Các sản phẩm sau đã hết hàng và không thể thanh toán: {string.Join(", ", hetHang)}. Vui lòng xóa khỏi giỏ.";
            }

            gvGioHang.DataSource = gioHangList;
            gvGioHang.DataKeyNames = new string[] { "ID_GH" };
            gvGioHang.DataBind();
        }

        // HÀM TÍNH TỔNG TIỀN (Chỉ tính các mục đã được Check)
        private void CalculateTotal()
        {
            decimal totalAmount = 0;

            foreach (GridViewRow row in gvGioHang.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkChonThanhToan");

                // Lấy TextBox Số lượng để đảm bảo lấy số lượng mới nhất nếu người dùng chưa nhấn Cập nhật
                TextBox txtSoLuong = (TextBox)row.FindControl("txtSoLuong");
                int currentSoLuong = 0;

                if (chk != null && chk.Checked && int.TryParse(txtSoLuong?.Text, out currentSoLuong))
                {
                    int idGh = (int)gvGioHang.DataKeys[row.RowIndex].Value;

                    var item = (from gh in db.Gio_Hangs
                                join sp in db.San_Phams on gh.ID_SP equals sp.ID_SP
                                where gh.ID_GH == idGh
                                select new { gh.So_luong, sp.Gia_co_ban }).FirstOrDefault();

                    if (item != null)
                    {
                        // Tính toán dựa trên số lượng người dùng vừa nhập (currentSoLuong) và giá hiện tại (Gia_co_ban)
                        totalAmount += (decimal)(currentSoLuong * item.Gia_co_ban);
                    }
                }
            }

            lblTongTien.Text = string.Format("{0:N0} VNĐ", totalAmount);
        }

        protected void gvGioHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idGh = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "XoaItem")
            {
                XoaSanPham(idGh);
            }
            else if (e.CommandName == "CapNhatItem")
            {
                // Lấy RowIndex bằng cách duyệt qua DataKeys
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
                lblMessage.Text = $"Đã xóa sản phẩm khỏi giỏ hàng.";
            }
        }

        private void CapNhatSanPham(int rowIndex, int idGh)
        {
            GridViewRow row = gvGioHang.Rows[rowIndex];
            TextBox txtSoLuong = (TextBox)row.FindControl("txtSoLuong");
            TextBox txtGhiChu = (TextBox)row.FindControl("txtGhiChuItem");

            if (txtSoLuong != null && int.TryParse(txtSoLuong.Text, out int newSoLuong))
            {
                if (newSoLuong <= 0)
                {
                    lblMessage.Text = "Số lượng phải lớn hơn 0.";
                    return;
                }

                var item = db.Gio_Hangs.FirstOrDefault(g => g.ID_GH == idGh);
                if (item != null)
                {
                    item.So_luong = newSoLuong;
                    item.Ghi_chu = txtGhiChu?.Text;
                    db.SubmitChanges();
                    lblMessage.Text = "Đã cập nhật số lượng và ghi chú.";
                }
            }
            else
            {
                lblMessage.Text = "Số lượng không hợp lệ. Vui lòng nhập số nguyên dương.";
            }
        }

        // Cần đảm bảo hàm này được gọi mỗi khi CheckBox thay đổi trạng thái
        protected void gvGioHang_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvGioHang.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkChonThanhToan");
                if (chk != null)
                {
                    // Đảm bảo PostBack xảy ra khi trạng thái CheckBox thay đổi
                    chk.AutoPostBack = true;
                    chk.CheckedChanged += ChkChonThanhToan_CheckedChanged;
                }
            }
        }

        protected void ChkChonThanhToan_CheckedChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        // ----------------------------------------------------------------------------------
        // Xử lý Đặt Hàng (Checkout)
        // ----------------------------------------------------------------------------------

        protected void btnDatHang_Click(object sender, EventArgs e)
        {
            string sdt = txtSoDienThoai.Text;
            if (string.IsNullOrEmpty(sdt))
            {
                lblMessage.Text = "Vui lòng đăng nhập để đặt hàng.";
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDiaChiGiaoHang.Text))
            {
                lblMessage.Text = "Vui lòng nhập Địa chỉ giao hàng.";
                return;
            }

            // Lấy danh sách sản phẩm được chọn để thanh toán
            var itemsToCheckout = new List<Gio_Hang>();
            List<string> hetHang = new List<string>();
            decimal tongTienDonHang = 0;
            int soSanPham = 0;

            foreach (GridViewRow row in gvGioHang.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkChonThanhToan");
                TextBox txtSoLuong = (TextBox)row.FindControl("txtSoLuong");

                int currentSoLuong = 0;

                if (chk != null && chk.Checked && int.TryParse(txtSoLuong?.Text, out currentSoLuong) && currentSoLuong > 0)
                {
                    int idGh = (int)gvGioHang.DataKeys[row.RowIndex].Value;
                    var item = db.Gio_Hangs.FirstOrDefault(g => g.ID_GH == idGh);

                    if (item != null)
                    {
                        var sp = db.San_Phams.FirstOrDefault(s => s.ID_SP == item.ID_SP);

                        if (sp != null && sp.Trang_thai == "Hết hàng")
                        {
                            hetHang.Add(sp.Ten_san_pham);
                        }
                        else
                        {
                            // Cập nhật số lượng và ghi chú mới nhất trước khi checkout
                            item.So_luong = currentSoLuong;
                            item.Ghi_chu = ((TextBox)row.FindControl("txtGhiChuItem"))?.Text;

                            itemsToCheckout.Add(item);
                            tongTienDonHang += (decimal)(item.So_luong * item.Gia_luc_them);
                            soSanPham++;
                        }
                    }
                }
            }

            if (hetHang.Any())
            {
                lblMessage.Text = "Các sản phẩm sau đã hết và không thể thanh toán: " + string.Join(", ", hetHang) + ".";
                return;
            }

            if (soSanPham == 0)
            {
                lblMessage.Text = "Vui lòng chọn ít nhất một sản phẩm để đặt hàng.";
                return;
            }

            // 1. Tạo đơn hàng tổng thể
            Don_Hang donHang = new Don_Hang
            {
                So_dien_thoai = sdt,
                Hinh_thuc_dat_don = ddlHinhThucDatDon.SelectedValue,
                Trang_thai_don = "Đang xử lý",
                Tong_tien = tongTienDonHang,
                Ghi_chu = txtGhiChuChung.Text,
                Dia_chi_giao_hang = txtDiaChiGiaoHang.Text, // Thêm địa chỉ giao hàng
                Ngay_tao = DateTime.Now
            };
            db.Don_Hangs.InsertOnSubmit(donHang);
            db.SubmitChanges(); // Lưu để lấy được ID_DH

            // 2. Thêm chi tiết đơn hàng và xóa khỏi giỏ hàng
            foreach (var item in itemsToCheckout)
            {
                Chi_Tiet_Don_Hang ct = new Chi_Tiet_Don_Hang
                {
                    ID_DH = donHang.ID_DH,
                    ID_SP = item.ID_SP,
                    So_luong = item.So_luong,
                    Gia_Ban = (decimal)item.Gia_luc_them, // Sử dụng giá tại thời điểm thêm vào giỏ
                    Ghi_chu_item = item.Ghi_chu
                };
                db.Chi_Tiet_Don_Hangs.InsertOnSubmit(ct);

                // Xóa khỏi giỏ hàng
                db.Gio_Hangs.DeleteOnSubmit(item);
            }

            // 3. Thêm Lịch sử đơn hàng (ghi nhận thời điểm đặt)
            Lich_Su_Don_Hang ls = new Lich_Su_Don_Hang
            {
                ID_DH = donHang.ID_DH,
                Ngay_gio_ghi_nhan = DateTime.Now,
                Trang_thai = "Đang xử lý",
                Ghi_chu = "Đơn hàng được khởi tạo bởi khách hàng"
            };
            db.Lich_Su_Don_Hangs.InsertOnSubmit(ls);

            db.SubmitChanges();

            lblMessage.Text = $"ĐẶT HÀNG THÀNH CÔNG! Đơn hàng #{donHang.ID_DH}, Tổng tiền: {string.Format("{0:N0} VNĐ", tongTienDonHang)}.";
            txtGhiChuChung.Text = "";

            LoadGioHang();
            BindPendingOrders();
            BindCompletedOrders();
        }

        // ----------------------------------------------------------------------------------
        // Xử lý Lịch Sử Đơn Hàng (Order History)
        // ----------------------------------------------------------------------------------

        private void BindPendingOrders()
        {
            string sdt = Session["LoggedInUser"]?.ToString();
            if (string.IsNullOrEmpty(sdt)) return;

            // Đơn hàng có trạng thái Đang xử lý hoặc Đang giao
            var query = from dh in db.Don_Hangs
                        where dh.So_dien_thoai == sdt && (dh.Trang_thai_don == "Đang xử lý" || dh.Trang_thai_don == "Đang giao")
                        select new
                        {
                            dh.ID_DH,
                            dh.Tong_tien,
                            dh.Trang_thai_don,
                            // Lấy ngày tạo đơn hàng
                            Ngay_tao = dh.Ngay_tao
                        };

            var result = query.OrderByDescending(q => q.Ngay_tao).ToList();

            gvPendingOrders.DataSource = result;
            gvPendingOrders.DataBind();
        }

        private void BindCompletedOrders()
        {
            string sdt = Session["LoggedInUser"]?.ToString();
            if (string.IsNullOrEmpty(sdt)) return;

            // Đơn hàng có trạng thái Hoàn thành hoặc Đã hủy
            var query = from dh in db.Don_Hangs
                        where dh.So_dien_thoai == sdt && (dh.Trang_thai_don == "Hoàn thành" || dh.Trang_thai_don == "Đã hủy")
                        select new
                        {
                            dh.ID_DH,
                            dh.Tong_tien,
                            dh.Trang_thai_don,
                            Ngay_tao = dh.Ngay_tao
                        };

            var result = query.OrderByDescending(q => q.Ngay_tao).ToList();

            gvCompletedOrders.DataSource = result;
            gvCompletedOrders.DataBind();
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idDh;
            if (!int.TryParse(e.CommandArgument.ToString(), out idDh))
            {
                lblMessage.Text = "Lỗi: ID đơn hàng không hợp lệ.";
                return;
            }

            if (e.CommandName == "ViewDetail")
            {
                LoadDetailModal(idDh);
                // Đăng ký script để hiển thị modal
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal",
                    $"document.getElementById('{pnlDetailModal.ClientID}').style.display = 'block';", true);
            }
            // Bổ sung: Xử lý Hủy đơn hàng
            else if (e.CommandName == "CancelOrder")
            {
                HuyDonHang(idDh);
            }

            // Tải lại lịch sử sau khi thao tác (Hủy)
            BindPendingOrders();
            BindCompletedOrders();
        }

        // Hàm Hủy đơn hàng (chỉ cho phép khi Đang xử lý)
        private void HuyDonHang(int idDh)
        {
            var donHang = db.Don_Hangs.FirstOrDefault(dh => dh.ID_DH == idDh);
            if (donHang == null)
            {
                lblMessage.Text = "Không tìm thấy đơn hàng.";
                return;
            }

            if (donHang.Trang_thai_don == "Đang xử lý")
            {
                donHang.Trang_thai_don = "Đã hủy";

                // Thêm lịch sử hủy
                Lich_Su_Don_Hang ls = new Lich_Su_Don_Hang
                {
                    ID_DH = donHang.ID_DH,
                    Ngay_gio_ghi_nhan = DateTime.Now,
                    Trang_thai = "Đã hủy",
                    Ghi_chu = "Khách hàng hủy đơn"
                };
                db.Lich_Su_Don_Hangs.InsertOnSubmit(ls);

                db.SubmitChanges();
                lblMessage.Text = $"Đơn hàng #{idDh} đã được hủy thành công.";
            }
            else
            {
                lblMessage.Text = $"Không thể hủy đơn hàng #{idDh} vì đơn hàng đang ở trạng thái '{donHang.Trang_thai_don}'.";
            }
        }


        // Tải chi tiết đơn hàng lên modal
        private void LoadDetailModal(int idDh)
        {
            string sdt = Session["LoggedInUser"]?.ToString();

            // Lấy thông tin chung đơn hàng
            var orderQuery = from dh in db.Don_Hangs
                             join tk in db.Tai_Khoans on dh.So_dien_thoai equals tk.So_dien_thoai
                             where dh.ID_DH == idDh && dh.So_dien_thoai == sdt
                             select new
                             {
                                 dh.ID_DH,
                                 Ho_va_ten = tk.Ho_va_ten,
                                 So_dien_thoai = dh.So_dien_thoai,
                                 Dia_chi = dh.Dia_chi_giao_hang,
                                 dh.Trang_thai_don,
                                 dh.Tong_tien,
                                 dh.Ghi_chu,
                                 dh.Hinh_thuc_dat_don,
                                 Thoi_gian_dat = dh.Ngay_tao
                             };

            var order = orderQuery.FirstOrDefault();
            if (order != null)
            {
                lblOrderID.Text = order.ID_DH.ToString();
                lblCustomerName.Text = order.Ho_va_ten;
                lblPhone.Text = order.So_dien_thoai;
                lblAddress.Text = order.Dia_chi;
                lblOrderTime.Text = order.Thoi_gian_dat.ToString("g");
                lblStatusDetail.Text = order.Trang_thai_don;
                lblNote.Text = order.Ghi_chu ?? string.Empty;
                lblTotalDetail.Text = order.Tong_tien.ToString("N0") + " VNĐ";
                lblPaymentMethod.Text = order.Hinh_thuc_dat_don;
            }

            // Lấy chi tiết sản phẩm
            var detailQuery = from spdh in db.Chi_Tiet_Don_Hangs
                              join sp in db.San_Phams on spdh.ID_SP equals sp.ID_SP
                              where spdh.ID_DH == idDh
                              select new
                              {
                                  Ten_san_pham = sp.Ten_san_pham,
                                  spdh.So_luong,
                                  spdh.Gia_Ban,
                                  Ghi_chu_item = spdh.Ghi_chu_item ?? string.Empty
                              };

            var details = detailQuery.ToList();
            gvOrderDetail.DataSource = details;
            gvOrderDetail.DataBind();
        }

        // Bổ sung: Hàm đóng modal (gọi từ JavaScript hoặc event)
        protected void btnCloseDetailModal_Click(object sender, EventArgs e)
        {
            // Đăng ký script để ẩn modal
            ScriptManager.RegisterStartupScript(this, GetType(), "HideModal",
        $"document.getElementById('{pnlDetailModal.ClientID}').style.display = 'none';", true);
        }
    }
}