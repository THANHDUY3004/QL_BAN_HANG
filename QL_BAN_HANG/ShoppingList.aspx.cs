using Cua_Hang_Tra_Sua;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QL_BAN_HANG
{
    public partial class ShoppingList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Mặc định tải tab Đang chờ (Trạng thái 0)
                BindOrders(0, ddlStatusPending.SelectedValue, txtSearchPending.Text);
                UpdateTabStyles(0);
            }
        }

        /// <summary>
        /// Hàm hiển thị thông báo JS
        /// </summary>
        private void ShowNotification(string message, string type)
        {
            string safeMessage = message.Replace("'", "\\'");
            string script = $"showToastNotification('{type}', '{safeMessage}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "StatusAlert", script, true);
        }

        // --- HÀM XỬ LÝ SỰ KIỆN TỪ .ASPX ---

        /// <summary>
        /// Xử lý khi nhấn 2 tab "Đang chờ" hoặc "Lịch sử"
        /// </summary>
        protected void Tab_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn == null) return;

            // 0 = Pending, 1 = History
            int newStatusType = (btn.CommandName == "History") ? 1 : 0;

            UpdateTabVisibility(newStatusType);
            UpdateTabStyles(newStatusType);

            // Tải dữ liệu cho tab tương ứng
            if (newStatusType == 0)
            {
                BindOrders(0, ddlStatusPending.SelectedValue, txtSearchPending.Text);
            }
            else
            {
                BindOrders(1, ddlStatusHistory.SelectedValue, txtSearchHistory.Text);
            }
        }

        /// <summary>
        /// Xử lý lọc cho tab Đang Chờ
        /// </summary>
        protected void Filter_Pending_Click(object sender, EventArgs e)
        {
            BindOrders(0, ddlStatusPending.SelectedValue, txtSearchPending.Text);
            UpdateTabStyles(0);
        }

        /// <summary>
        /// Xử lý lọc cho tab Lịch Sử
        /// </summary>
        protected void Filter_History_Click(object sender, EventArgs e)
        {
            BindOrders(1, ddlStatusHistory.SelectedValue, txtSearchHistory.Text);
            UpdateTabStyles(1);
        }

        /// <summary>
        /// Xử lý các nút trong GridView (Xác nhận, Hủy, Chi tiết)
        /// </summary>
        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCtdh = 0;
            if (!int.TryParse(e.CommandArgument.ToString(), out idCtdh))
            {
                ShowNotification("Lỗi: ID đơn hàng không hợp lệ.", "error");
                return;
            }

            try
            {
                if (e.CommandName == "ConfirmOrder")
                {
                    Update_OrderStatus(idCtdh, "Đang giao");
                    ShowNotification($"Đơn hàng #{idCtdh} đã được xác nhận và đang giao.", "success");
                    BindOrders(0, ddlStatusPending.SelectedValue, txtSearchPending.Text);
                }
                else if (e.CommandName == "CancelOrder")
                {
                    Update_OrderStatus(idCtdh, "Đã hủy");
                    ShowNotification($"Đơn hàng #{idCtdh} đã được hủy.", "success");
                    BindOrders(0, ddlStatusPending.SelectedValue, txtSearchPending.Text);
                }
                else if (e.CommandName == "ViewDetail")
                {
                    LoadDetailModal(idCtdh);
                    pnlDetailModal.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowNotification($"Lỗi xử lý đơn hàng #{idCtdh}: {ex.Message}", "error");
            }
        }

        /// <summary>
        /// Xử lý màu mè cho Trạng thái trong GridView
        /// </summary>
        protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatusPending") ?? (Label)e.Row.FindControl("lblStatusHistory");

                if (lblStatus != null)
                {
                    string status = lblStatus.Text;
                    string cssClass = "status-badge ";

                    switch (status.ToLower())
                    {
                        case "đang xử lý":
                            cssClass += "status-pending"; // Vàng
                            break;
                        case "đang giao":
                            cssClass += "status-shipping"; // Xanh dương
                            break;
                        case "hoàn thành":
                            cssClass += "status-completed"; // Xanh lá
                            break;
                        case "đã hủy":
                            cssClass += "status-cancelled"; // Đỏ
                            break;
                        default:
                            cssClass += "bg-gray-400 text-gray-800"; // Xám
                            break;
                    }
                    lblStatus.CssClass = cssClass;
                }
            }
        }

        /// <summary>
        /// Xử lý nút Đóng Modal
        /// </summary>
        protected void btnCloseDetail_Click(object sender, EventArgs e)
        {
            pnlDetailModal.Visible = false;
        }

        // --- CÁC HÀM HỖ TRỢ ---

        /// <summary>
        /// Hàm chính tải dữ liệu cho GridView bằng LINQ
        /// </summary>
        private void BindOrders(int statusType, string statusFilter, string searchTerm)
        {
            GridView targetGridView = (statusType == 0) ? gvPendingOrders : gvHistoryOrders;
            if (targetGridView == null) return;

            UpdateTabVisibility(statusType);

            using (var context = new Cua_Hang_Tra_SuaDataContext())
            {
                try
                {
                    // Truy vấn LINQ: Join Chi_Tiet_Don_Hang với Tai_Khoan và Lich_Su_Don_Hang
                    var query = from ctdh in context.Chi_Tiet_Don_Hangs
                                join tk in context.Tai_Khoans on ctdh.So_dien_thoai equals tk.So_dien_thoai
                                join lsdh in context.Lich_Su_Don_Hangs on ctdh.ID_CTDH equals lsdh.ID_DH into lsdhGroup
                                from lsdh in lsdhGroup.DefaultIfEmpty() // LEFT JOIN
                                select new
                                {
                                    ID_DH = ctdh.ID_CTDH,
                                    Ten_khach_hang = tk.Ho_va_ten,
                                    Dia_chi = tk.Dia_chi,
                                    Tong_tien = ctdh.Tong_tien,
                                    Trang_thai = ctdh.Trang_thai_don,
                                    So_dien_thoai = tk.So_dien_thoai,
                                    Thoi_gian_dat = lsdh != null ? lsdh.Ngay_gio_ghi_nhan : DateTime.Now // Sử dụng Ngay_gio_ghi_nhan làm proxy
                                };

                    // 1. Lọc theo loại Tab (Pending/History)
                    if (statusType == 0) // Đang chờ
                    {
                        query = query.Where(q => q.Trang_thai == "Đang xử lý" || q.Trang_thai == "Đang giao");
                    }
                    else // Lịch sử
                    {
                        query = query.Where(q => q.Trang_thai == "Hoàn thành" || q.Trang_thai == "Đã hủy");
                    }

                    // 2. Lọc theo DropDownList (statusFilter)
                    if (statusFilter != "Tất cả" && statusFilter != "Tất cả Đơn Chờ" && statusFilter != "Tất Cả Lịch Sử")
                    {
                        query = query.Where(q => q.Trang_thai == statusFilter);
                    }

                    // 3. Lọc theo Search Term
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = query.Where(q => q.Ten_khach_hang.Contains(searchTerm) ||
                                                  q.So_dien_thoai.Contains(searchTerm) ||
                                                  q.ID_DH.ToString().Contains(searchTerm));
                    }

                    // Sắp xếp và thực thi
                    var result = query.OrderByDescending(q => q.Thoi_gian_dat).ToList();

                    // Chuyển sang DataTable để bind với GridView (hoặc bind trực tiếp nếu GridView hỗ trợ List)
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID_DH", typeof(int));
                    dt.Columns.Add("Ten_khach_hang", typeof(string));
                    dt.Columns.Add("Dia_chi", typeof(string));
                    dt.Columns.Add("Tong_tien", typeof(decimal));
                    dt.Columns.Add("Thoi_gian_dat", typeof(DateTime));
                    dt.Columns.Add("Trang_thai", typeof(string));

                    foreach (var item in result)
                    {
                        dt.Rows.Add(item.ID_DH, item.Ten_khach_hang, item.Dia_chi, item.Tong_tien, item.Thoi_gian_dat, item.Trang_thai);
                    }

                    targetGridView.DataSource = dt;
                    targetGridView.DataBind();
                }
                catch (Exception ex)
                {
                    ShowNotification($"Lỗi kết nối hoặc tải dữ liệu CSDL: {ex.Message}", "error");
                }
            }
        }

        /// <summary>
        /// Cập nhật CSS cho 2 nút Tab
        /// </summary>
        private void UpdateTabStyles(int statusType)
        {
            string activeClass = "tab-button px-6 py-3 text-lg font-medium text-blue-700 border-b-2 border-blue-700";
            string inactiveClass = "tab-button px-6 py-3 text-lg font-medium text-gray-500 hover:text-blue-700 hover:border-blue-700";

            bool isHistorySelected = (statusType == 1);

            if (btnTabPending != null)
            {
                btnTabPending.CssClass = isHistorySelected ? inactiveClass : activeClass;
            }
            if (btnTabHistory != null)
            {
                btnTabHistory.CssClass = isHistorySelected ? activeClass : inactiveClass;
            }
        }

        /// <summary>
        /// Ẩn/hiện Panel của 2 tab
        /// </summary>
        private void UpdateTabVisibility(int statusType)
        {
            if (pnlPendingOrders != null) pnlPendingOrders.Visible = (statusType == 0);
            if (pnlHistory != null) pnlHistory.Visible = (statusType == 1);
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng bằng LINQ/Entity Framework
        /// </summary>
        private void Update_OrderStatus(int orderId, string status)
        {
            using (var context = new Cua_Hang_Tra_SuaDataContext())
            {
                var order = context.Chi_Tiet_Don_Hangs.SingleOrDefault(o => o.ID_CTDH == orderId);
                if (order != null)
                {
                    order.Trang_thai_don = status;
                    context.SubmitChanges();
                }
            }
        }


        /// <summary>
        /// Tải chi tiết đơn hàng lên Modal bằng LINQ
        /// </summary>
        private void LoadDetailModal(int idCtdh)
        {
            using (var context = new Cua_Hang_Tra_SuaDataContext())
            {
                try
                {
                    // 1. Lấy thông tin đơn hàng chung
                    var orderQuery = from ctdh in context.Chi_Tiet_Don_Hangs
                                     join tk in context.Tai_Khoans on ctdh.So_dien_thoai equals tk.So_dien_thoai
                                     join lsdh in context.Lich_Su_Don_Hangs on ctdh.ID_CTDH equals lsdh.ID_DH into lsdhGroup
                                     from lsdh in lsdhGroup.DefaultIfEmpty()
                                     where ctdh.ID_CTDH == idCtdh
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
                        lblNote.Text = order.Ghi_chu;
                        lblTotalDetail.Text = order.Tong_tien.ToString("N0") + " VNĐ";
                    }

                    // 2. Lấy chi tiết sản phẩm
                    var detailQuery = from spdh in context.SP_Don_Hangs
                                      join sp in context.San_Phams on spdh.ID_SP equals sp.ID_SP
                                      where spdh.ID_CTDH == idCtdh
                                      select new
                                      {
                                          So_luong = spdh.So_luong,
                                          Gia_Ban = spdh.Gia_Ban,
                                          Ghi_chu_item = spdh.Ghi_chu_item,
                                          Ten_san_pham = sp.Ten_san_pham
                                      };

                    var details = detailQuery.ToList();
                    gvOrderDetail.DataSource = details;
                    gvOrderDetail.DataBind();
                }
                catch (Exception ex)
                {
                    ShowNotification("Lỗi khi tải chi tiết đơn hàng: " + ex.Message, "error");
                }
            }
        }
    }
}
