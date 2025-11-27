<%@ Page Title="Quản Lý Đơn Hàng" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="ShoppingList.aspx.cs" Inherits="QL_BAN_HANG.ShoppingList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Import font Inter */
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@100..900&display=swap');
        
        body { font-family: 'Inter', sans-serif; background-color: #f4f7f9; }

        /* --- Global Container Styling --- */
        .page-container {
            max-width: 1280px; /* Tăng max-width cho không gian rộng hơn */
            margin: 2rem auto;
            padding: 2.5rem;
            background-color: #ffffff;
            box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 8px 10px -6px rgba(0, 0, 0, 0.1);
            border-radius: 12px;
        }

        /* --- Header Styling --- */
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
        
        /* --- GridView Styling (Cập nhật màu sắc) --- */
        .gridview-style table { 
            width: 100%; 
            border-collapse: collapse; 
            border-radius: 8px; 
            overflow: hidden; 
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05); /* Shadow nhẹ hơn */
        }
        .gridview-style th { 
            background-color: #1e40af; /* Blue 800 - Màu đậm và mạnh mẽ */
            color: white; 
            padding: 16px 20px; 
            text-align: left; 
            font-weight: 700; /* Bolder */
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
        .gridview-style tr:nth-child(even) { background-color: #f9fafb; } /* Gray 50 */
        .gridview-style tr:hover { 
            background-color: #e0f2fe; /* Blue 100 - Hiệu ứng hover nổi bật */
            cursor: pointer;
        }
        .gridview-style td strong { font-weight: 600; color: #1f2937; } /* Tô đậm nội dung quan trọng */
        
        /* --- Status Badges (Giữ nguyên) --- */
        .status-badge { 
            padding: 4px 12px; 
            border-radius: 9999px; 
            font-weight: 700; 
            font-size: 0.75rem; 
            display: inline-block;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            white-space: nowrap; /* Ngăn trạng thái bị xuống dòng */
        }
        /* Đang xử lý - Vàng */
        .status-pending { background-color: #fef3c7; color: #b45309; border: 1px solid #fde68a; } 
        /* Đang giao - Xanh Dương */
        .status-shipping { background-color: #dbeafe; color: #1e40af; border: 1px solid #93c5fd; } 
        /* Hoàn thành - Xanh Lá */
        .status-completed { background-color: #d1fae5; color: #065f46; border: 1px solid #a7f3d0; } 
        /* Đã hủy - Đỏ */
        .status-cancelled { background-color: #fee2e2; color: #991b1b; border: 1px solid #fecaca; } 

        /* --- Custom Button Styles (Nâng cấp) --- */
        .btn {
            font-size: 0.8rem; /* Kích thước nhỏ gọn hơn cho action */
            padding: 8px 15px; /* Padding lớn hơn */
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
        .btn:active {
            transform: translateY(1px); /* Hiệu ứng nhấn */
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        }
        
        /* Xác nhận & Giao (Primary - Xanh Blue Đậm) */
        .btn-primary { 
            background-color: #2563eb; 
            color: white; 
        } 
        .btn-primary:hover { 
            background-color: #1d4ed8; 
            box-shadow: 0 4px 10px rgba(37, 99, 235, 0.4);
        }
        
        /* Hoàn thành (Success - Xanh Lá Cây Tươi) */
        .btn-success { 
            background-color: #059669; /* Emerald 600 */
            color: white; 
        } 
        .btn-success:hover { 
            background-color: #047857; /* Emerald 700 */
            box-shadow: 0 4px 10px rgba(5, 150, 105, 0.4);
        }
        
        /* Hủy (Danger - Đỏ Rực) */
        .btn-danger { 
            background-color: #dc2626; /* Red 600 */
            color: white; 
        } 
        .btn-danger:hover { 
            background-color: #b91c1c; /* Red 700 */
            box-shadow: 0 4px 10px rgba(220, 38, 38, 0.4);
        }

        /* Chi tiết (View Detail - Tím/Indigo) */
        .btn-detail {
            background-color: #4f46e5 !important; /* Indigo 600 */
            color: white !important;
            font-size: 0.85rem !important;
            padding: 8px 15px !important;
            border-radius: 6px !important;
            font-weight: 600 !important;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }
        .btn-detail:hover {
            background-color: #4338ca !important; /* Indigo 700 */
            box-shadow: 0 4px 10px rgba(79, 70, 229, 0.4) !important;
        }

        /* Nút Lịch sử */
        .btn-history {
            background-color: #9333ea; /* Màu Tím Sáng */
            color: white;
            font-weight: 700;
            padding: 10px 20px;
            border-radius: 8px;
            box-shadow: 0 4px 6px rgba(147, 51, 234, 0.3);
            transition: all 0.2s ease;
        }
        .btn-history:hover {
            background-color: #7e22ce; /* Tím đậm hơn */
            box-shadow: 0 6px 10px rgba(147, 51, 234, 0.5);
            transform: translateY(-1px);
        }

        /* Modal Styling */
        .modal-content {
            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
        }
        .modal-close-btn {
            background-color: #6b7280; /* Gray 500 */
            color: white;
        }
        .modal-close-btn:hover {
            background-color: #4b5563; /* Gray 600 */
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="page-container">
        
        <h1 class="page-header">
            <span class="page-header-icon">🛍️</span> Quản Lý Đơn Hàng Đang Chờ <br /> - Quán Trà Sữa -
        </h1>
        
        <div class="mb-6">
            <asp:Button ID="btn_ls" runat="server" Text="Kiểm tra lịch sử đơn hàng" OnClick="btn_ls_Click" 
                CssClass="btn-history" />
        </div>
        
        <asp:Panel ID="pnlPendingOrders" runat="server" Visible="true">
            <h2 class="text-2xl font-semibold text-gray-800 mb-4 pt-4">Đơn Hàng Đang Chờ Xử Lý</h2>

            <div class="flex flex-col sm:flex-row space-y-4 sm:space-y-0 sm:space-x-4 mb-6 items-center">
                <asp:DropDownList 
                    ID="ddlStatusPending" 
                    runat="server" 
                    AutoPostBack="True" 
                    OnSelectedIndexChanged="Filter_Pending_Click"
                    CssClass="rounded-lg border border-gray-300 p-2.5 focus:ring-blue-500 focus:border-blue-500 w-full sm:w-auto transition duration-150 shadow-sm">
                    <asp:ListItem Value="Tất cả Đơn Chờ" Text="Tất cả Đơn Chờ" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Đang xử lý" Text="Đang xử lý"></asp:ListItem>
                    <asp:ListItem Value="Đang giao" Text="Đang giao"></asp:ListItem>
                </asp:DropDownList>
                
                <asp:TextBox ID="txtSearchPending" runat="server" placeholder="Tìm theo SĐT, Tên khách..." 
                    CssClass="rounded-lg border border-gray-300 p-2.5 w-full sm:w-80 focus:ring-blue-500 focus:border-blue-500 transition duration-150 shadow-sm" />
                
                <asp:Button ID="btnSearchPending" runat="server" Text="Tìm kiếm" OnClick="Filter_Pending_Click" 
                    CssClass="btn btn-primary shadow-lg hover:shadow-xl w-full sm:w-auto" />
            </div>
            
            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="text-red-500 font-medium mb-4 block"></asp:Label>

            <div class="overflow-x-auto">
                <asp:GridView ID="gvPendingOrders" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="ID_DH" CssClass="gridview-style" 
                    OnRowCommand="gvOrders_RowCommand" OnRowDataBound="gvOrders_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ID_DH" HeaderText="Mã ĐH" ItemStyle-Width="80px" >
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Ten_khach_hang" HeaderText="Khách hàng" />
                        <asp:BoundField DataField="Tong_tien" HeaderText="Tổng tiền" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" >
                            <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Trạng thái" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblStatusPending" runat="server" Text='<%# Eval("Trang_thai") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hành động" ItemStyle-Width="250px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkConfirm" runat="server" CommandName="ConfirmOrder" CommandArgument='<%# Eval("ID_DH") %>' Text="Xác nhận & Giao" CssClass="btn btn-primary" />
                                <asp:LinkButton ID="lnkComplete" runat="server" CommandName="CompleteOrder" CommandArgument='<%# Eval("ID_DH") %>' Text="Hoàn thành" CssClass="btn btn-success" Visible="false" />
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="CancelOrder" CommandArgument='<%# Eval("ID_DH") %>' Text="Hủy" CssClass="btn btn-danger" />
                            </ItemTemplate>
                            <ItemStyle Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chi tiết" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="Xem chi tiết" CommandName="ViewDetail" CommandArgument='<%# Eval("ID_DH") %>' 
                                    CssClass="btn-detail" />
                            </ItemTemplate>
                             <HeaderStyle Width="150px" />
                             <ItemStyle HorizontalAlign="Center" Width="100px" />
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
                    <p><strong>Trạng thái:</strong> <asp:Label ID="lblStatusDetail" runat="server" CssClass="font-bold status-badge" /></p>
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
                        CssClass="btn modal-close-btn py-2.5 px-6" />
                </div>
            </div>
        </asp:Panel>

        <asp:Label ID="lblNotification" runat="server" CssClass="text-lg font-semibold mt-4 block text-green-600" />
    </div>
</asp:Content>