<%@ Page Title="Quản lý bài viết" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="Default_admin.aspx.cs" Inherits="QL_BAN_HANG.Default_admin" validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.ckeditor.com/4.22.1/standard/ckeditor.js"></script>
    <style>
        body { font-family: 'Arial', sans-serif; background-color: #f4f7f6; color: #333; }
        .container { margin: 30px auto; width: 95%; max-width: 1200px; background-color: #fff; padding: 20px; border-radius: 10px; box-shadow: 0 4px 12px rgba(0,0,0,0.1); }
        h2 { text-align: center; color: #2c3e50; margin-bottom: 20px; padding-bottom: 10px; border-bottom: 2px solid #eee; }
        h3 { color: #2980b9; margin-top: 25px; margin-bottom: 10px; border-left: 5px solid #2980b9; padding-left: 10px; }
        .control-area { padding: 15px; border-radius: 8px; margin-bottom: 20px; background-color: #e9f7e9; border: 1px solid #d4edda; }
        .control-group { display: flex; align-items: center; margin-bottom: 15px; }
        .control-group label { flex-basis: 200px; font-weight: bold; color: #2c3e50; text-align: right; padding-right: 15px; }
        .full-width-control { display: block; }
        .full-width-control label { text-align: left; flex-basis: auto; margin-bottom: 5px; padding-right: 0; }
        #txtAddOrderKey { width: 100px; padding: 8px; border: 1px solid #ccc; border-radius: 6px; }
        #txtAddTieuDe { flex-grow: 1; padding: 8px; border: 1px solid #ccc; border-radius: 6px; }
        #txtAddTomTat { flex-grow: 1; padding: 8px; border: 1px solid #ccc; border-radius: 6px; min-height: 80px; resize: vertical; }
        #AddNoiDung { width: 100%; padding: 10px; border: 1px solid #ccc; border-radius: 6px; background-color: #f9f9f9; min-height: 100px; text-mode: MultiLine; }
        .action-button-container { text-align: right; margin-top: 20px; }
        .action-button { padding: 10px 20px; border: none; border-radius: 6px; cursor: pointer; font-weight: bold; color: white; transition: background-color 0.3s ease, transform 0.1s; }
        .btn-add { background-color: #27ae60; }
        #lblMessage { display: block; margin: 15px 0; font-weight: bold; color: #c0392b; text-align: center; }
        .gridview-style { width: 100%; border-collapse: collapse; font-family: "Segoe UI", Arial, sans-serif; font-size: 14px; margin: 20px 0; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
        .gridview-style th { background-color: #3498db; color: #fff; font-weight: bold; text-align: center; padding: 10px 12px; border: 1px solid #2980b9; line-height: 1.4; }
        .gridview-style td { padding: 8px 12px; border: 1px solid #ddd; color: #333; vertical-align: middle; line-height: 1.5; word-break: break-word; text-align: center; }
        .gridview-style tr:nth-child(even) { background-color: #f9f9f9; }
        .gridview-style tr:hover { background-color: #e6f0ff; }
        .gridview-style img { max-width: 100px; max-height: 80px; border-radius: 6px; box-shadow: 0 1px 4px rgba(0,0,0,0.2); }
        .link-edit { color: #2980b9; font-weight: bold; text-decoration: none; }
        .link-update { color: #27ae60; font-weight: bold; text-decoration: none; }
        .link-delete { color: #e74c3c; font-weight: bold; text-decoration: none; }
        .gridview-style .pgr { text-align: center; margin-top: 15px; }
        .gridview-style .pgr a { display: inline-block; padding: 6px 12px; margin: 0 4px; background-color: #3498db; color: #fff; border-radius: 4px; text-decoration: none; border: 1px solid #2980b9; }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="container">
        <h2>📂 QUẢN LÝ BÀI VIẾT</h2>

        <h3>➕ Thêm Bài Viết Mới</h3>
        <div class="control-area">
            <div class="control-group">
                <label>Thứ tự (OrderKey):</label>
                <asp:TextBox ID="txtAddOrderKey" runat="server" Height="30px" Text="1" type="number" min="1" step="1" />
            </div>

            <div class="control-group">
                <label>Tiêu đề:</label>
                <asp:TextBox ID="txtAddTieuDe" runat="server" Height="30px" Width="800px" />
            </div>

            <div class="control-group">
                <label>Tóm tắt:</label>
                <asp:TextBox ID="txtAddTomTat" runat="server" TextMode="MultiLine" Rows="3" Height="30px" Width="800px" />
            </div>

            <div class="control-group full-width-control">
                <label>Nội dung đầy đủ:</label>
                <asp:TextBox ID="AddNoiDung" runat="server" Height="200px" TextMode="MultiLine"></asp:TextBox>
            </div>

            <div class="control-group full-width-control">
                <label>Hình ảnh đại diện:</label>
                <asp:FileUpload ID="fileUploadHinhAnh" runat="server" />
            </div>

            <div class="action-button-container">
                <asp:Button ID="butAdd" runat="server" Text="Thêm Bài Viết" OnClick="ButAdd_Click" CssClass="action-button btn-add" />
            </div>
        </div>

        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

        <h3>📋 Danh Sách Bài Viết</h3>
        <asp:GridView ID="GridViewBaiViet" runat="server" AutoGenerateColumns="False" DataKeyNames="ID_BV" CssClass="gridview-style" AllowPaging="true" PageSize="5" OnPageIndexChanging="GridViewBaiViet_PageIndexChanging" OnRowDeleting="GridViewBaiViet_RowDeleting" OnRowCommand="GridViewBaiViet_RowCommand">
            <PagerStyle CssClass="pgr" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="ID_BV" HeaderText="ID" ReadOnly="True" ItemStyle-Width="50px" />
                <asp:TemplateField HeaderText="Hình ảnh">
                    <ItemTemplate>
                        <asp:Image ID="imgBaiViet" runat="server" ImageUrl='<%# "~/uploads/images/" + Eval("Hinh_anh_page") %>' Visible='<%# !string.IsNullOrEmpty(Eval("Hinh_anh_page") as string) %>' Width="100px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Tieu_de" HeaderText="Tiêu đề" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                <asp:BoundField DataField="OrderKey" HeaderText="Thứ tự" />
                <asp:TemplateField HeaderText="Sửa OrderKey">
                    <ItemTemplate>
                        <asp:TextBox ID="txtOrderKey" runat="server" Text='<%# Eval("OrderKey") %>' Width="60px" Style="text-align:center; border:1px solid #ccc; border-radius:4px;" />
                        <asp:LinkButton ID="btnUpdateOrderKey" runat="server" CommandName="UpdateOrderKey" CommandArgument='<%# Eval("ID_BV") %>' Text="Cập nhật" CssClass="link-update" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Chỉnh sửa">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# "EditDefault_admin.aspx?ID_BV=" + Eval("ID_BV") %>' Text="Sửa" CssClass="link-edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Xóa">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Xóa" CssClass="link-delete" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa bài viết này?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <script type="text/javascript">
        CKEDITOR.replace('<%= AddNoiDung.ClientID %>', {
            language: 'vi',
            height: 350,
            versionCheck: false,
            allowedContent: true,
            forcePasteAsPlainText: false
        });
    </script>
</asp:Content>