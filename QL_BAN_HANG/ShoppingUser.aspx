<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ShoppingUser.aspx.cs" Inherits="QL_BAN_HANG.ShoppingUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/shopping.css" rel="stylesheet" />
    <style>
        /* CSS cho tabs đơn giản */
        .tab-container { margin-top: 30px; }
        .tab-buttons { display: flex; border-bottom: 1px solid #ddd; }
        .tab-button { 
            padding: 10px 20px; 
            cursor: pointer; 
            background: #f9f9f9; 
            border: 1px solid #ddd; /* Thêm border để tách biệt rõ hơn */
            border-bottom: 2px solid transparent; 
            margin-right: 5px; 
            border-radius: 5px 5px 0 0;
            transition: background 0.3s;
        }
        .action-button {
            text-decoration: none;
        }
        .tab-button.active { 
            background: #fff; 
            border-bottom: 2px solid #007bff; 
            font-weight: bold; 
            border-color: #007bff #007bff #fff #007bff; /* Trừ border-bottom */
        }
        .tab-content { display: none; padding: 20px 0; border-top: 1px solid #ddd; }
        .tab-content.active { display: block; }
        
        /* Style cho GridView đơn hàng */
        .order-gridview { width: 100%; border-collapse: collapse; margin-top: 10px; }
        .order-gridview th, .order-gridview td { padding: 10px; text-align: left; border-bottom: 1px solid #ddd; }
        .order-gridview th { background-color: #f2f2f2; }
        .btn-detail { color: #007bff; text-decoration: none; font-size: 0.9em; }
        .btn-detail:hover { text-decoration: underline; }
        
        /* Layout Giỏ hàng và Thanh toán */
        .checkout-area { 
            display: flex; 
            justify-content: space-between; 
            align-items: flex-start;
            margin-top: 30px; 
            padding-top: 20px;
            border-top: 1px dashed #ccc;
        }
        .checkout-form { width: 60%; background: #f9f9f9; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.05); }
        .summary-total { 
            width: 35%; 
            padding: 20px; 
            text-align: right; 
            font-size: 1.2em;
            background: #eef;
            border-radius: 8px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.05);
        }
        .summary-total #lblTongTien { 
            font-size: 2em; 
            color: #e74c3c; 
            font-weight: bold; 
            display: block; 
            margin-top: 10px;
        }
        .form-group { margin-bottom: 15px; }
        .form-group label { font-weight: bold; display: block; margin-bottom: 5px; }
        .form-group input[type="text"], .form-group textarea, .form-group select {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        /* Style cho Modal */
        .modal { display: none; position: fixed; z-index: 1000; left: 0; top: 0; width: 100%; height: 100%; overflow: auto; background-color: rgba(0,0,0,0.6); }
        .modal-content { background-color: #fefefe; margin: 5% auto; padding: 25px; border: 1px solid #888; width: 90%; max-width: 700px; border-radius: 10px; box-shadow: 0 5px 15px rgba(0,0,0,0.3); }
        .modal-content h3, .modal-content h4 { color: #007bff; border-bottom: 1px solid #eee; padding-bottom: 10px; margin-bottom: 15px; }
        .close { color: #aaa; float: right; font-size: 36px; font-weight: bold; cursor: pointer; }
        .close:hover, .close:focus { color: #c0392b; text-decoration: none; }
        
        /* Responsive cho checkout-area */
        @media (max-width: 768px) {
            .checkout-area { flex-direction: column; }
            .checkout-form, .summary-total { width: 100%; margin-bottom: 20px; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="container">
        <h2>🛒 Giỏ Hàng Hiện Tại</h2>

        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="#c0392b" Font-Bold="true" Style="display: block; margin-bottom: 15px;"></asp:Label>

        <asp:GridView ID="gvGioHang" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="ID_GH"
            CssClass="gridview-style"
            OnRowCommand="gvGioHang_RowCommand"
            OnDataBound="gvGioHang_DataBound"
            EmptyDataText="Giỏ hàng của bạn đang trống.">
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
                        <div class="product-name" style="font-weight: bold;"><%# Eval("Ten_san_pham") %></div>
                        <div style="font-size: 0.9em; margin-top: 5px;">Đơn giá:
                            <span class="price" style="color: #e74c3c;"><%# Eval("Gia_co_ban", "{0:N0} VNĐ") %></span>
                        </div>
                        <div style="margin-top: 8px; font-size: 0.9em;">Ghi chú:
                            <asp:TextBox ID="txtGhiChuItem" runat="server" Text='<%# Bind("Ghi_chu") %>'
                                Width="180px" placeholder="Ít đá, thêm topping..." />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Số Lượng" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSoLuong" runat="server" Text='<%# Bind("So_luong") %>'
                            Width="50px" TextMode="Number" min="1" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Thành Tiền" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <div class="price" style="font-weight: bold; color: #2ecc71;">
                            <%# string.Format("{0:N0} VNĐ",
                                Convert.ToDecimal(Eval("So_luong")) * Convert.ToDecimal(Eval("Gia_co_ban"))) %>
                        </div>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" Width="150px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Hành Động" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" Text="🔄 Cập nhật"
                            CommandName="CapNhatItem" CommandArgument='<%# Eval("ID_GH") %>'
                            CssClass="action-button btn-update" />
                        <br /><br />
                        <asp:LinkButton ID="btnDelete" runat="server" Text="🗑️ Xóa"
                            CommandName="XoaItem" CommandArgument='<%# Eval("ID_GH") %>'
                            CssClass="action-button btn-delete"
                            OnClientClick="return confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ?');" />
                    </ItemTemplate>
                    <HeaderStyle Width="150px" />
                    <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <div class="checkout-area">
            <div class="checkout-form">
                <h3>Thông Tin Đặt Hàng</h3>
                <div class="form-group">
                    <label>Số điện thoại (Người nhận):</label>
                    <asp:TextBox ID="txtSoDienThoai" runat="server" placeholder="Vui lòng nhập SĐT nhận hàng" />
                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtSoDienThoai"
                        ErrorMessage="SĐT không được để trống!" ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="form-group">
                    <label>Địa chỉ giao hàng:</label>
                    <asp:TextBox ID="txtDiaChiGiaoHang" runat="server" placeholder="Vui lòng nhập địa chỉ nhận hàng" />
                    <asp:Label ID="lblchackdiachi" runat="server" Text=""></asp:Label>
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
                    <asp:TextBox ID="txtGhiChuChung" runat="server" TextMode="MultiLine" Rows="3" placeholder="Ghi chú thêm về thời gian giao, yêu cầu đặc biệt..." />
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

        <div class="tab-container">
            <h2>📋 Lịch Sử Đơn Hàng Của Bạn</h2>
            <div class="tab-buttons">
                <button type="button" class="tab-button active" onclick="openTab(event, 'pending')">Đơn Hàng Đang Xử Lý</button>
                <button type="button" class="tab-button" onclick="openTab(event, 'completed')">Đơn Hàng Đã Hoàn Thành</button>
            </div>

            <div id="pending" class="tab-content active">
                <asp:GridView ID="gvPendingOrders" runat="server" AutoGenerateColumns="False" CssClass="order-gridview"
                    OnRowCommand="gvOrders_RowCommand" EmptyDataText="Không có đơn hàng đang xử lý.">
                    <Columns>
                        <asp:BoundField DataField="ID_DH" HeaderText="ID Đơn" />
                        <asp:BoundField DataField="Ngay_tao" HeaderText="Thời Gian Đặt" DataFormatString="{0:g}" />
                        <asp:BoundField DataField="Tong_tien" HeaderText="Tổng Tiền" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Trang_thai_don" HeaderText="Trạng Thái" />
                        <asp:TemplateField HeaderText="Hành Động">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandName="ViewDetail" 
                                    CommandArgument='<%# Eval("ID_DH") %>' Text="Xem Chi Tiết" CssClass="btn-detail" />
                                <asp:LinkButton ID="lnkCancelOrder" runat="server" CommandName="CancelOrder" 
                                    CommandArgument='<%# Eval("ID_DH") %>' Text=" | Hủy Đơn" CssClass="btn-detail" ForeColor="#e74c3c"
                                    OnClientClick="return confirm('Bạn có chắc muốn HỦY đơn hàng này? Thao tác không thể hoàn tác.');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>

            <div id="completed" class="tab-content">
                <asp:GridView ID="gvCompletedOrders" runat="server" AutoGenerateColumns="False" CssClass="order-gridview"
                    OnRowCommand="gvOrders_RowCommand" EmptyDataText="Không có đơn hàng đã hoàn thành.">
                    <Columns>
                        <asp:BoundField DataField="ID_DH" HeaderText="ID Đơn" />
                        <asp:BoundField DataField="Ngay_tao" HeaderText="Thời Gian Đặt" DataFormatString="{0:g}" />
                        <asp:BoundField DataField="Tong_tien" HeaderText="Tổng Tiền" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Trang_thai_don" HeaderText="Trạng Thái" />
                        <asp:TemplateField HeaderText="Hành Động">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandName="ViewDetail" 
                                    CommandArgument='<%# Eval("ID_DH") %>' Text="Xem Chi Tiết" CssClass="btn-detail" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
        </div>

        <asp:Panel ID="pnlDetailModal" runat="server" CssClass="modal">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h3>Chi Tiết Đơn Hàng <asp:Label ID="lblOrderID" runat="server" /></h3>
                <div style="display: flex; justify-content: space-between;">
                    <div style="width: 48%;">
                        <p><strong>Tên Khách Hàng:</strong> <asp:Label ID="lblCustomerName" runat="server" /></p>
                        <p><strong>Số Điện Thoại:</strong> <asp:Label ID="lblPhone" runat="server" /></p>
                        <p><strong>Địa Chỉ:</strong> <asp:Label ID="lblAddress" runat="server" /></p>
                        <p><strong>Thời Gian Đặt:</strong> <asp:Label ID="lblOrderTime" runat="server" /></p>
                    </div>
                    <div style="width: 48%;">
                        <p><strong>Trạng Thái:</strong> <asp:Label ID="lblStatusDetail" runat="server" ForeColor="#007bff" Font-Bold="true" /></p>
                        <p><strong>Hình Thức:</strong> <asp:Label ID="lblPaymentMethod" runat="server" /></p>
                        <p><strong>Ghi Chú Chung:</strong> <asp:Label ID="lblNote" runat="server" /></p>
                        <p><strong>TỔNG TIỀN:</strong> <asp:Label ID="lblTotalDetail" runat="server" ForeColor="#e74c3c" Font-Size="Large" Font-Bold="true" /></p>
                    </div>
                </div>
                
                <h4 style="margin-top: 20px;">Danh Sách Sản Phẩm:</h4>
                <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="False" CssClass="order-gridview">
                    <Columns>
                        <asp:BoundField DataField="Ten_san_pham" HeaderText="Tên Sản Phẩm" />
                        <asp:BoundField DataField="So_luong" HeaderText="SL" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Gia_Ban" HeaderText="Giá/SP" DataFormatString="{0:N0} VNĐ" ItemStyle-HorizontalAlign="Right" />
                        <asp:TemplateField HeaderText="Thành Tiền">
                            <ItemTemplate>
                                <%# string.Format("{0:N0} VNĐ", Convert.ToDecimal(Eval("Gia_Ban")) * Convert.ToDecimal(Eval("So_luong"))) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
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
            // Set active class cho button
            evt.currentTarget.className += " active";
        }
        
        // Khởi tạo tab đầu tiên khi load trang (đảm bảo tab 'pending' luôn hiển thị)
        document.addEventListener('DOMContentLoaded', function() {
            openTab({ currentTarget: document.querySelector('.tab-button.active') }, 'pending');
        });

        // Đóng modal
        function closeModal() {
            document.getElementById('<%= pnlDetailModal.ClientID %>').style.display = 'none';
        }
    </script>
</asp:Content>