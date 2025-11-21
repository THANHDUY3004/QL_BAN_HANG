<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="HistoryList.aspx.cs" Inherits="QL_BAN_HANG.HistoryList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@100..900&display=swap');
        body { font-family: 'Inter', sans-serif; background-color: #f4f7f9; }
        .gridview-style table { width: 100%; border-collapse: collapse; }
        .gridview-style th { background-color: #1e40af; color: white; padding: 12px 16px; text-align: left; }
        .gridview-style td { padding: 12px 16px; border-bottom: 1px solid #e5e7eb; }
        .gridview-style tr:nth-child(even) { background-color: #f9fafb; }
        .gridview-style tr:hover { background-color: #eef2ff; }
        .status-badge { padding: 4px 8px; border-radius: 9999px; font-weight: 600; font-size: 0.8rem; }
        .status-pending { background-color: #fef3c7; color: #b45309; } /* Vàng */
        .status-shipping { background-color: #dbeafe; color: #1e40af; } /* Xanh Dương */
        .status-completed { background-color: #d1fae5; color: #065f46; } /* Xanh Lá */
        .status-cancelled { background-color: #fee2e2; color: #991b1b; } /* Đỏ */
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="max-w-7xl mx-auto p-6 lg:p-10 bg-white shadow-xl rounded-xl mt-8">
        <h1 class="text-3xl font-bold text-gray-800 mb-6 border-b pb-3">Quản Lý Lịch Sử Đơn Hàng - Quán Trà Sữa</h1>
        <asp:Button ID="bnt_dh" runat="server" Text="Quay lại quản lý đơn hàng" OnClick="bnt_dh_Click" />
        <asp:Panel ID="pnlHistory" runat="server" Visible="true">
            <h2 class="text-2xl font-semibold text-gray-700 mb-4">Lịch Sử Đơn Hàng (Đã Hoàn thành/Đã Hủy)</h2>

            <div class="flex space-x-4 mb-6">
                <asp:DropDownList ID="ddlStatusHistory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Filter_History_Click" 
                    CssClass="rounded-lg border border-gray-300 p-2 focus:ring-blue-500 focus:border-blue-500">
                    <asp:ListItem Value="Tất Cả Lịch Sử" Text="Tất Cả Lịch Sử" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Hoàn thành" Text="Đã Hoàn thành"></asp:ListItem>
                    <asp:ListItem Value="Đã hủy" Text="Đã Hủy"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtSearchHistory" runat="server" placeholder="Tìm theo Mã ĐH, SĐT..." 
                    CssClass="rounded-lg border border-gray-300 p-2 w-72 focus:ring-blue-500 focus:border-blue-500" />
                <asp:Button ID="btnSearchHistory" runat="server" Text="Tìm kiếm" OnClick="Filter_History_Click" 
                    CssClass="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-lg transition duration-150" />
            </div>
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            <asp:GridView ID="gvHistoryOrders" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="ID_DH" CssClass="gridview-style" 
                OnRowDataBound="gvOrders_RowDataBound" OnRowCommand="gvOrders_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ID_DH" HeaderText="Mã ĐH" ItemStyle-Width="80px" />
                    <asp:BoundField DataField="Thoi_gian_dat" HeaderText="Thời gian" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="Tong_tien" HeaderText="Tổng tiền" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="Trạng thái" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblStatusHistory" runat="server" Text='<%# Eval("Trang_thai") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hành động" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDetailHistory" runat="server" Text="Chi tiết" CommandName="ViewDetail" CommandArgument='<%# Eval("ID_DH") %>' 
                                CssClass="bg-gray-500 hover:bg-gray-600 text-white text-sm font-semibold py-1 px-3 rounded-md transition" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <asp:Panel ID="pnlDetailModal" runat="server" Visible="false" 
            CssClass="fixed inset-0 bg-gray-600 bg-opacity-75 flex items-center justify-center z-50">
            <div class="bg-white p-6 rounded-lg shadow-2xl w-full max-w-xl">
                <h3 class="text-xl font-bold mb-4 border-b pb-2">Chi Tiết Đơn Hàng #<asp:Label ID="lblOrderID" runat="server" /></h3>
                
                <div class="space-y-2 text-gray-700">
                    <p><strong>Khách hàng:</strong> <asp:Label ID="lblCustomerName" runat="server" /></p>
                    <p><strong>SĐT:</strong> <asp:Label ID="lblPhone" runat="server" /></p>
                    <p><strong>Địa chỉ:</strong> <asp:Label ID="lblAddress" runat="server" /></p>
                    <p><strong>Thời gian đặt:</strong> <asp:Label ID="lblOrderTime" runat="server" /></p>
                    <p><strong>Trạng thái:</strong> <asp:Label ID="lblStatusDetail" runat="server" /></p>
                    <p><strong>Ghi chú:</strong> <asp:Label ID="lblNote" runat="server" /></p>
                </div>

                <h4 class="text-lg font-semibold mt-4 mb-2">Sản phẩm:</h4>
                <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="False" 
                    CssClass="gridview-style">
                    <Columns>
                        <asp:BoundField DataField="Ten_san_pham" HeaderText="Tên SP" />
                        <asp:BoundField DataField="So_luong" HeaderText="SL" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Gia_tai_thoi_diem" HeaderText="Giá" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Ghi_chu_item" HeaderText="Ghi chú SP" />
                    </Columns>
                </asp:GridView>

                <p class="text-xl font-bold text-right mt-4 pt-2 border-t">
                    Tổng cộng: <asp:Label ID="lblTotalDetail" runat="server" />
                </p>

                <div class="text-right mt-6">
                    <asp:Button ID="btnCloseDetail" runat="server" Text="Đóng" OnClick="btnCloseDetail_Click" 
                        CssClass="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded-lg transition" />
                </div>
            </div>
        </asp:Panel>

        <asp:Label ID="lblNotification" runat="server" CssClass="text-lg font-semibold mt-4 block" />
    </div>
</asp:Content>