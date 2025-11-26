<%@ Page Title="Lịch Sử Đơn Hàng" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="HistoryList.aspx.cs" Inherits="QL_BAN_HANG.HistoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Import font Inter */
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@100..900&display=swap');
        
        body { font-family: 'Inter', sans-serif; background-color: #f4f7f9; }

        /* --- Global Container Styling (Đồng bộ với ShoppingList) --- */
        .page-container {
            max-width: 1280px;
            margin: 2rem auto;
            padding: 2.5rem;
            background-color: #ffffff;
            box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 8px 10px -6px rgba(0, 0, 0, 0.1);
            border-radius: 12px;
        }

        /* --- Header Styling (Đồng bộ với ShoppingList) --- */
        .page-header {
            font-size: 2.25rem; /* text-4xl */
            font-weight: 800; /* font-extrabold */
            color: #1f2937; /* Gray 800 */
            margin-bottom: 1.5rem;
            border-bottom: 5px solid #3b82f6; /* Blue 500 */
            padding-bottom: 0.75rem;
            display: flex;
            align-items: center;
        }
        .page-header-icon {
            margin-right: 0.75rem;
            color: #3b82f6; /* Blue 500 */
            font-size: 1.5em;
        }

        /* --- GridView Styling (Đồng bộ với ShoppingList) --- */
        .gridview-style table { 
            width: 100%; 
            border-collapse: collapse; 
            border-radius: 8px; 
            overflow: hidden; 
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
        }
        .gridview-style th { 
            background-color: #1e40af; /* Blue 800 */
            color: white; 
            padding: 16px 20px; 
            text-align: left; 
            font-weight: 700;
            text-transform: uppercase;
            font-size: 0.85rem;
            letter-spacing: 0.05em;
        }
        .gridview-style td { 
            padding: 14px 20px; 
            border-bottom: 1px solid #e5e7eb; /* Gray 200 */
            font-size: 0.9rem;
            color: #374151; /* Gray 700 */
            vertical-align: middle;
        }
        .gridview-style tr:nth-child(even) { background-color: #f9fafb; }
        .gridview-style tr:hover { 
            background-color: #e0f2fe; /* Blue 100 */
            cursor: pointer;
        }

        /* --- Status Badges (Đồng bộ với ShoppingList) --- */
        .status-badge { 
            padding: 4px 12px; 
            border-radius: 9999px; 
            font-weight: 700; 
            font-size: 0.75rem; 
            display: inline-block;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            white-space: nowrap;
            border: 1px solid transparent;
        }
        /* Đang xử lý - Vàng */
        .status-pending { background-color: #fef3c7; color: #b45309; border-color: #fde68a; } 
        /* Đang giao - Xanh Dương */
        .status-shipping { background-color: #dbeafe; color: #1e40af; border-color: #93c5fd; } 
        /* Hoàn thành - Xanh Lá */
        .status-completed { background-color: #d1fae5; color: #065f46; border-color: #a7f3d0; } 
        /* Đã hủy - Đỏ */
        .status-cancelled { background-color: #fee2e2; color: #991b1b; border-color: #fecaca; } 

        /* --- Nút bấm (Đồng bộ hiệu ứng) --- */
        .btn-base {
            font-size: 0.9rem;
            padding: 8px 15px;
            border-radius: 6px;
            margin-right: 6px;
            text-decoration: none;
            display: inline-block;
            transition: background-color 0.2s ease, transform 0.1s ease, box-shadow 0.2s ease;
            font-weight: 600;
            border: none;
            cursor: pointer;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }
        .btn-base:active {
            transform: translateY(1px);
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        }

        /* Nút Quay lại (Return Button - Indigo/Blue) */
        .btn-return {
            background-color: #4f46e5; /* Indigo 600 */
            color: white;
            font-weight: 700;
            padding: 10px 20px;
            border-radius: 8px;
            box-shadow: 0 4px 6px rgba(79, 70, 229, 0.3);
        }
        .btn-return:hover {
            background-color: #4338ca; /* Indigo 700 */
            box-shadow: 0 6px 10px rgba(79, 70, 229, 0.5);
            transform: translateY(-1px);
        }
        
        /* Nút Tìm kiếm (Search Button - Primary Blue) */
        .btn-primary {
            background-color: #2563eb; 
            color: white;
        }
        .btn-primary:hover {
            background-color: #1d4ed8;
            box-shadow: 0 4px 10px rgba(37, 99, 235, 0.4);
        }

        /* Nút Chi tiết (Detail Button - Gray) */
        .btn-detail-history {
            background-color: #6b7280 !important; /* Gray 500 */
            color: white !important;
            font-size: 0.85rem !important;
            padding: 6px 12px !important;
            border-radius: 6px !important;
            font-weight: 600 !important;
        }
        .btn-detail-history:hover {
            background-color: #4b5563 !important; /* Gray 700 */
            box-shadow: 0 4px 10px rgba(107, 114, 128, 0.4) !important;
        }

        /* Nút đóng Modal */
        .modal-close-btn {
            background-color: #6b7280;
            color: white;
        }
        .modal-close-btn:hover {
            background-color: #4b5563;
        }
        
        /* Modal Styling */
        .modal-content {
            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="page-container">
        
        <h1 class="page-header">
            <span class="page-header-icon">🕰️</span> Quản Lý Lịch Sử Đơn Hàng - Quán Trà Sữa
        </h1>

        <div class="mb-6">
            <asp:Button ID="bnt_dh" runat="server" Text="Quay lại quản lý đơn hàng" OnClick="bnt_dh_Click" 
                CssClass="btn-return" />
        </div>
        
        <asp:Panel ID="pnlHistory" runat="server" Visible="true">
            <h2 class="text-2xl font-semibold text-gray-700 mb-4 pt-4">Lịch Sử Đơn Hàng (Đã Hoàn thành/Đã Hủy)</h2>

            <div class="flex flex-col sm:flex-row space-y-4 sm:space-y-0 sm:space-x-4 mb-6 items-center">
                <asp:DropDownList ID="ddlStatusHistory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Filter_History_Click" 
                    CssClass="rounded-lg border border-gray-300 p-2.5 focus:ring-blue-500 focus:border-blue-500 w-full sm:w-auto transition duration-150 shadow-sm">
                    <asp:ListItem Value="Tất Cả Lịch Sử" Text="Tất Cả Lịch Sử" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Hoàn thành" Text="Đã Hoàn thành"></asp:ListItem>
                    <asp:ListItem Value="Đã hủy" Text="Đã Hủy"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtSearchHistory" runat="server" placeholder="Tìm theo Mã ĐH, SĐT..." 
                    CssClass="rounded-lg border border-gray-300 p-2.5 w-full sm:w-80 focus:ring-blue-500 focus:border-blue-500 transition duration-150 shadow-sm" />
                <asp:Button ID="btnSearchHistory" runat="server" Text="Tìm kiếm" OnClick="Filter_History_Click" 
                    CssClass="btn-base btn-primary shadow-lg hover:shadow-xl w-full sm:w-auto" />
            </div>

            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="text-red-500 font-medium mb-4 block"></asp:Label>
            
            <div class="overflow-x-auto">
                <asp:GridView ID="gvHistoryOrders" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="ID_DH" CssClass="gridview-style" 
                    OnRowDataBound="gvOrders_RowDataBound" OnRowCommand="gvOrders_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ID_DH" HeaderText="Mã ĐH" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="Thoi_gian_dat" HeaderText="Thời gian" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:BoundField DataField="Tong_tien" HeaderText="Tổng tiền" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" >
                            <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="So_dien_thoai" HeaderText="SĐT Người Đặt" ItemStyle-Width="150px" />
                        
                        <asp:TemplateField HeaderText="Trạng thái" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblStatusHistory" runat="server" Text='<%# Eval("Trang_thai") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hành động" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetailHistory" runat="server" Text="Chi tiết" CommandName="ViewDetail" CommandArgument='<%# Eval("ID_DH") %>' 
                                    CssClass="btn-base btn-detail-history" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlDetailModal" runat="server" Visible="false" 
            CssClass="fixed inset-0 bg-gray-900 bg-opacity-75 flex items-center justify-center z-50 p-4">
            <div class="bg-white p-8 rounded-xl w-full max-w-3xl transform transition-all duration-300 scale-100 opacity-100 modal-content">
                <h3 class="text-2xl font-bold text-gray-800 mb-5 border-b-2 border-indigo-500 pb-3 flex items-center">
                    <span class="mr-2 text-indigo-500">📄</span> Chi Tiết Đơn Hàng #<asp:Label ID="lblOrderID" runat="server" CssClass="ml-1 text-indigo-600" />
                </h3>
                
                <div class="grid grid-cols-1 md:grid-cols-2 gap-4 text-gray-700 text-base border border-gray-200 p-4 rounded-lg bg-gray-50 mb-6">
                    <p class="col-span-1 md:col-span-2 text-lg"><strong>Khách hàng:</strong> <asp:Label ID="lblCustomerName" runat="server" CssClass="font-semibold text-gray-900" /></p>
                    <p><strong>SĐT:</strong> <asp:Label ID="lblPhone" runat="server" CssClass="font-medium" /></p>
                    <p><strong>Địa chỉ:</strong> <asp:Label ID="lblAddress" runat="server" CssClass="font-medium" /></p>
                    <p><strong>Thời gian đặt:</strong> <asp:Label ID="lblOrderTime" runat="server" CssClass="font-medium" /></p>
                    <p><strong>Trạng thái:</strong> <asp:Label ID="lblStatusDetail" runat="server" CssClass="font-bold" /></p>
                    <p class="col-span-1 md:col-span-2 mt-3 text-sm"><strong>Ghi chú đơn hàng:</strong> <asp:Label ID="lblNote" runat="server" CssClass="italic text-gray-600" /></p>
                </div>

                <h4 class="text-xl font-semibold text-gray-800 mt-6 mb-3 border-b pb-1">Sản phẩm:</h4>
                <div class="overflow-x-auto">
                    <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="False" 
                        CssClass="gridview-style">
                        <Columns>
                            <asp:BoundField DataField="Ten_san_pham" HeaderText="Tên SP" />
                            <asp:BoundField DataField="So_luong" HeaderText="SL" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Gia_tai_thoi_diem" HeaderText="Giá" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Ghi_chu_item" HeaderText="Ghi chú SP" />
                        </Columns>
                    </asp:GridView>
                </div>

                <p class="text-2xl font-bold text-right mt-6 pt-4 border-t-2 border-dashed border-gray-300">
                    Tổng cộng: <asp:Label ID="lblTotalDetail" runat="server" CssClass="text-red-600 ml-2" />
                </p>

                <div class="text-right mt-6">
                    <asp:Button ID="btnCloseDetail" runat="server" Text="Đóng" OnClick="btnCloseDetail_Click" 
                        CssClass="btn-base modal-close-btn py-2.5 px-6" />
                </div>
            </div>
        </asp:Panel>

        <asp:Label ID="lblNotification" runat="server" CssClass="text-lg font-semibold mt-4 block text-green-600" />
    </div>
</asp:Content>