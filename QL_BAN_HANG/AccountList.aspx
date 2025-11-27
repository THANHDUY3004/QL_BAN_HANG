<%@ Page Title="Quản Lý Tài Khoản" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="AccountList.aspx.cs" Inherits="QL_BAN_HANG.AccountList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Import font Inter */
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@100..900&display=swap');
        
        body { font-family: 'Inter', sans-serif; background-color: #f4f7f9; }

        /* --- Global Container Styling (Đồng bộ) --- */
        .page-container {
            max-width: 1280px;
            margin: 2rem auto;
            padding: 2.5rem;
            background-color: #ffffff;
            box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 8px 10px -6px rgba(0, 0, 0, 0.1);
            border-radius: 12px;
        }

        /* --- Header Styling (Đồng bộ) --- */
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
        
        /* --- GridView Styling (Đồng bộ) --- */
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
        
        /* Cải tiến style cho Header/Footer của GridView cũ */
        /* Ghi đè Footer và Header mặc định của ASP.NET GridView */
        .gridview-style .asp-header-style { 
            background-color: #1e40af !important; 
            color: white !important;
        }
        
        /* --- Nút bấm (Đồng bộ hiệu ứng) --- */
        .btn-base {
            font-size: 0.9rem;
            padding: 10px 20px; /* Tăng padding cho nút */
            border-radius: 6px;
            margin-right: 6px;
            text-decoration: none;
            display: inline-block;
            transition: background-color 0.2s ease, transform 0.1s ease, box-shadow 0.2s ease;
            font-weight: 600;
            border: none;
            cursor: pointer;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
            white-space: nowrap;
        }
        .btn-base:active {
            transform: translateY(1px);
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        }

        /* Nút Đăng Xuất/Quay lại (Return Button - Indigo/Blue) */
        .btn-return {
            background-color: #ef4444; /* Red 500 */
            color: white;
            font-weight: 700;
            border-radius: 8px;
            box-shadow: 0 4px 6px rgba(239, 68, 68, 0.3);
            text-transform: uppercase;
        }
        .btn-return:hover {
            background-color: #dc2626; /* Red 600 */
            box-shadow: 0 6px 10px rgba(239, 68, 68, 0.5);
            transform: translateY(-1px);
        }
        
        /* Nút Thêm/Tìm kiếm (Primary Blue) */
        .btn-primary {
            background-color: #2563eb; 
            color: white;
            font-weight: 700;
        }
        .btn-primary:hover {
            background-color: #1d4ed8;
            box-shadow: 0 4px 10px rgba(37, 99, 235, 0.4);
        }

        /* Nút Sửa (Edit Link) */
        .btn-edit {
            color: #3b82f6; /* Blue 500 */
            text-decoration: none;
            font-weight: 600;
            padding: 4px 8px;
            border-radius: 4px;
            transition: color 0.15s;
        }
        .btn-edit:hover {
            color: #1d4ed8; /* Blue 700 */
            text-decoration: underline;
        }
        
        /* Nút Xóa (Delete CommandField) */
        .gridview-style a[onclick*='Delete'] {
            color: #ef4444; /* Red 500 */
            text-decoration: none;
            font-weight: 600;
            padding: 4px 8px;
            border-radius: 4px;
            transition: color 0.15s;
        }
        .gridview-style a[onclick*='Delete']:hover {
            color: #dc2626; /* Red 600 */
            text-decoration: underline;
        }
        
        /* Tùy chỉnh input/select */
        .form-input {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 6px;
            box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.075);
            transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
            box-sizing: border-box;
            height: 40px;
        }
        .form-input:focus {
            border-color: #3b82f6;
            outline: 0;
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.5);
        }
        
        /* Căn chỉnh form thêm/lọc */
        .form-group-flex {
            display: flex;
            align-items: center;
            gap: 15px;
            margin-bottom: 10px;
        }
        .form-group-flex label {
            min-width: 100px;
            font-weight: 600;
            color: #4b5563; /* Gray 600 */
        }
        .form-field-wrapper {
            flex-grow: 1;
        }
        
        /* CSS cho Label thông báo */
        .message-label {
            font-weight: bold;
            color: #ef4444; /* Red for error */
        }
        
        /* Nút xóa hàng loạt */
        #ContentPlaceHolderContent_butDelete {
            background-color: #f59e0b; /* Amber */
            color: white;
            font-weight: 700;
            padding: 8px 15px;
            border-radius: 6px;
            cursor: pointer;
            border: none;
            transition: background-color 0.2s;
        }
        #ContentPlaceHolderContent_butDelete:hover {
            background-color: #d97706; /* Amber Darker */
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="page-container">
        
        <h1 class="page-header">
            <span class="page-header-icon">⚙️</span> Quản Lý Tài Khoản Người Dùng
        </h1>
        
        <div style="text-align: right; margin-bottom: 2rem;">
            <asp:Button ID="btnLoginPage" 
                        runat="server" 
                        Text="Đăng Xuất (Quay lại trang Login)" 
                        OnClick="BtnLoginPage_Click" 
                        CssClass="btn-base btn-return" />
        </div>

        <div class="section-add mb-8 p-6 border border-gray-200 rounded-lg bg-gray-50">
            <h3 class="text-xl font-bold text-gray-800 mb-4 border-b pb-2">➕ Thêm Tài Khoản Mới</h3>
            
            <div style="max-width: 600px; margin: 0 auto;">
                <div class="form-group-flex">
                    <label>Phân Quyền:</label>
                    <div class="form-field-wrapper">
                        <asp:DropDownList ID="ddlPhanQuyen" runat="server" CssClass="form-input" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group-flex">
                    <label>Họ Và Tên:</label>
                    <div class="form-field-wrapper">
                        <asp:TextBox ID="txtht" runat="server" CssClass="form-input" placeholder="Nhập họ và tên"></asp:TextBox> 
                    </div>
                </div>
                <div class="form-group-flex">
                    <label>Số Điện Thoại:</label>
                    <div class="form-field-wrapper">
                        <asp:TextBox ID="txtsdt" runat="server" CssClass="form-input" placeholder="Nhập số điện thoại (Dùng làm Username)"></asp:TextBox> 
                    </div>
                </div>
                <div class="form-group-flex">
                    <label>Địa Chỉ:</label>
                    <div class="form-field-wrapper">
                        <asp:TextBox ID="txtdchi" runat="server" CssClass="form-input" placeholder="Nhập địa chỉ"></asp:TextBox> 
                    </div>
                </div>
                <div class="form-group-flex">
                    <label>Mật Khẩu:</label>
                    <div class="form-field-wrapper">
                        <asp:TextBox ID="txtmk" runat="server" TextMode="Password" CssClass="form-input" placeholder="Nhập mật khẩu"></asp:TextBox> 
                    </div>
                </div>
                
                <div class="form-group-flex" style="justify-content: space-between; margin-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message-label text-sm ml-2"></asp:Label>
                    <asp:Button ID="butAdd" runat="server" Text="➕ Thêm tài khoản" OnClick="ButAdd_Click" CssClass="btn-base btn-primary" />
                </div>
            </div>
        </div>
        
        <hr class="my-6 border-gray-200"/>
        
        <div class="section-list">
            <h3 class="text-xl font-bold text-gray-800 mb-4">👤 Danh Sách Tài Khoản</h3>
            
            <div style="margin-bottom: 20px;" class="flex items-center space-x-3">
                <asp:TextBox ID="txtTuKhoa" runat="server" CssClass="form-input" placeholder="Tìm theo tên hoặc SĐT..." Width="300px" />
                <asp:Button ID="btnTimKiem" runat="server" Text="🔍 Tìm kiếm" OnClick="BtnTimKiem_Click" CssClass="btn-base btn-primary" />
            </div>
            
            <div class="overflow-x-auto">
                <asp:GridView ID="GridViewAccounts" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="So_dien_thoai" OnRowDeleting="GridView1_RowDeleting" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridViewAccounts_SelectedIndexChanged"
                    CssClass="gridview-style">
                    <Columns>
                        <asp:BoundField DataField="Ho_va_ten" HeaderText="Họ và Tên" />
                        <asp:BoundField DataField="So_dien_thoai" HeaderText="Số Điện Thoại" ReadOnly="True" />
                     
                        <asp:BoundField DataField="Dia_Chi" HeaderText="Địa Chỉ" />
                        <asp:BoundField DataField="Mat_khau" HeaderText="Mật Khẩu (MD5)" ReadOnly="True">
                             <ItemStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:HyperLinkField DataNavigateUrlFields="So_dien_thoai" DataNavigateUrlFormatString="EditAccount.aspx?ID={0}" Text="Sửa" 
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px" ControlStyle-CssClass="btn-edit" />
                        
                        <asp:CommandField DeleteText="Xóa" ShowDeleteButton="True" 
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px" />
                        
                        <asp:TemplateField HeaderText="Chọn">
                            <HeaderTemplate>
                                <asp:Button ID="butDelete" runat="server" OnClick="ButDelete_Click" Text="Xóa" CssClass="btn-base btn-return" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckhDelete" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="80px" />
                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="asp-header-style" />
                    <FooterStyle CssClass="asp-header-style" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>