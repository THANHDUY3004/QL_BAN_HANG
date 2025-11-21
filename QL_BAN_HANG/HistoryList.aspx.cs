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
    public partial class HistoryList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (!IsPostBack)
            {
                // Tải dữ liệu cho lịch sử đơn hàng
                BindOrders(ddlStatusHistory.SelectedValue, txtSearchHistory.Text);
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

        /// <summary>
        /// Xử lý lọc cho lịch sử đơn hàng
        /// </summary>
        protected void Filter_History_Click(object sender, EventArgs e)
        {
            BindOrders(ddlStatusHistory.SelectedValue, txtSearchHistory.Text);
            if (gvHistoryOrders.Rows.Count == 0)
            {
                lblMessage.Text = "Không tìm thấy đơn hàng phù hợp với tiêu chí lọc.";
            }
        }

        /// <summary>
        /// Xử lý các nút trong GridView (chỉ Chi tiết cho History)
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
                if (e.CommandName == "ViewDetail")
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
                Label lblStatus = (Label)e.Row.FindControl("lblStatusHistory");

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

        /// <summary>
        /// Hàm tải dữ liệu cho GridView (chỉ History)
        /// </summary>
        private void BindOrders(string statusFilter, string searchTerm)
        {
            using (var context = new Cua_Hang_Tra_SuaDataContext())
            {
                try
                {
                    var query = from ctdh in context.Chi_Tiet_Don_Hangs
                                join tk in context.Tai_Khoans on ctdh.So_dien_thoai equals tk.So_dien_thoai
                                join lsdh in context.Lich_Su_Don_Hangs on ctdh.ID_CTDH equals lsdh.ID_DH into lsdhGroup
                                from lsdh in lsdhGroup.DefaultIfEmpty()
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

                    // Lọc cho History
                    query = query.Where(q => q.Trang_thai == "Hoàn thành" || q.Trang_thai == "Đã hủy");

                    // Lọc theo DropDownList
                    if (statusFilter != "Tất Cả Lịch Sử")
                    {
                        query = query.Where(q => q.Trang_thai == statusFilter);
                    }

                    // Lọc theo Search Term
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = query.Where(q => q.Ten_khach_hang.Contains(searchTerm) ||
                                                  q.So_dien_thoai.Contains(searchTerm) ||
                                                  q.ID_DH.ToString().Contains(searchTerm));
                    }

                    var result = query.OrderByDescending(q => q.Thoi_gian_dat).ToList();

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

                    gvHistoryOrders.DataSource = dt;
                    gvHistoryOrders.DataBind();
                }
                catch (Exception ex)
                {
                    ShowNotification($"Lỗi kết nối hoặc tải dữ liệu CSDL: {ex.Message}", "error");
                }
            }
        }

        /// <summary>
        /// Tải chi tiết đơn hàng lên Modal
        /// </summary>
        private void LoadDetailModal(int idCtdh)
        {
            using (var context = new Cua_Hang_Tra_SuaDataContext())
            {
                try
                {
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

        protected void bnt_dh_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShoppingList.aspx");
        }
    }
}