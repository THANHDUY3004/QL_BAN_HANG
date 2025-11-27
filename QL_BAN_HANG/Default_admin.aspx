<%@ Page Title="Quản lý bài viết" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="Default_admin.aspx.cs" Inherits="QL_BAN_HANG.Default_admin" validateRequest="false" %>
<%@ Register Assembly="RichTextEditor" Namespace="RTE" TagPrefix="RTE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* CSS Chung cho Body */
        body {
            font-family: 'Arial', sans-serif;
            background-color: #f4f7f6; /* Nền nhẹ nhàng */
            color: #333;
        }

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
            color: #2c3e50; /* Màu tiêu đề chính */
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 2px solid #eee;
        }

        h3 {
            color: #2980b9; /* Màu tiêu đề phụ */
            margin-top: 25px;
            margin-bottom: 10px;
            border-left: 5px solid #2980b9;
            padding-left: 10px;
        }

        /* Vùng Thêm/Lọc - Sử dụng flexbox cho các cặp label/input và block cho RTE/FileUpload */
        .control-area {
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 20px;
            background-color: #e9f7e9; /* Nền nhẹ cho vùng thêm */
            border: 1px solid #d4edda;
        }

            /* Nhóm label và input */
            .control-group {
                display: flex;
                align-items: center;
                margin-bottom: 15px;
            }

            .control-group label {
                flex-basis: 200px; /* Cố định chiều rộng nhãn */
                font-weight: bold;
                color: #2c3e50;
                text-align: right;
                padding-right: 15px; /* Khoảng cách giữa nhãn và input */
            }
        
            .control-group input[type="text"], 
            .control-group select, 
            .control-group textarea {
                flex-grow: 1; /* Cho input mở rộng hết phần còn lại */
                padding: 8px;
                border: 1px solid #ccc;
                border-radius: 6px;
                box-sizing: border-box;
            }

            .control-group textarea {
                min-height: 80px;
                resize: vertical;
            }

            /* Đặc biệt cho RTE Editor và FileUpload để chúng chiếm toàn bộ chiều rộng */
            .control-group.full-width-control {
                display: block; /* Chuyển về block để RTE/FileUpload chiếm dòng riêng */
            }

            .control-group.full-width-control label {
                text-align: left; /* Nhãn của RTE/FileUpload căn trái */
                flex-basis: auto; /* Bỏ cố định chiều rộng nhãn */
                margin-bottom: 5px;
                padding-right: 0;
            }
            /* Đảm bảo RTE và FileUpload chiếm toàn bộ chiều rộng có sẵn */
            .control-group.full-width-control .RTE_Editor,
            .control-group.full-width-control input[type="file"] {
                width: 100%;
            }


            .action-button-container {
                text-align: right; /* Đẩy nút Add về bên phải */
                margin-top: 20px;
            }

        /* Nút bấm */
        .action-button {
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-weight: bold;
            color: white;
            transition: background-color 0.3s ease, transform 0.1s;
        }

            .action-button:hover {
                transform: translateY(-1px);
            }

        .btn-add {
            background-color: #27ae60;
        }

            .btn-add:hover {
                background-color: #1e8449;
            }

        /* Thông báo */
        #lblMessage {
            display: block;
            margin: 15px 0;
            font-weight: bold;
            color: #c0392b; /* Màu đỏ nổi bật */
            text-align: center;
        }

        /* GridView Styling */
        .gridview-style {
            width: 100%;
            border-collapse: collapse;
            font-family: "Segoe UI", Arial, sans-serif;
            font-size: 14px;
            margin: 20px 0;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }

            /* Header */
            .gridview-style th {
                background-color: #3498db; /* Màu xanh dương hiện đại */
                color: #fff;
                font-weight: bold;
                text-align: center;
                padding: 10px 12px;
                border: 1px solid #2980b9;
                line-height: 1.4;
            }

            /* Nội dung ô */
            .gridview-style td {
                padding: 8px 12px;
                border: 1px solid #ddd;
                color: #333;
                vertical-align: middle;
                line-height: 1.5;
                word-break: break-word;
                text-align: center; /* Căn giữa nội dung ô */
            }

            /* Dòng xen kẽ */
            .gridview-style tr:nth-child(even) {
                background-color: #f9f9f9;
            }

            /* Hover */
            .gridview-style tr:hover {
                background-color: #e6f0ff;
                cursor: default;
            }

            /* Hình ảnh trong GridView */
            .gridview-style img {
                max-width: 100px;
                max-height: 80px;
                border-radius: 6px;
                box-shadow: 0 1px 4px rgba(0,0,0,0.2);
            }

            /* Ô nhập OrderKey */
            .gridview-style input[type="text"] {
                padding: 4px 6px;
                border: 1px solid #ccc;
                border-radius: 4px;
                text-align: center;
            }

            /* Nút bấm trong GridView (Cập nhật OrderKey) */
            .gridview-style input[type="submit"],
            .gridview-style button {
                background-color: #27ae60;
                color: #fff;
                border: none;
                padding: 6px 10px;
                border-radius: 4px;
                transition: background-color 0.3s ease;
                font-size: 13px;
                cursor: pointer;
            }

                .gridview-style input[type="submit"]:hover,
                .gridview-style button:hover {
                    background-color: #1e8449;
                }

        /* Link hành động */
        .link-edit, .link-update, .link-delete {
            font-weight: bold;
            text-decoration: none;
            padding: 4px 8px;
            border-radius: 4px;
            transition: background-color 0.3s, color 0.3s;
        }

        .link-edit {
            color: #2980b9;
        }
        .link-edit:hover {
            text-decoration: underline;
            color: #1f618d;
        }

        .link-update {
            color: #27ae60;
        }
        .link-update:hover {
            text-decoration: underline;
            color: #1e8449;
        }

        .link-delete {
            color: #e74c3c;
        }
        .link-delete:hover {
            text-decoration: underline;
            color: #c0392b;
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
            font-size: 14px;
            border: none;
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
            background-color: #2980b9;
            border-color: #1f618d;
        }
        
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="container">
        <h2>📂 QUẢN LÝ BÀI VIẾT</h2>

        <h3>➕ Thêm Bài Viết Mới</h3>
        <div class="control-area">
            <div class="control-group">
                <label>Thứ tự (OrderKey):</label>
                <asp:TextBox ID="txtAddOrderKey" runat="server" Text="1" Width="100px" />
            </div>

            <div class="control-group">
                <label>Tiêu đề:</label>
                <asp:TextBox ID="txtAddTieuDe" runat="server" />
            </div>

            <div class="control-group">
                <label>Tóm tắt:</label>
                <asp:TextBox ID="txtAddTomTat" runat="server" TextMode="MultiLine" Rows="3" />
            </div>

            <%-- RTE và FileUpload sẽ dùng control-group riêng để chiếm toàn bộ chiều rộng --%>
            <div class="control-group full-width-control">
                <label>Nội dung đầy đủ:</label>
                <RTE:Editor ID="EditorAddNoiDung" runat="server" Height="300px" Width="100%" />
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

                <asp:BoundField DataField="Tieu_de" HeaderText="Tiêu đề" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
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