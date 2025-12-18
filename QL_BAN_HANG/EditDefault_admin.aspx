<%@ Page Title="Chỉnh sửa Bài Viết" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="EditDefault_admin.aspx.cs" Inherits="QL_BAN_HANG.EditDefault_admin" validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.tailwindcss.com"></script>
    <script src="https://cdn.ckeditor.com/4.22.1/standard/ckeditor.js"></script>
    <style>
        .edit-container { margin: 30px auto; width: 95%; max-width: 900px; background-color: #f7f9fc; padding: 30px; border-radius: 10px; box-shadow: 0 4px 12px rgba(0,0,0,0.1); }
        .edit-container h3 { color: #2980b9; margin-bottom: 25px; padding-bottom: 10px; border-bottom: 3px solid #3498db; font-size: 1.8rem; font-weight: 700; }
        .edit-container label { display: block; font-weight: bold; margin-top: 15px; margin-bottom: 5px; color: #2c3e50; }
        #imgPreview { max-width: 200px; height: auto; border-radius: 4px; border: 1px solid #ccc; padding: 2px; margin: 10px 0; display: block; }
        .asp-label-message { display: block; padding: 10px; background-color: #d4edda; color: #155724; border: 1px solid #c3e6cb; border-radius: 5px; font-weight: bold; margin-bottom: 10px; }
        
        /* CSS cho nút để đảm bảo luôn có màu */
        .btn-custom-update { background-color: #27ae60 !important; color: white !important; padding: 10px 25px !important; border-radius: 6px; border: none; font-weight: bold; cursor: pointer; }
        .btn-custom-exit { background-color: #95a5a6 !important; color: white !important; padding: 10px 25px !important; border-radius: 6px; border: none; font-weight: bold; cursor: pointer; }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="edit-container">
        <h3>📝 Chỉnh sửa Bài Viết</h3>
        <asp:Label ID="lblMessage" runat="server" CssClass="asp-label-message" Visible="false"></asp:Label>
        
        <label>Thứ tự (OrderKey):</label>
        <asp:TextBox ID="txtOrderKey" runat="server" type="number" min="1" 
            Style="width: 800px !important; padding: 10px; border: 1px solid #ccc; border-radius: 6px;" /> 
        
        <label>Ảnh đại diện:</label>
        <asp:Image ID="imgPreview" runat="server" AlternateText="No Image" />
        <asp:FileUpload ID="fileUploadHinhAnh" runat="server" onchange="previewImage(this)" />

        <label>Tiêu đề:</label>
        <asp:TextBox ID="txtTieuDe" runat="server" 
            Style="width: 800px !important; padding: 10px; border: 1px solid #ccc; border-radius: 6px;" />

        <label>Tóm tắt:</label>
        <asp:TextBox ID="txtTomTat" runat="server" TextMode="MultiLine" Rows="3" 
            Style="width: 800px !important; padding: 10px; border: 1px solid #ccc; border-radius: 6px;" />

        <label>Nội dung:</label>
        <asp:TextBox ID="NoiDung" runat="server" TextMode="MultiLine" />

        <div style="display: flex; gap: 20px; margin-top: 30px; align-items: center;">
            <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật bài viết" 
                OnClick="BtnUpdate_Click" CssClass="btn-custom-update" />
            
            <asp:Button ID="btnExit" runat="server" Text="Quay lại" 
                OnClick="BtnExit_Click" CssClass="btn-custom-exit" />
        </div>
    </div>
    
    <script type="text/javascript">
        function previewImage(input) { if (input.files && input.files[0]) { var reader = new FileReader(); reader.onload = function (e) { document.getElementById('<%= imgPreview.ClientID %>').src = e.target.result; }; reader.readAsDataURL(input.files[0]); } }
        
        CKEDITOR.replace('<%= NoiDung.ClientID %>', {
            language: 'vi',
            height: 400,
            width: '100%',
            versionCheck: false,
            allowedContent: true
        });
    </script>
</asp:Content>