<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="Default_admin.aspx.cs" Inherits="QL_BAN_HANG.Default_admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/default_admin.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
            <div class="container">
            <h2> QUẢN LÝ BÀI VIẾT</h2>
            
            <div class="control-area filter-area">
                <label>Lọc theo Danh mục:</label>
                <asp:DropDownList ID="ddlFilterMenus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFilterMenus_SelectedIndexChanged">
                </asp:DropDownList>
                
                <asp:Button ID="butDeleteSelected" runat="server" Text="Xóa Bài Viết Đã Chọn" OnClick="butDeleteSelected_Click" CssClass="action-button btn-delete" />
            </div>

            <h3>➕ Thêm Bài Viết Mới</h3>
            <div class="control-area" style="background-color: #e9f7e9;">
                <label>Danh mục (Menu):</label>
                <asp:DropDownList ID="ddlAddMenu" runat="server">
                </asp:DropDownList>

                <label>Thứ tự (OrderKey):</label>
                <asp:TextBox ID="txtAddOrderKey" runat="server" Text="1" Width="100px" />
                
                <label>Tiêu đề:</label>
                <asp:TextBox ID="txtAddTieuDe" runat="server" />

                <label>Tóm tắt:</label>
                <asp:TextBox ID="txtAddTomTat" runat="server" TextMode="MultiLine" Rows="3" />

                <label>Nội dung đầy đủ:</label>
                <asp:TextBox ID="txtAddNoiDung" runat="server" TextMode="MultiLine" Rows="6" />

                <label>Hình ảnh đại diện:</label>
                <asp:FileUpload ID="fileUploadHinhAnh" runat="server" />

                <div class="full-width">
                    <asp:Button ID="butAdd" runat="server" Text="Thêm Bài Viết" OnClick="butAdd_Click" CssClass="action-button btn-add" />
                </div>
            </div>

            <b style="color: red; margin-top: 15px; display: inline-block;"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></b>
            
            <h3>📋 Danh Sách Bài Viết</h3>

            <asp:GridView ID="GridViewBaiViet" runat="server" 
                AutoGenerateColumns="False" 
                CellPadding="4" 
                DataKeyNames="ID_BV" 
                ForeColor="#333333" 
                GridLines="None" 
                CssClass="gridview-style"
                OnRowDeleting="GridViewBaiViet_RowDeleting"
                OnRowEditing="GridViewBaiViet_RowEditing"
                OnRowUpdating="GridViewBaiViet_RowUpdating"
                OnRowCancelingEdit="GridViewBaiViet_RowCancelingEdit" OnSelectedIndexChanged="GridViewBaiViet_SelectedIndexChanged"
                >
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ID_BV" HeaderText="ID_BV" ReadOnly="True" ItemStyle-Width="50px" >
                    
<ItemStyle Width="50px"></ItemStyle>
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Hình ảnh" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Image ID="imgBaiViet" runat="server" 
                                ImageUrl='<%# Eval("Hinh_anh_page", "~/Images/{0}") %>' 
                                Visible='<%# !string.IsNullOrEmpty(Eval("Hinh_anh_page") as string) %>'
                                AlternateText='<%# Eval("Tieu_de") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditHinhAnh" runat="server" Text='<%# Bind("Hinh_anh_page") %>' Width="100px"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Tiêu đề" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Tieu_de") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditTieuDe" runat="server" Text='<%# Bind("Tieu_de") %>' Width="95%"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Tóm tắt" ItemStyle-CssClass="col-tomtat">
                         <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Tom_tac") %>'></asp:Label>
                         </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditTomTat" runat="server" Text='<%# Bind("Tom_tac") %>' Width="95%" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle CssClass="col-tomtat"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="ID Menu" ItemStyle-Width="80px">
                         <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("ID_MN") %>'></asp:Label>
                         </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditIDMN" runat="server" Text='<%# Bind("ID_MN") %>' Width="60px"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle Width="80px"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Thứ tự" ItemStyle-Width="80px">
                         <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("OrderKey") %>'></asp:Label>
                         </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditOrderKey" runat="server" Text='<%# Bind("OrderKey") %>' Width="60px"></asp:TextBox>
                        </EditItemTemplate>

<ItemStyle Width="80px"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Chọn Xóa" ItemStyle-Width="70px">
                        <HeaderTemplate>
                            <asp:Button ID="butDelete" runat="server" OnClick="butDelete_Click" Text="Xóa" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDelete" runat="server" />
                        </ItemTemplate>

<ItemStyle Width="70px"></ItemStyle>
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
            </asp:GridView>

        </div>
</asp:Content>