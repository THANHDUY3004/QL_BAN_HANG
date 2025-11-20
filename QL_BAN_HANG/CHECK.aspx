<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CHECK.aspx.cs" Inherits="QL_BAN_HANG.CHECK" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Danh sách sản phẩm</title>
    <style>
        .row { display: flex; flex-wrap: wrap; margin-bottom: 20px; }
        .product-card { flex: 1 0 30%; margin: 10px; border: 1px solid #ccc; padding: 10px; }
        .product-img { width: 100%; height: auto; }
        .product-info { margin-top: 10px; }
        .product-name { font-weight: bold; display: block; }
        .product-price { color: green; display: block; }
        .btn-cart { margin-top: 10px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="lblMessage" runat="server" CssClass="message" />
        <asp:Repeater ID="RepeaterProducts" runat="server">
            <ItemTemplate>
                <%# (Container.ItemIndex % 3 == 0) ? "<div class='row'>" : "" %>
                <div class="product-card">
                    <asp:Image ID="imgSanPham" runat="server"
                        ImageUrl='<%# Eval("Hinh_anh", "~/uploads/images/{0}") %>'
                        AlternateText='<%# Eval("Ten_san_pham") %>'
                        CssClass="product-img" />
                    <div class="product-info">
                        <asp:Label ID="txt_sp" runat="server" Text='<%# Eval("Ten_san_pham") %>' CssClass="product-name" />
                        <asp:Label ID="txt_gcb" runat="server" Text='<%# String.Format("{0:N0} đ", Eval("Gia_co_ban")) %>' CssClass="product-price" />
                    </div>
                    <asp:Button ID="btnAddCart" runat="server" Text="Thêm Giỏ Hàng"
                        CssClass="btn-cart"
                        CommandArgument='<%# Eval("ID_SP") %>'
                        OnClick="btnAddCart_Click" />
                </div>
                <%# (Container.ItemIndex % 3 == 2 || Container.ItemIndex == ((RepeaterProducts.Items.Count - 1))) ? "</div>" : "" %>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
