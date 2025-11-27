<%@ Page Title="Quản lý Sản Phẩm" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="QL_BAN_HANG.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Quản Lý Sản Phẩm - Admin</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <style>
        /* ================================
           Layout chung (Đồng bộ với các trang Admin khác)
        ================================ */
        .admin-container {
            margin: 30px auto;
            width: 95%;
            max-width: 1400px;
            background-color: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        
        .admin-h2 {
            text-align: center;
            color: #2c3e50;
            margin-bottom: 20px;
            font-size: 1.875rem; /* text-3xl */
            font-weight: 700; /* font-bold */
        }
        
        .admin-h3 {
            color: #2980b9;
            margin-top: 25px;
            margin-bottom: 15px;
            padding-bottom: 5px;
            border-bottom: 2px solid #ecf0f1;
            font-size: 1.5rem; /* text-2xl */
            font-weight: 600; /* font-semibold */
        }

        /* ================================
           Control Area (Thêm / Lọc / Sửa)
        ================================ */
        .control-area {
            display: flex;
            align-items: center;
            flex-wrap: wrap;
            margin-bottom: 20px;
            gap: 20px; /* Khoảng cách giữa các phần tử */
            padding: 15px;
            border-radius: 8px;
            border: 1px solid #ccc;
        }
        
        /* Màu nền cho vùng thêm/sửa */
        .control-area.add-form {
            background-color: #e9f7e9; /* Màu xanh lá nhạt */
        }

        .control-area.edit-form {
            background-color: #fcf8e3; /* Màu vàng nhạt */
        }

        .control-area label {
            font-weight: bold;
            color: #2c3e50;
            white-space: nowrap; /* Giữ nhãn không bị ngắt dòng */
        }
        
        .control-area input[type="text"],
        .control-area select {
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 6px;
            min-width: 150px;
            box-sizing: border-box;
        }

        /* ================================
           Buttons (Đồng bộ)
        ================================ */
        .action-button,
        .link-action-base {
            padding: 8px 16px;
            border: none;
            border-radius: 5px;
            font-weight: bold;
            cursor: pointer;
            color: #fff;
            transition: background-color 0.3s;
        }

        .btn-add {
            background-color: #28a745;
        }
        .btn-add:hover {
            background-color: #1e7e34;
        }
        
        .btn-delete {
            background-color: #dc3545;
        }
        .btn-delete:hover {
            background-color: #c82333;
        }

        .btn-edit-save {
            background-color: #3498db;
        }
        .btn-edit-save:hover {
            background-color: #2980b9;
        }
        
        .btn-cancel {
            background-color: #95a5a6;
        }
        .btn-cancel:hover {
            background-color: #7f8c8d;
        }

        /* ================================
           GridView (Giữ lại style gốc, tối ưu hóa hiển thị)
        ================================ */
        .gridview-style {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }

        .gridview-style th {
            background-color: #507CD1; /* Giữ màu header */
            color: #fff;
            padding: 12px;
            font-size: 14px;
            font-weight: bold;
            text-align: center;
        }

        .gridview-style td {
            padding: 10px;
            border: 1px solid #ddd;
            text-align: center;
            vertical-align: middle;
        }
        
        .link-update {
            color: #27ae60;
            font-weight: bold;
            text-decoration: none;
            margin-left: 5px;
        }
        .link-update:hover {
            text-decoration: underline;
        }

        /* Nút Sửa trong GridView */
        .link-edit-gv {
            color: #2980b9;
            font-weight: bold;
            text-decoration: none;
            padding: 4px 8px;
            border: 1px solid #2980b9;
            border-radius: 4px;
        }
        .link-edit-gv:hover {
            background-color: #2980b9;
            color: #fff;
        }
        
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="admin-container">
        <h2 class="admin-h2">☕ QUẢN LÝ SẢN PHẨM</h2>
        
        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Bold="True" CssClass="block my-3"></asp:Label>

        <h3 class="admin-h3">🔍 Lọc Sản Phẩm</h3>
        <div class="control-area">
            <label>Danh mục:</label>
            <asp:DropDownList ID="ddlMenus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMenus_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        <h3 class="admin-h3">➕ Thêm Sản Phẩm Mới</h3>
        <div class="control-area add-form">
            
            <label>Tên SP:</label>
            <asp:TextBox ID="txtTenSP" runat="server" Placeholder="Nhập tên sản phẩm" />
            
            <label>Giá (VNĐ):</label>
            <asp:TextBox ID="txtGia" runat="server" Placeholder="Nhập giá (số)" />
            
            <label>Mô tả:</label>
            <asp:TextBox ID="txtMoTa" runat="server" Placeholder="Nhập mô tả" />
            
            <label>Danh mục:</label>
            <asp:DropDownList ID="ddlAddCategory" runat="server">
            </asp:DropDownList>
            
            <label>Trạng thái:</label>
            <asp:DropDownList ID="ddlStatus" runat="server">
            </asp:DropDownList>
            
            <label>Hình ảnh:</label>
            <asp:FileUpload ID="fileUploadHinhAnh" runat="server" />
            
            <asp:Button ID="butAdd" runat="server" Text="Thêm Sản Phẩm" OnClick="butAdd_Click" CssClass="action-button btn-add" />
        </div>
        
        <asp:Panel ID="pnlEditProduct" runat="server" Visible="false">
             <h3 class="admin-h3">✏️ Chỉnh Sửa Sản Phẩm: <asp:Label ID="lblEditProductName" runat="server" ForeColor="#e67e22"></asp:Label></h3>
            <div class="control-area edit-form flex flex-col md:flex-row md:justify-around items-start">
                <asp:HiddenField ID="hfProductID" runat="server" Value="-1" />
                
                <div class="w-full md:w-1/3 p-2">
                    <label>Tên SP:</label>
                    <asp:TextBox ID="txtEditTenSP" runat="server" CssClass="w-full" />
                    
                    <label>Giá (VNĐ):</label>
                    <asp:TextBox ID="txtEditGia" runat="server" CssClass="w-full" />
                    
                    <label>Mô tả:</label>
                    <asp:TextBox ID="txtEditMoTa" runat="server" TextMode="MultiLine" Rows="2" CssClass="w-full" />
                </div>
                
                <div class="w-full md:w-1/3 p-2">
                    <label>Danh mục:</label>
                    <asp:DropDownList ID="ddlEditCategory" runat="server" CssClass="w-full">
                    </asp:DropDownList>
                    
                    <label>Trạng thái:</label>
                    <asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="w-full">
                         <asp:ListItem Text="Còn hàng" Value="Còn hàng" />
                         <asp:ListItem Text="Hết hàng" Value="Hết hàng" />
                    </asp:DropDownList>
                    
                    <label>Trạng thái Hot:</label>
                    <asp:CheckBox ID="chkEditHot" runat="server" Text="Sản phẩm Hot" CssClass="block my-2" />

                    <label>Order Key:</label>
                    <asp:TextBox ID="txtEditOrderKey" runat="server" CssClass="w-24" />
                </div>
                
                <div class="w-full md:w-1/3 p-2 text-center">
                    <label>Hình ảnh hiện tại:</label>
                    <asp:Image ID="imgEditCurrent" runat="server" Width="100px" AlternateText="Ảnh hiện tại" CssClass="block mx-auto border p-1 rounded my-2" />
                    
                    <label>Thay hình ảnh (nếu cần):</label>
                    <asp:FileUpload ID="fileUploadEditHinhAnh" runat="server" />
                    <asp:HiddenField ID="hfCurrentHinhAnh" runat="server" />

                    <div class="mt-4">
                        <asp:Button ID="btnUpdateProduct" runat="server" Text="Lưu Cập Nhật" OnClick="btnUpdateProduct_Click" CssClass="action-button btn-edit-save" />
                        <asp:Button ID="btnCancelEdit" runat="server" Text="Hủy" OnClick="btnCancelEdit_Click" CssClass="action-button btn-cancel ml-2" />
                    </div>
                </div>
            </div>
        </asp:Panel>


        <hr class="my-8 border-t border-gray-300" />
        
        <h3 class="admin-h3">📋 Danh Sách Sản Phẩm</h3>

        <asp:GridView ID="GridViewProducts" runat="server" 
            AutoGenerateColumns="False" 
            DataKeyNames="ID_SP" 
            CssClass="gridview-style"
            AllowPaging="true"
            OnRowCommand="GridViewProducts_RowCommand"
            OnRowDeleting="GridViewProducts_RowDeleting"
            OnPageIndexChanging="GridViewProducts_PageIndexChanging">
            <PagerStyle CssClass="pgr" HorizontalAlign="Center" />
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ID_SP" HeaderText="Mã SP" ReadOnly="True" ItemStyle-Width="60px" />
                
                <asp:TemplateField HeaderText="Hình ảnh" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Image ID="imgSanPham" runat="server" 
                            ImageUrl='<%# Eval("Hinh_anh", "~/uploads/images/{0}") %>' 
                            Visible='<%# !string.IsNullOrEmpty(Eval("Hinh_anh") as string) %>'
                            AlternateText='<%# Eval("Ten_san_pham") %>'
                            Width="80px" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="Ten_san_pham" HeaderText="Tên Sản Phẩm" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                
                <asp:TemplateField HeaderText="Giá (VNĐ)" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Gia_co_ban", "{0:N0}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="ID_DM" HeaderText="ID Danh Mục" ItemStyle-Width="80px" />

                <asp:TemplateField HeaderText="Trạng Thái">
                    <ItemTemplate>
                        <asp:Label ID="lblTrangThai" runat="server" 
                            Text='<%# Eval("Trang_thai") %>'
                            ForeColor='<%# Eval("Trang_thai").ToString() == "Còn hàng" ? System.Drawing.Color.Green : System.Drawing.Color.Red %>'
                            Font-Bold="True"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Hot">
                    <ItemTemplate>
                        <asp:Label ID="lblHot" runat="server" 
                            Text='<%# Convert.ToBoolean(Eval("IsHot")) ? "🔥 HOT" : "---" %>'
                            ForeColor='<%# Convert.ToBoolean(Eval("IsHot")) ? System.Drawing.Color.Red : System.Drawing.Color.Gray %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Thứ tự" ItemStyle-Width="100px">
                     <ItemTemplate>
                        <asp:TextBox ID="txtOrderKey" runat="server" 
                            Text='<%# Eval("OrderKey") %>' Width="50px" CssClass="text-center border" />
                        <asp:LinkButton ID="btnUpdateOrderKey" runat="server" 
                            CommandName="UpdateOrderKey" 
                            CommandArgument='<%# Eval("ID_SP") %>' 
                            Text="Cập nhật" CssClass="link-update text-xs" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Hành động" ItemStyle-Width="100px">
                    <ItemTemplate>
                         <asp:LinkButton ID="btnEditProduct" runat="server" 
                            CommandName="EditProduct" 
                            CommandArgument='<%# Eval("ID_SP") %>' 
                            Text="Sửa" CssClass="link-edit-gv" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Chọn Xóa" ItemStyle-Width="60px">
                    <HeaderTemplate>
                        <asp:Button ID="butDelete" runat="server" OnClick="Button2_Click" CssClass="action-button btn-delete text-xs" Text="Xóa" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDelete" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:CommandField ShowDeleteButton="True" DeleteText="Xóa" ItemStyle-Width="60px" />

            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
        </asp:GridView>
    </div>
</asp:Content>