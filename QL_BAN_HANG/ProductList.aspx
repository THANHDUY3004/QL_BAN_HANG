<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="QL_BAN_HANG.ProductList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/product.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
            <div class="container">
            <h2> QUẢN LÝ SẢN PHẨM</h2>
            
            <div class="control-area">
                <label>Danh mục:</label>
                <asp:DropDownList ID="ddlMenus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMenus_SelectedIndexChanged">
                </asp:DropDownList>
            </div>

            <h3>➕ Thêm Sản Phẩm Mới</h3>
            <div class="control-area" style="background-color: #e9f7e9;">
                <label>Tên SP:</label><asp:TextBox ID="txtTenSP" runat="server" />
                <label>Giá (VNĐ):</label><asp:TextBox ID="txtGia" runat="server" />
                <label>Mô tả:</label><asp:TextBox ID="txtMoTa" runat="server" />
                
                <label>Danh mục:</label>
                <asp:DropDownList ID="ddlAddCategory" runat="server">
                </asp:DropDownList>
                
                <label>Trạng thái:<asp:DropDownList ID="DropDownList1" runat="server">
                </asp:DropDownList>
                Hình ảnh:</label>
                <asp:FileUpload ID="fileUploadHinhAnh" runat="server" />
                
                <asp:Button ID="butAdd" runat="server" Text="Thêm Sản Phẩm" OnClick="butAdd_Click" CssClass="action-button btn-add" />
            </div>

            <b><asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label></b>
            
            <h3>📋 Danh Sách Sản Phẩm</h3>

            <asp:GridView ID="GridViewProducts" runat="server" 
                AutoGenerateColumns="False" 
                CellPadding="4" 
                DataKeyNames="ID_SP" 
                ForeColor="#333333" 
                GridLines="None" 
                CssClass="gridview-style"
                OnRowDeleting="GridViewProducts_RowDeleting"
                OnRowEditing="GridViewProducts_RowEditing"
                OnRowUpdating="GridViewProducts_RowUpdating"
                OnRowCancelingEdit="GridViewProducts_RowCancelingEdit"
                >
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ID_SP" HeaderText="Mã SP" ReadOnly="True" ItemStyle-Width="60px" >
                    
<ItemStyle Width="60px"></ItemStyle>
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Hình ảnh" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Image ID="imgSanPham" runat="server" 
                                ImageUrl='<%# Eval("Hinh_anh", "~/uploads/images/{0}") %>' 
                                Visible='<%# !string.IsNullOrEmpty(Eval("Hinh_anh") as string) %>'
                                AlternateText='<%# Eval("Ten_san_pham") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditHinhAnh" runat="server" Text='<%# Bind("Hinh_anh") %>' Width="100px"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Tên Sản Phẩm" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Ten_san_pham") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditTenSP" runat="server" Text='<%# Bind("Ten_san_pham") %>' Width="200px"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Giá (VNĐ)" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                         <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Gia_co_ban", "{0:N0}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditGia" runat="server" Text='<%# Bind("Gia_co_ban") %>' Width="100px"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle HorizontalAlign="Right" Width="120px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ID Danh Mục" ItemStyle-Width="100px">
                         <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("ID_MN") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditIDMN" runat="server" Text='<%# Bind("ID_MN") %>' Width="80px"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle Width="100px"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Trạng Thái (Còn Hàng)" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <%-- Hiển thị Checkbox chỉ để xem (readonly) --%>
                            <asp:CheckBox ID="chkViewTrangThai" runat="server" 
                                Enabled="false" 
                                Checked='<%# Eval("Trang_thai").ToString() == "Còn hàng" %>' />
                            <%-- Hiển thị trạng thái bằng text --%>
                            <asp:Label ID="lblTrangThai" runat="server" Text='<%# Eval("Trang_thai") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%-- Dùng Eval để đặt trạng thái ban đầu --%>
                            <asp:CheckBox ID="chkEditTrangThai" runat="server" 
                                Text="Còn Hàng" 
                                Checked='<%# Eval("Trang_thai").ToString() == "Còn hàng" %>' />
                        </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Chọn Xóa" ItemStyle-Width="80px">
                        <HeaderTemplate>
                            <asp:Button ID="butDelete" runat="server" OnClick="Button2_Click" CssClass="action-button btn-delete" Text="Xóa" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDelete" runat="server" />
                        </ItemTemplate>

<ItemStyle Width="80px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" EditText="Sửa" UpdateText="Lưu" CancelText="Hủy" ItemStyle-Width="80px" >
<ItemStyle Width="80px"></ItemStyle>
                    </asp:CommandField>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Xóa" ItemStyle-Width="80px" >
<ItemStyle Width="80px"></ItemStyle>
                    </asp:CommandField>
                </Columns>
                <EditRowStyle BackColor="#FFFFCC" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>

        </div>
</asp:Content>

