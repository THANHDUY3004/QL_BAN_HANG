<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ShoppingUser.aspx.cs" Inherits="QL_BAN_HANG.ShoppingUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/shopping.css" rel="stylesheet" />
    <style>
        /* CSS cho tabs đơn giản */
        .tab-container { margin-top: 30px; }
        .tab-buttons { display: flex; border-bottom: 1px solid #ddd; }
        .tab-button { padding: 10px 20px; cursor: pointer; background: #f9f9f9; border: none; border-bottom: 2px solid transparent; }
        .tab-button.active { background: #fff; border-bottom: 2px solid #007bff; font-weight: bold; }
        .tab-content { display: none; padding: 20px 0; }
        .tab-content.active { display: block; }
        
        /* Style cho GridView đơn hàng */
        .order-gridview { width: 100%; border-collapse: collapse; margin-top: 10px; }
        .order-gridview th, .order-gridview td { padding: 10px; text-align: left; border-bottom: 1px solid #ddd; }
        .order-gridview th { background-color: #f2f2f2; }
        .btn-detail { color: #007bff; text-decoration: none; }
        .btn-detail:hover { text-decoration: underline; }
        
        /* Modal style (đơn giản, có thể dùng Bootstrap modal nếu có) */
        .modal { display: none; position: fixed; z-index: 1; left: 0; top: 0; width: 100%; height: 100%; overflow: auto; background-color: rgba(0,0,0,0.4); }
        .modal-content { background-color: #fefefe; margin: 15% auto; padding: 20px; border: 1px solid #888; width: 80%; max-width: 600px; }
        .close { color: #aaa; float: right; font-size: 28px; font-weight: bold; cursor: pointer; }
        .close:hover { color: black; }
    </style>
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

        <!-- Phần mới: Lịch sử đơn hàng -->
        <div class="tab-container">
            <h2>📋 Lịch Sử Đơn Hàng Của Bạn</h2>
            <div class="tab-buttons">
                <button type="button" class="tab-button active" onclick="openTab(event, 'pending')">Đơn Hàng Đang Xử Lý</button>
                <button type="button" class="tab-button" onclick="openTab(event, 'completed')">Đơn Hàng Đã Hoàn Thành</button>
            </div>

            <!-- Tab Đơn Hàng Đang Xử Lý -->
            <div id="pending" class="tab-content active">
                <asp:GridView ID="gvPendingOrders" runat="server" AutoGenerateColumns="False" CssClass="order-gridview"
                    OnRowCommand="gvOrders_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ID_DH" HeaderText="ID Đơn" />
                        <asp:BoundField DataField="Thoi_gian_dat" HeaderText="Thời Gian Đặt" DataFormatString="{0:g}" />
                        <asp:BoundField DataField="Tong_tien" HeaderText="Tổng Tiền" DataFormatString="{0:N0} VNĐ" />
                        <asp:BoundField DataField="Trang_thai" HeaderText="Trạng Thái" />
                        <asp:TemplateField HeaderText="Hành Động">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandName="ViewDetail" 
                                    CommandArgument='<%# Eval("ID_DH") %>' Text="Xem Chi Tiết" CssClass="btn-detail" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblNoPendingOrders" runat="server" Text="Không có đơn hàng đang xử lý." Visible="false" />
            </div>

            <!-- Tab Đơn Hàng Đã Hoàn Thành -->
            <div id="completed" class="tab-content">
                <asp:GridView ID="gvCompletedOrders" runat="server" AutoGenerateColumns="False" CssClass="order-gridview"
                    OnRowCommand="gvOrders_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ID_DH" HeaderText="ID Đơn" />
                        <asp:BoundField DataField="Thoi_gian_dat" HeaderText="Thời Gian Đặt" DataFormatString="{0:g}" />
                        <asp:BoundField DataField="Tong_tien" HeaderText="Tổng Tiền" DataFormatString="{0:N0} VNĐ" />
                        <asp:BoundField DataField="Trang_thai" HeaderText="Trạng Thái" />
                        <asp:TemplateField HeaderText="Hành Động">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandName="ViewDetail" 
                                    CommandArgument='<%# Eval("ID_DH") %>' Text="Xem Chi Tiết" CssClass="btn-detail" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblNoCompletedOrders" runat="server" Text="Không có đơn hàng đã hoàn thành." Visible="false" />
            </div>
        </div>

        <!-- Modal Chi Tiết Đơn Hàng -->
        <asp:Panel ID="pnlDetailModal" runat="server" CssClass="modal">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h3>Chi Tiết Đơn Hàng</h3>
                <p><strong>ID Đơn:</strong> <asp:Label ID="lblOrderID" runat="server" /></p>
                <p><strong>Tên Khách Hàng:</strong> <asp:Label ID="lblCustomerName" runat="server" /></p>
                <p><strong>Số Điện Thoại:</strong> <asp:Label ID="lblPhone" runat="server" /></p>
                <p><strong>Địa Chỉ:</strong> <asp:Label ID="lblAddress" runat="server" /></p>
                <p><strong>Thời Gian Đặt:</strong> <asp:Label ID="lblOrderTime" runat="server" /></p>
                <p><strong>Trạng Thái:</strong> <asp:Label ID="lblStatusDetail" runat="server" /></p>
                <p><strong>Ghi Chú:</strong> <asp:Label ID="lblNote" runat="server" /></p>
                <p><strong>Tổng Tiền:</strong> <asp:Label ID="lblTotalDetail" runat="server" /></p>
                <h4>Danh Sách Sản Phẩm:</h4>
                <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="False" CssClass="order-gridview">
                    <Columns>
                        <asp:BoundField DataField="Ten_san_pham" HeaderText="Tên Sản Phẩm" />
                        <asp:BoundField DataField="So_luong" HeaderText="Số Lượng" />
                        <asp:BoundField DataField="Gia_tai_thoi_diem" HeaderText="Giá" DataFormatString="{0:N0} VNĐ" />
                        <asp:BoundField DataField="Ghi_chu_item" HeaderText="Ghi Chú" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>

    <script>
        // JavaScript cho tabs
        function openTab(evt, tabName) {
            var i, tabcontent, tabbuttons;
            tabcontent = document.getElementsByClassName("tab-content");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            tabbuttons = document.getElementsByClassName("tab-button");
            for (i = 0; i < tabbuttons.length; i++) {
                tabbuttons[i].className = tabbuttons[i].className.replace(" active", "");
            }
            document.getElementById(tabName).style.display = "block";
            evt.currentTarget.className += " active";
        }

        // Đóng modal
        function closeModal() {
            document.getElementById('<%= pnlDetailModal.ClientID %>').style.display = 'none';
        // Nếu cần gọi code-behind, uncomment dòng dưới (nhưng không cần thiết nếu dùng script trực tiếp)
            // __doPostBack('<%= pnlDetailModal.ClientID %>', 'Close');
        }
    </script>
</asp:Content>
