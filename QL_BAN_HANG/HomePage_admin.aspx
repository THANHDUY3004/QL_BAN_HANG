<%@ Page Title="Quản lý Trang Chủ" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="HomePage_admin.aspx.cs" Inherits="QL_BAN_HANG.HomePage_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Quản Lý Trang Chủ - Admin</title>
    <script src="https://cdn.tailwindcss.com"></script>
    
    <style>
        /* Tái tạo .container của Default_admin.aspx */
        .admin-container {
            margin: 30px auto;
            width: 95%;
            max-width: 1200px;
            background-color: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        
        /* Tái tạo style h2 */
        .admin-h2 {
            text-align: center;
            color: #2c3e50;
            margin-bottom: 20px;
            font-size: 1.875rem; /* text-3xl */
            font-weight: 700; /* font-bold */
        }

        /* Tái tạo style h3 */
        .admin-h3 {
            color: #2980b9;
            margin-top: 25px;
            margin-bottom: 15px;
            padding-bottom: 5px;
            border-bottom: 2px solid #ecf0f1;
            font-size: 1.5rem; /* text-2xl */
            font-weight: 600; /* font-semibold */
        }
        
        /* Màu nền cho vùng thêm/sửa */
        .control-area-add {
            border: 1px solid #ccc;
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 25px;
        }

        .control-area-add.slider {
            background-color: #e9f7e9; /* Màu xanh nhạt */
        }

        .control-area-add.shop {
            background-color: #f7e9e9; /* Màu hồng nhạt (tùy chỉnh) */
        }

        /* Đồng bộ hóa style Input/Label */
        .control-area-add label {
            display: block;
            font-weight: bold;
            margin-top: 10px;
            color: #2c3e50;
        }
        
        .control-area-add input[type="text"],
        .control-area-add textarea {
             padding: 8px;
             border: 1px solid #ccc;
             border-radius: 6px;
             margin-top: 5px;
             margin-bottom: 10px;
        }


        /* Tái tạo GridView */
        .gridview-style {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.05);
        }

        .gridview-style th {
            background-color: #3498db;
            color: #fff;
            padding: 10px;
            text-align: center;
        }

        .gridview-style td {
            padding: 10px;
            text-align: center;
            border-bottom: 1px solid #eee;
            vertical-align: middle;
        }

        .gridview-style tr:hover {
            background-color: #f9f9f9;
        }
        
        /* Style cho nút hành động */
        .btn-action-base {
            padding: 5px 10px;
            margin: 2px;
            display: inline-block;
            font-weight: bold;
            text-decoration: none;
            border-radius: 4px;
            transition: background-color 0.3s ease;
        }
        
        .link-edit {
            color: #2980b9;
            border: 1px solid #2980b9;
        }
        .link-edit:hover {
            color: #fff;
            background-color: #2980b9;
        }

        .link-delete {
            color: #e74c3c;
            border: 1px solid #e74c3c;
        }
        .link-delete:hover {
            color: #fff;
            background-color: #e74c3c;
        }
        
        /* Style cho nút Thêm/Hiện Form (Button) */
        .btn-show-form {
            background-color: #3498db;
            color: #fff;
            padding: 10px 20px;
            border-radius: 6px;
            font-weight: bold;
        }
        .btn-show-form:hover {
             background-color: #2980b9;
        }

        .btn-add-submit {
            background-color: #27ae60;
            color: #fff;
            padding: 10px 20px;
            border-radius: 6px;
            font-weight: bold;
        }
        .btn-add-submit:hover {
             background-color: #2ecc71;
        }
        
        .btn-cancel {
            background-color: #95a5a6;
            color: #fff;
            padding: 10px 20px;
            border-radius: 6px;
            font-weight: bold;
        }
        .btn-cancel:hover {
             background-color: #7f8c8d;
        }

    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="admin-container">
        <h2 class="admin-h2">🏠 QUẢN LÝ TRANG CHỦ</h2>

        <section class="mb-8">
            <h3 class="admin-h3">🖼️ Quản Lý Slider Images</h3>
            
            <asp:Button ID="btnShowSliderForm" runat="server" Text="Thêm Slider Mới" OnClick="btnShowSliderForm_Click" CssClass="btn-show-form mb-4" />
            
            <asp:Panel ID="pnlSliderForm" runat="server" Visible="false" CssClass="control-area-add slider">
                <asp:HiddenField ID="hfSliderID" runat="server" Value="-1" />
                <label>Tiêu đề (Title):</label>
                <asp:TextBox ID="txtSliderTitle" runat="server" placeholder="Title" CssClass="w-full" />
                
                <label>Mô tả (Description):</label>
                <asp:TextBox ID="txtSliderDesc" runat="server" placeholder="Description" TextMode="MultiLine" Rows="2" CssClass="w-full" />
                
                <label>Thứ tự (OrderKey):</label>
                <asp:TextBox ID="txtSliderOrder" runat="server" placeholder="Order Key" CssClass="w-24" />
                
                <label>Hình ảnh:</label>
                <asp:FileUpload ID="fuSlider" runat="server" CssClass="mb-2" />
                
                <div class="mt-2">
                    <asp:CheckBox ID="chkSliderActive" runat="server" Text="Hoạt động" />
                </div>
                
                <div class="text-center mt-5">
                    <asp:Button ID="btnAddSlider" runat="server" Text="Thêm Mới" OnClick="btnAddSlider_Click" CssClass="btn-add-submit" />
                    <asp:Button ID="btnCancelSlider" runat="server" Text="Hủy" OnClick="btnCancelSlider_Click" CssClass="btn-cancel ml-2" />
                </div>
            </asp:Panel>

            <asp:GridView ID="gvSlider" runat="server" 
                AutoGenerateColumns="false" 
                OnRowCommand="gvSlider_RowCommand" 
                CssClass="gridview-style"
                DataKeyNames="ID">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Width="50px" />
                    <asp:TemplateField HeaderText="Hình ảnh">
                        <ItemTemplate>
                             <asp:Image ID="imgSlider" runat="server" 
                                ImageUrl='<%# "~/uploads/images/" + Eval("ImageUrl") %>' 
                                Visible='<%# !string.IsNullOrEmpty(Eval("ImageUrl") as string) %>'
                                Width="100px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Title" HeaderText="Tiêu đề" />
                    <asp:BoundField DataField="Description" HeaderText="Mô tả" ItemStyle-Width="30%" />
                    <asp:BoundField DataField="OrderKey" HeaderText="Thứ tự" ItemStyle-Width="70px" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ItemStyle-Width="70px" />
                    <asp:TemplateField HeaderText="Hành động" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditSlider" CommandArgument='<%# Eval("ID") %>' Text="Sửa" CssClass="btn-action-base link-edit" />
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteSlider" CommandArgument='<%# Eval("ID") %>' Text="Xóa" CssClass="btn-action-base link-delete" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa Slider này?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </section>
        
        <hr class="my-8 border-t border-gray-300" />

        <section class="mb-8">
             <h3 class="admin-h3">🏬 Quản Lý Shop Images (Banner)</h3>
            
            <asp:Button ID="btnShowShopForm" runat="server" Text="Thêm Shop Image Mới" OnClick="btnShowShopForm_Click" CssClass="btn-show-form mb-4" />
            
            <asp:Panel ID="pnlShopForm" runat="server" Visible="false" CssClass="control-area-add shop">
                <asp:HiddenField ID="hfShopID" runat="server" Value="-1" />
                <label>Text thay thế (Alt Text):</label>
                <asp:TextBox ID="txtShopAlt" runat="server" placeholder="Alt Text" CssClass="w-full" />
                
                <label>Thứ tự (OrderKey):</label>
                <asp:TextBox ID="txtShopOrder" runat="server" placeholder="Order Key" CssClass="w-24" />
                
                <label>Hình ảnh:</label>
                <asp:FileUpload ID="fuShop" runat="server" CssClass="mb-2" />
                
                <div class="mt-2">
                    <asp:CheckBox ID="chkShopActive" runat="server" Text="Hoạt động" />
                </div>
                
                <div class="text-center mt-5">
                    <asp:Button ID="btnAddShop" runat="server" Text="Thêm Mới" OnClick="btnAddShop_Click" CssClass="btn-add-submit" />
                    <asp:Button ID="btnCancelShop" runat="server" Text="Hủy" OnClick="btnCancelShop_Click" CssClass="btn-cancel ml-2" />
                </div>
            </asp:Panel>

            <asp:GridView ID="gvShop" runat="server" 
                AutoGenerateColumns="false" 
                OnRowCommand="gvShop_RowCommand" 
                CssClass="gridview-style"
                DataKeyNames="ID">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Width="50px" />
                    <asp:TemplateField HeaderText="Hình ảnh">
                        <ItemTemplate>
                             <asp:Image ID="imgShop" runat="server" 
                                ImageUrl='<%# "~/uploads/images/" + Eval("ImageUrl") %>' 
                                Visible='<%# !string.IsNullOrEmpty(Eval("ImageUrl") as string) %>'
                                Width="100px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AltText" HeaderText="Alt Text" />
                    <asp:BoundField DataField="OrderKey" HeaderText="Thứ tự" ItemStyle-Width="70px" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ItemStyle-Width="70px" />
                    <asp:TemplateField HeaderText="Hành động" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditShop" CommandArgument='<%# Eval("ID") %>' Text="Sửa" CssClass="btn-action-base link-edit" />
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteShop" CommandArgument='<%# Eval("ID") %>' Text="Xóa" CssClass="btn-action-base link-delete" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa Shop Image này?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </section>
    </div>
</asp:Content>