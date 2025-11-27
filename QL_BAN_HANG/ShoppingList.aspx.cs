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
            lblMessage.Text = "";
            //clear llMessage
            if (!IsPostBack)
            {
                // Tải dữ liệu mặc định cho đơn hàng đang chờ
                BindOrders(ddlStatusPending.SelectedValue, txtSearchPending.Text);
            }
        }

        /// <summary>
        /// Hàm hiển thị thông báo JS (sử dụng ScriptManager)
        /// </summary>
        private void ShowNotification(string message, string type)
        {
            string safeMessage = message.Replace("'", "\\'");
            string script = $"showToastNotification('{type}', '{safeMessage}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "StatusAlert", script, true);
        }

        /// <summary>
        /// Xử lý sự kiện lọc (DropDownList và Button)
        /// </summary>
        protected void Filter_Pending_Click(object sender, EventArgs e)
        {
            BindOrders(ddlStatusPending.SelectedValue, txtSearchPending.Text);
            if(gvPendingOrders.Rows.Count == 0)
            {
                lblMessage.Text = "Không tìm thấy đơn hàng phù hợp với tiêu chí lọc.";
            }
        }

        /// <summary>
        /// Xử lý các lệnh từ GridView (Xác nhận, Hủy, Chi tiết, Hoàn thành)
        /// </summary>
        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCtdh;
            if (!int.TryParse(e.CommandArgument.ToString(), out idCtdh))
            {
                ShowNotification("Lỗi: ID đơn hàng không hợp lệ.", "error");
                return;
            }

            try
            {
                switch (e.CommandName)
                {
                    case "ConfirmOrder":
                        Update_OrderStatus(idCtdh, "Đang giao");
                        ShowNotification($"Đơn hàng #{idCtdh} đã được xác nhận và đang giao.", "success");
                        BindOrders(ddlStatusPending.SelectedValue, txtSearchPending.Text);
                        break;
                    case "CompleteOrder":
                        Update_OrderStatus(idCtdh, "Hoàn thành");
                        ShowNotification($"Đơn hàng #{idCtdh} đã được hoàn thành.", "success");
                        BindOrders(ddlStatusPending.SelectedValue, txtSearchPending.Text);
                        break;
                    case "CancelOrder":
                        Update_OrderStatus(idCtdh, "Đã hủy");
                        ShowNotification($"Đơn hàng #{idCtdh} đã được hủy.", "success");
                        BindOrders(ddlStatusPending.SelectedValue, txtSearchPending.Text);
                        break;
                    case "ViewDetail":
                        LoadDetailModal(idCtdh);
                        pnlDetailModal.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowNotification($"Lỗi xử lý đơn hàng #{idCtdh}: {ex.Message}", "error");
            }
        }

        /// <summary>
        /// Áp dụng CSS cho trạng thái trong GridView và điều chỉnh hiển thị nút dựa trên trạng thái
        /// </summary>
        protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Áp dụng CSS cho trạng thái
                Label lblStatus = (Label)e.Row.FindControl("lblStatusPending");
                if (lblStatus != null)
                {
                    string status = lblStatus.Text.ToLower();
                    string cssClass = "status-badge ";
                    switch (status)
                    {
                        case "đang xử lý":
                            cssClass += "status-pending";
                            break;
                        case "đang giao":
                            cssClass += "status-shipping";
                            break;
                        case "hoàn thành":
                            cssClass += "status-completed";
                            break;
                        case "đã hủy":
                            cssClass += "status-cancelled";
                            break;
                        default:
                            cssClass += "bg-gray-400 text-gray-800";
                            break;
                    }
                    lblStatus.CssClass = cssClass;
                }

                // Điều chỉnh hiển thị nút dựa trên trạng thái
                string orderStatus = DataBinder.Eval(e.Row.DataItem, "Trang_thai").ToString();
                LinkButton lnkConfirm = (LinkButton)e.Row.FindControl("lnkConfirm");
                LinkButton lnkComplete = (LinkButton)e.Row.FindControl("lnkComplete");
                LinkButton lnkCancel = (LinkButton)e.Row.FindControl("lnkCancel");
                LinkButton lnkViewDetail = (LinkButton)e.Row.FindControl("lnkViewDetail");

                if (lnkConfirm != null && lnkComplete != null)
                {
                    if (orderStatus == "Đang xử lý")
                    {
                        lnkConfirm.Visible = true;
                        lnkConfirm.Text = "Xác nhận & Giao";
                        lnkComplete.Visible = false;
                    }
                    else if (orderStatus == "Đang giao")
                    {
                        lnkConfirm.Visible = false;
                        lnkComplete.Visible = true;
                        lnkConfirm.Text = "";
                        lnkComplete.Text = "Xác nhận & Hoàn thành";
                    }
                    else
                    {
                        // Ẩn cả hai nếu không phải trạng thái pending
                        lnkConfirm.Visible = false;
                        lnkComplete.Visible = false;
                    }
                }

                // Luôn hiển thị Hủy và Chi tiết
                if (lnkCancel != null) lnkCancel.Visible = true;
                if (lnkViewDetail != null) lnkViewDetail.Visible = true;
            }
        }

        /// <summary>
        /// Đóng modal chi tiết
        /// </summary>
        protected void btnCloseDetail_Click(object sender, EventArgs e)
        {
            pnlDetailModal.Visible = false;
        }

        /// <summary>
        /// Tải dữ liệu cho GridView Pending Orders
        /// </summary>
        private void BindOrders(string statusFilter, string searchTerm)
        {
            using (var context = new Cua_Hang_Tra_SuaDataContext())
            {
                try
                {
                    var query = from ctdh in context.Don_Hangs
                                join tk in context.Tai_Khoans on ctdh.So_dien_thoai equals tk.So_dien_thoai
                                join lsdh in context.Lich_Su_Don_Hangs on ctdh.ID_DH equals lsdh.ID_DH into lsdhGroup
                                from lsdh in lsdhGroup.DefaultIfEmpty()
                                select new
                                {
                                    ID_DH = ctdh.ID_DH,
                                    Ten_khach_hang = tk.Ho_va_ten,
                                    Dia_chi = tk.Dia_chi,
                                    Tong_tien = ctdh.Tong_tien,
                                    Trang_thai = ctdh.Trang_thai_don,
                                    So_dien_thoai = tk.So_dien_thoai,
                                    Thoi_gian_dat = lsdh != null ? lsdh.Ngay_gio_ghi_nhan : DateTime.Now
                                };

                    // Lọc cho Pending: "Đang xử lý" hoặc "Đang giao"
                    query = query.Where(q => q.Trang_thai == "Đang xử lý" || q.Trang_thai == "Đang giao");

                    // Lọc theo DropDownList
                    if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "Tất cả Đơn Chờ")
                    {
                        query = query.Where(q => q.Trang_thai == statusFilter);
                    }

                    // Lọc theo từ khóa tìm kiếm
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = query.Where(q => q.Ten_khach_hang.Contains(searchTerm) ||
                                                  q.So_dien_thoai.Contains(searchTerm) ||
                                                  q.ID_DH.ToString().Contains(searchTerm));
                    }

                    // Sắp xếp và thực thi
                    var result = query.OrderByDescending(q => q.Thoi_gian_dat).ToList();

                    // Chuyển sang DataTable để bind
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID_DH", typeof(int));
                    dt.Columns.Add("Ten_khach_hang", typeof(string));
                    dt.Columns.Add("Dia_chi", typeof(string)); // Thêm nếu cần dùng sau
                    dt.Columns.Add("Tong_tien", typeof(decimal));
                    dt.Columns.Add("Thoi_gian_dat", typeof(DateTime));
                    dt.Columns.Add("Trang_thai", typeof(string));

                    foreach (var item in result)
                    {
                        dt.Rows.Add(item.ID_DH, item.Ten_khach_hang, item.Dia_chi, item.Tong_tien, item.Thoi_gian_dat, item.Trang_thai);
                    }

                    gvPendingOrders.DataSource = dt;
                    gvPendingOrders.DataBind();
                }
                catch (Exception ex)
                {
                    ShowNotification($"Lỗi tải dữ liệu: {ex.Message}", "error");
                }
            }
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        private void Update_OrderStatus(int orderId, string status)
        {
            using (var context = new Cua_Hang_Tra_SuaDataContext())
            {
                try
                {
                    var order = context.Don_Hangs.SingleOrDefault(o => o.ID_DH == orderId);
                    if (order != null)
                    {
                        order.Trang_thai_don = status;
                        context.SubmitChanges();
                    }
                    else
                    {
                        ShowNotification("Không tìm thấy đơn hàng để cập nhật.", "error");
                    }
                }
                catch (Exception ex)
                {
                    ShowNotification($"Lỗi cập nhật trạng thái: {ex.Message}", "error");
                }
            }
        }

        /// <summary>
        /// Tải chi tiết đơn hàng lên modal
        /// </summary>
        private void LoadDetailModal(int idCtdh)
        {
            using (var context = new Cua_Hang_Tra_SuaDataContext())
            {
                try
                {
                    // Lấy thông tin chung đơn hàng
                    var orderQuery = from ctdh in context.Don_Hangs
                                     join tk in context.Tai_Khoans on ctdh.So_dien_thoai equals tk.So_dien_thoai
                                     join lsdh in context.Lich_Su_Don_Hangs on ctdh.ID_DH equals lsdh.ID_DH into lsdhGroup
                                     from lsdh in lsdhGroup.DefaultIfEmpty()
                                     where ctdh.ID_DH == idCtdh
                                     select new
                                     {
                                         ID_CTDH = ctdh.ID_DH,
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
                    var detailQuery = from spdh in context.Chi_Tiet_Don_Hangs
                                      join sp in context.San_Phams on spdh.ID_SP equals sp.ID_SP
                                      where spdh.ID_DH == idCtdh
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
                catch (Exception ex)
                {
                    ShowNotification($"Lỗi tải chi tiết: {ex.Message}", "error");
                }
            }
        }

        protected void btn_ls_Click(object sender, EventArgs e)
        {
            //chuyển sang trang HitoryList
            Response.Redirect("HistoryList.aspx");
        }
    }
}
