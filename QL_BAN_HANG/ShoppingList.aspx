<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="ShoppingList.aspx.cs" Inherits="QL_BAN_HANG.ShoppingList" %>
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
        <h1 class="text-3xl font-bold text-gray-800 mb-6 border-b pb-3">Quản Lý Đơn Hàng Đang Chờ - Quán Trà Sữa</h1>
        <asp:Button ID="btn_ls" runat="server" Text="Kiểm tra lịch sử đơn hàng" OnClick="btn_ls_Click" />

        <asp:Panel ID="pnlPendingOrders" runat="server" Visible="true">
            <h2 class="text-2xl font-semibold text-gray-700 mb-4">Đơn Hàng Đang Chờ Xử Lý</h2>

            <div class="flex space-x-4 mb-6">
                <asp:DropDownList 
                    ID="ddlStatusPending" 
                    runat="server" 
                    AutoPostBack="True" 
                    OnSelectedIndexChanged="Filter_Pending_Click"
                    CssClass="rounded-lg border border-gray-300 p-2 focus:ring-blue-500 focus:border-blue-500">
                    <asp:ListItem Value="Tất cả Đơn Chờ" Text="Tất cả Đơn Chờ" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Đang xử lý" Text="Đang xử lý"></asp:ListItem>
                    <asp:ListItem Value="Đang giao" Text="Đang giao"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtSearchPending" runat="server" placeholder="Tìm theo SĐT, Tên khách..." 
                    CssClass="rounded-lg border border-gray-300 p-2 w-72 focus:ring-blue-500 focus:border-blue-500" />
                <asp:Button ID="btnSearchPending" runat="server" Text="Tìm kiếm" OnClick="Filter_Pending_Click" 
                    CssClass="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-lg transition duration-150" />
            </div>
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

            <asp:GridView ID="gvPendingOrders" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="ID_DH" CssClass="gridview-style" 
                OnRowCommand="gvOrders_RowCommand" OnRowDataBound="gvOrders_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ID_DH" HeaderText="Mã ĐH" ItemStyle-Width="80px" >
                    <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Ten_khach_hang" HeaderText="Khách hàng" />
                    <asp:BoundField DataField="Tong_tien" HeaderText="Tổng tiền" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" >
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Trạng thái" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblStatusPending" runat="server" Text='<%# Eval("Trang_thai") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkConfirm" runat="server" CommandName="ConfirmOrder" CommandArgument='<%# Eval("ID_DH") %>' Text="Xác nhận & Giao" CssClass="btn btn-primary btn-sm" />
                            <asp:LinkButton ID="lnkComplete" runat="server" CommandName="CompleteOrder" CommandArgument='<%# Eval("ID_DH") %>' Text="Xác nhận & Hoàn thành" CssClass="btn btn-success btn-sm" Visible="false" />
                            <br />
                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="CancelOrder" CommandArgument='<%# Eval("ID_DH") %>' Text="Hủy" CssClass="btn btn-danger btn-sm" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Chi tiết đơn hàng" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                        <asp:LinkButton ID="btnDetail" runat="server" Text="Chi tiết" CommandName="ViewDetail" CommandArgument='<%# Eval("ID_DH") %>' 
                            CssClass="bg-indigo-500 hover:bg-indigo-600 text-white text-sm font-semibold py-1 px-3 rounded-md mt-1 transition" />
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