<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductUser.aspx.cs" Inherits="QL_BAN_HANG.ProductUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/productuser.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="container">
        <h2 class="page-title">🛍️ Danh Sách Sản Phẩm</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

        <asp:GridView ID="GridViewProducts" runat="server" AutoGenerateColumns="False" CssClass="product-grid">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <div class="product-card">
                            <!-- Ảnh sản phẩm -->
                            <asp:Image ID="imgSanPham" runat="server" 
                                ImageUrl='<%# Eval("Hinh_anh", "~/uploads/images/{0}") %>' 
                                Visible='<%# !string.IsNullOrEmpty(Eval("Hinh_anh") as string) %>'
                                AlternateText='<%# Eval("Ten_san_pham") %>' CssClass="product-img" />

                            <!-- Thông tin sản phẩm -->
                            <div class="product-info">
                                <asp:Label ID="txt_sp" runat="server" Text='<%# Eval("Ten_san_pham") %>' CssClass="product-name"></asp:Label>
                                <asp:Label ID="txt_gcb" runat="server" Text='<%# String.Format("{0:N0} đ", Eval("Gia_co_ban")) %>' CssClass="product-price"></asp:Label>
                            </div>

                            <!-- Nút hành động -->
                            <div class="product-actions">
                                <asp:Button ID="btnAddCart" runat="server" Text="Thêm Giỏ Hàng" CssClass="btn-cart" CommandArgument='<%# Eval("ID_SP") %>' OnClick="btnAddCart_Click" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
