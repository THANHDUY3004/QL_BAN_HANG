<%@ Page Title="Chỉnh sửa Bài Viết" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="EditDefault_admin.aspx.cs" Inherits="QL_BAN_HANG.EditDefault_admin" validateRequest="false" %>

<%@ Register Assembly="RichTextEditor" Namespace="RTE" TagPrefix="RTE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chỉnh sửa Bài Viết - Admin</title>
    <script src="https://cdn.tailwindcss.com"></script>

    <style>
        /* Tái tạo .container của Default_admin.aspx (chú trọng vào màu nền và box-shadow) */
        .edit-container {
            margin: 30px auto;
            width: 95%;
            max-width: 900px; /* Nhỏ hơn trang danh sách một chút */
            background-color: #fff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            /* Thêm màu nền cho vùng edit để phân biệt với master page */
            background-color: #f7f9fc; 
        }

        /* Tái tạo style h3 (tiêu đề trang) */
        .edit-container h3 {
            color: #2980b9;
            margin-bottom: 25px;
            padding-bottom: 10px;
            border-bottom: 3px solid #3498db;
            font-size: 1.8rem; /* text-3xl */
            font-weight: 700; /* font-bold */
        }
        
        /* Tái tạo style Label */
        .edit-container label {
            display: block;
            font-weight: bold;
            margin-top: 15px;
            margin-bottom: 5px;
            color: #2c3e50;
        }

        /* Tái tạo style TextBox */
        .edit-container input[type="text"],
        .edit-container textarea {
             padding: 10px;
             border: 1px solid #ccc;
             border-radius: 6px;
             margin-top: 5px;
             margin-bottom: 10px;
             box-sizing: border-box; /* Quan trọng để Width=100% không bị tràn */
             transition: border-color 0.3s;
        }

        .edit-container input[type="text"]:focus,
        .edit-container textarea:focus {
            border-color: #3498db;
            outline: none;
        }

        /* Style cho Message (lblMessage) */
        .edit-container .asp-label-message {
            display: block;
            margin-top: 10px;
            padding: 10px;
            background-color: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
            border-radius: 5px;
            font-weight: bold;
        }
        
        /* Style cho Preview Box */
        .preview-box {
            margin-top: 15px;
            padding: 15px;
            border: 1px solid #bdc3c7; /* Đổi màu viền cho rõ ràng */
            border-radius: 8px;
            width: 240px;
            text-align: center;
            background-color: #ecf0f1;
            box-shadow: 0 1px 3px rgba(0,0,0,0.05);
        }
        .preview-box img {
            max-width: 200px;
            height: auto;
            border-radius: 4px;
            border: 1px solid #ccc;
            padding: 2px;
            margin-top: 5px;
        }

        /* Tái tạo style cho nút Update/Cancel */
        .btn-update {
            background-color: #27ae60; /* Màu xanh lá (Add/Update) */
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            font-weight: bold;
            cursor: pointer;
            margin-right: 10px;
            margin-top: 30px;
            transition: background-color 0.3s;
        }
        .btn-update:hover {
             background-color: #2ecc71;
        }

        .edit-container input[type="submit"][id$="btnExit"] {
            background-color: #95a5a6; /* Màu xám (Cancel/Exit) */
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            font-weight: bold;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        .edit-container input[type="submit"][id$="btnExit"]:hover {
             background-color: #7f8c8d;
        }
    </style>
    
    <script type="text/javascript">
        function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = document.getElementById('<%= imgPreview.ClientID %>');
                    img.src = e.target.result;
                    img.style.display = 'block';
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="edit-container">
        <h3>📝 Chỉnh sửa Bài Viết</h3>
        
        <asp:Label ID="lblMessage" runat="server" Text="" CssClass="asp-label-message mb-5" Visible="false"></asp:Label>
        
        <label>Thứ tự (OrderKey):</label>
        <asp:TextBox ID="txtOrderKey" runat="server" Text="" CssClass="w-32 inline-block" /> 
        
        <div class="preview-box">
            <label>Ảnh đại diện hiện tại / mới:</label><br />
            <asp:Image ID="imgPreview" runat="server" AlternateText="Chưa có ảnh" CssClass="block mx-auto" />
        </div>

        <label>Thay ảnh đại diện mới (nếu cần):</label>
        <asp:FileUpload ID="fileUploadHinhAnh" runat="server" onchange="previewImage(this)" CssClass="mt-2" />
        <br />
        <br />

        <label>Tiêu đề:</label>
        <asp:TextBox ID="txtTieuDe" runat="server" Width="100%" CssClass="w-full" />

        <label>Tóm tắt:</label>
        <asp:TextBox ID="txtTomTat" runat="server" TextMode="MultiLine" Rows="3" Width="100%" CssClass="w-full" />

        <label>Nội dung đầy đủ:</label>
        <RTE:Editor ID="EditorNoiDung" runat="server" Height="400px" Width="100%" />

        <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật bài viết" OnClick="BtnUpdate_Click" CssClass="btn-update" />
        <asp:Button ID="btnExit" runat="server" Text="Quay lại" OnClick="BtnExit_Click" />
    </div>
</asp:Content>