<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ShoppingUser.aspx.cs" Inherits="QL_BAN_HANG.ShoppingUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/shopping.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="container">
        <h2>🛒 Giỏ Hàng Của Bạn</h2>

        <!-- Thông báo -->
        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>

        <!-- GridView giỏ hàng -->
        <asp:GridView ID="gvGioHang" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="ID_GH"
            CssClass="gridview-style"
            OnRowCommand="gvGioHang_RowCommand"
            OnDataBound="gvGioHang_DataBound">
            <Columns>
                <asp:TemplateField HeaderText="Chọn" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkChonThanhToan" runat="server" Checked="true" />
                    </ItemTemplate>

<ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Sản Phẩm" ItemStyle-Width="120px">
                    <ItemTemplate>
                        <asp:Image ID="imgHinhAnh" runat="server" 
                            ImageUrl='<%# Eval("Hinh_anh", "~/uploads/images/{0}") %>' 
                            Visible='<%# !string.IsNullOrEmpty(Eval("Hinh_anh") as string) %>'
                            AlternateText='<%# Eval("Ten_san_pham") %>' Width="80px" Height="80px" />
                    </ItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Tên Sản Phẩm">
                    <ItemTemplate>
                        <div class="product-name"><%# Eval("Ten_san_pham") %></div>
                        <div style="font-size: 0.9em;">Đơn giá:
                            <span class="price"><%# Eval("Gia_tai_thoi_diem", "{0:N0} VNĐ") %></span>
                        </div>
                        <div style="margin-top: 8px;">Ghi chú:
                            <asp:TextBox ID="txtGhiChuItem" runat="server" Text='<%# Bind("Ghi_chu") %>'
                                Width="150px" placeholder="Ít đá, thêm topping..." />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Số Lượng" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSoLuong" runat="server" Text='<%# Bind("So_luong") %>'
                            Width="50px" TextMode="Number" />
                    </ItemTemplate>

<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Thành Tiền" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <div class="price">
                            <%# string.Format("{0:N0} VNĐ",
                                Convert.ToDecimal(Eval("So_luong")) * Convert.ToDecimal(Eval("Gia_tai_thoi_diem"))) %>
                        </div>
                    </ItemTemplate>

<ItemStyle HorizontalAlign="Right" Width="150px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Hành Động" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" Text="Cập nhật"
                            CommandName="CapNhatItem" CommandArgument='<%# Eval("ID_GH") %>'
                            CssClass="action-button btn-update" />
                        <br /><br />
                        <asp:LinkButton ID="btnDelete" runat="server" Text="Xóa"
                            CommandName="XoaItem" CommandArgument='<%# Eval("ID_GH") %>'
                            CssClass="action-button btn-delete"
                            OnClientClick="return confirm('Bạn có chắc muốn xóa sản phẩm này?');" />
                    </ItemTemplate>

<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:Label ID="lblThongBaoTrong" runat="server" Text="Giỏ hàng của bạn đang trống."
            Visible="false" Font-Size="Large" />

        <div class="checkout-area">
            <div class="checkout-form">
                <h3>Thông Tin Đặt Hàng</h3>
                <div class="form-group">
                    <label>Số điện thoại (Người nhận):</label>
                    <asp:TextBox ID="txtSoDienThoai" runat="server"/>
                </div>
                <div class="form-group">
                    <label>Địa chỉ giao hàng:</label>
                    <asp:TextBox ID="txtDiaChiGiaoHang" runat="server" />
                </div>
                <div class="form-group">
                    <label>Hình thức đặt hàng:</label>
                    <asp:DropDownList ID="ddlHinhThucDatDon" runat="server" Width="100%">
                        <asp:ListItem Value="Thanh toán khi nhận hàng" Selected="True">
                            Thanh toán khi nhận hàng (COD)
                        </asp:ListItem>
                        <asp:ListItem Value="Chuyển khoản">Chuyển khoản</asp:ListItem>
                        <asp:ListItem Value="Ví điện tử">Ví điện tử</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Ghi chú chung (cho đơn hàng):</label>
                    <asp:TextBox ID="txtGhiChuChung" runat="server" TextMode="MultiLine" Rows="3" />
                </div>
                <div class="form-group" style="text-align: right;">
                    <asp:Button ID="btnDatHang" runat="server" Text="TIẾN HÀNH ĐẶT HÀNG"
                        OnClick="btnDatHang_Click" CssClass="action-button btn-checkout" />
                </div>
            </div>

            <div class="summary-total">
                TỔNG TIỀN CẦN THANH TOÁN:
                <br />
                <asp:Label ID="lblTongTien" runat="server" Text="0 VNĐ" />
            </div>
        </div>
    </div>
</asp:Content>
