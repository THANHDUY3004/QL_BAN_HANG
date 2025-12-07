<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductUser.aspx.cs" Inherits="QL_BAN_HANG.ProductUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/productuser.css" rel="stylesheet" />
    <style>
        /* Container chính */
        .container {
            margin: 30px auto;
            width: 100%;
            max-width: 1200px; /* giữ width tối đa 1200px */
            background-color: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            text-align: center;
        }

        .page-title {
            color: #007bff;
            text-align: center;
            margin-bottom: 30px;
            font-size: 24px;
            font-weight: bold;
        }

        .message {
            display: block;
            text-align: center;
            margin-bottom: 20px;
            font-size: 15px;
            color: #555;
        }

        /* Grid sản phẩm */
        .product-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr); /* luôn 3 cột */
            gap: 20px;
            margin-top: 30px;
        }

        /* Card sản phẩm */
        .product-card {
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 15px;
            text-align: center;
            background: #fff;
            transition: transform 0.2s ease, box-shadow 0.2s ease;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }

        .product-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.2);
        }

        /* Ảnh sản phẩm */
        .product-img {
            width: 150px;
            height: auto;
            margin-bottom: 10px;
            border-radius: 6px;
        }

        /* Tên sản phẩm */
        .product-name {
            font-weight: bold;
            font-size: 16px;
            display: block;
            margin-bottom: 5px;
            color: #2c3e50;
        }

        /* Giá sản phẩm */
        .product-price {
            color: #e74c3c;
            font-size: 15px;
            display: block;
            margin-bottom: 10px;
        }

        /* Nút hành động */
        .btn-cart {
            margin: 5px;
            padding: 8px 14px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: bold;
            background: #2ecc71;
            color: #fff;
            transition: background 0.2s ease;
        }

        .btn-cart:hover {
            background: #27ae60;
        }

        /* Responsive */
        @media (max-width: 992px) {
            .product-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 600px) {
            .product-grid {
                grid-template-columns: repeat(1, 1fr);
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="container">
        <h2 class="page-title">🛍️ Danh Sách Sản Phẩm</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

        <div class="product-grid">
            <asp:Repeater ID="RepeaterProducts" runat="server">
                <ItemTemplate>
                    <div class="product-card">
                        <asp:Image ID="imgSanPham" runat="server"
                            ImageUrl='<%# Eval("Hinh_anh", "~/uploads/images/{0}") %>'
                            AlternateText='<%# Eval("Ten_san_pham") %>'
                            CssClass="product-img" />
                        <div class="product-info">
                            <asp:Label ID="txt_sp" runat="server" Text='<%# Eval("Ten_san_pham") %>' CssClass="product-name" />
                            <asp:Label ID="txt_gcb" runat="server" Text='<%# String.Format("{0:N0} đ", Eval("Gia_co_ban")) %>' CssClass="product-price" />
                        </div>
                        <asp:Button ID="btnAddCart" runat="server" Text="🛒 Thêm Giỏ Hàng"
                            CssClass="btn-cart"
                            CommandArgument='<%# Eval("ID_SP") %>'
                            OnClick="btnAddCart_Click" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
