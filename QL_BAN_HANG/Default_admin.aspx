<%@ Page Title="Quản lý bài viết" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="Default_admin.aspx.cs" Inherits="QL_BAN_HANG.Default_admin" validateRequest="false" %>

<%@ Register Assembly="RichTextEditor" Namespace="RTE" TagPrefix="RTE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/default_admin.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="container">
        <h2>📂 QUẢN LÝ BÀI VIẾT</h2>

        <h3>➕ Thêm Bài Viết Mới</h3>
        <div class="control-area" style="background-color: #e9f7e9;">

            <label>Thứ tự (OrderKey):</label>
            <asp:TextBox ID="txtAddOrderKey" runat="server" Text="1" Width="100px" />

            <label>Tiêu đề:</label>
            <asp:TextBox ID="txtAddTieuDe" runat="server" Width="100%" />

            <label>Tóm tắt:</label>
            <asp:TextBox ID="txtAddTomTat" runat="server" TextMode="MultiLine" Rows="3" Width="100%" />

            <label>Nội dung đầy đủ:</label>
            <RTE:Editor ID="EditorAddNoiDung" runat="server" Height="300px" Width="100%" />

            <label>Hình ảnh đại diện:</label>
            <asp:FileUpload ID="fileUploadHinhAnh" runat="server" />

            <div class="full-width">
                <asp:Button ID="butAdd" runat="server" Text="Thêm Bài Viết" OnClick="butAdd_Click" CssClass="action-button btn-add" />
            </div>
        </div>

        <b style="color: red; margin-top: 15px; display: inline-block;">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </b>

        <h3>📋 Danh Sách Bài Viết</h3>
        <asp:GridView ID="GridViewBaiViet" runat="server"
    AutoGenerateColumns="False"
    DataKeyNames="ID_BV"
    CssClass="gridview-style"
    OnRowDeleting="GridViewBaiViet_RowDeleting"
    OnRowCommand="GridViewBaiViet_RowCommand">

    <Columns>
        <asp:BoundField DataField="ID_BV" HeaderText="ID" ReadOnly="True" ItemStyle-Width="50px" >

<ItemStyle Width="50px"></ItemStyle>
        </asp:BoundField>

        <asp:TemplateField HeaderText="Hình ảnh">
            <ItemTemplate>
                <asp:Image ID="imgBaiViet" runat="server"
                    ImageUrl='<%# "~/uploads/images/" + Eval("Hinh_anh_page") %>'
                    Visible='<%# !string.IsNullOrEmpty(Eval("Hinh_anh_page") as string) %>'
                    Width="100px" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="Tieu_de" HeaderText="Tiêu đề" />

        <asp:BoundField DataField="OrderKey" HeaderText="Thứ tự" />
        <asp:TemplateField HeaderText="Sửa OrderKey">
            <ItemTemplate>
                <asp:TextBox ID="txtOrderKey" runat="server" 
                    Text='<%# Eval("OrderKey") %>' Width="60px" />
                <asp:LinkButton ID="btnUpdateOrderKey" runat="server" 
                    CommandName="UpdateOrderKey" 
                    CommandArgument='<%# Eval("ID_BV") %>' 
                    Text="Cập nhật" CssClass="link-update" />
            </ItemTemplate>
            <HeaderStyle Width="60px" />
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Chỉnh sửa">
            <ItemTemplate>
                <asp:HyperLink ID="lnkEdit" runat="server" 
                    NavigateUrl='<%# "EditDefault_admin.aspx?ID_BV=" + Eval("ID_BV") %>' 
                    Text="Sửa" CssClass="link-edit" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Xóa">
            <ItemTemplate>
                <asp:LinkButton ID="btnDelete" runat="server" 
                    CommandName="Delete" 
                    Text="Xóa" 
                    CssClass="link-delete"
                    OnClientClick="return confirm('Bạn có chắc chắn muốn xóa bài viết này?');">
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

    </div>
</asp:Content>