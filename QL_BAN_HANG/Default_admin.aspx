<%@ Page Title="Quản lý bài viết" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="Default_admin.aspx.cs" Inherits="QL_BAN_HANG.Default_admin" validateRequest="false" %>
<%@ Register Assembly="RichTextEditor" Namespace="RTE" TagPrefix="RTE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Container chính */
        .container {
            margin: 30px auto;
            width: 95%;
            max-width: 1200px;
            background-color: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }

        h2 {
            text-align: center;
            color: #2c3e50;
            margin-bottom: 20px;
        }

        h3 {
            color: #2980b9;
            margin-top: 25px;
            margin-bottom: 10px;
        }

        /* Vùng thêm bài viết */
        .control-area {
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 20px;
        }

        .control-area label {
            display: block;
            font-weight: bold;
            margin-top: 10px;
            color: #2c3e50;
        }

        .control-area input[type="text"],
        .control-area textarea {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 6px;
            margin-top: 5px;
        }

        .full-width {
            margin-top: 15px;
            text-align: center;
        }

        .action-button {
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-weight: bold;
            transition: background-color 0.3s ease;
        }

        .btn-add {
            background-color: #27ae60;
            color: #fff;
        }

        .btn-add:hover {
            background-color: #2ecc71;
        }

        /* Thông báo */
        #lblMessage {
            font-weight: bold;
            color: red;
        }

        /* GridView quản lý */
        .gridview-style {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .gridview-style th {
            background-color: #3498db;
            color: #fff;
            padding: 10px;
            text-align: center;
        }

        .gridview-style td {
            padding: 10px;
            text-align: center;
            border-bottom: 1px solid #eee;
        }

        .gridview-style tr:hover {
            background-color: #f9f9f9;
        }

        .gridview-style img {
            max-width: 100px;
            border-radius: 6px;
            box-shadow: 0 1px 4px rgba(0,0,0,0.2);
        }

        /* Link hành động */
        .link-edit {
            color: #2980b9;
            font-weight: bold;
            text-decoration: none;
        }
        .link-edit:hover {
            text-decoration: underline;
            color: #e67e22;
        }

        .link-delete {
            color: #e74c3c;
            font-weight: bold;
            text-decoration: none;
        }
        .link-delete:hover {
            text-decoration: underline;
            color: #c0392b;
        }

        .link-update {
            color: #27ae60;
            font-weight: bold;
            text-decoration: none;
        }
        .link-update:hover {
            text-decoration: underline;
            color: #2ecc71;
        }
        /* Phân trang GridView */
.gridview-style .pgr {
    text-align: center;
    margin-top: 15px;
}

.gridview-style .pgr table {
    margin: 0 auto;
}

.gridview-style .pgr td {
    padding: 6px 10px;
    font-weight: bold;
    color: #007bff;
    font-size: 14px;
}

.gridview-style .pgr a {
    display: inline-block;
    padding: 6px 12px;
    margin: 0 4px;
    background-color: #3498db;
    color: #fff;
    border-radius: 4px;
    text-decoration: none;
    border: 1px solid #2980b9;
    transition: background-color 0.3s ease;
}

.gridview-style .pgr a:hover {
    background-color: #e74c3c;
    border-color: #c0392b;
}

    </style>
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

        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

        <h3>📋 Danh Sách Bài Viết</h3>
        <asp:GridView ID="GridViewBaiViet" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="ID_BV"
            CssClass="gridview-style"
            AllowPaging="true" PageSize="5"
            OnPageIndexChanging="GridViewBaiViet_PageIndexChanging"
            OnRowDeleting="GridViewBaiViet_RowDeleting"
            OnRowCommand="GridViewBaiViet_RowCommand">

            <PagerStyle CssClass="pgr" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="ID_BV" HeaderText="ID" ReadOnly="True" ItemStyle-Width="50px" />

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
