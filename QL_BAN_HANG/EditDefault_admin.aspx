<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="EditDefault_admin.aspx.cs" Inherits="QL_BAN_HANG.EditDefault_admin" validateRequest="false" %>

<%@ Register Assembly="RichTextEditor" Namespace="RTE" TagPrefix="RTE" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    .preview-box {
        margin-top: 10px;
        padding: 10px;
        border: 1px dashed #aaa;
        width: 220px;
        text-align: center;
        background-color: #f9f9f9;
    }
    .preview-box img {
        max-width: 200px;
        height: auto;
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
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <br />
    <label>Thứ tự (OrderKey):</label>
    <asp:TextBox ID="txtOrderKey" runat="server" Text="" Width="100px" />   
    <br />
      <div class="preview-box">
          <label>Ảnh đại diện hiện tại / mới:</label><br />
          <asp:Image ID="imgPreview" runat="server" AlternateText="Chưa có ảnh" Width="100px" />
      </div>

      <label>Thay ảnh đại diện mới (nếu cần):</label>
      <asp:FileUpload ID="fileUploadHinhAnh" runat="server" onchange="previewImage(this)" />
    <br />
    <br />

    <label>Tiêu đề:</label>
    <asp:TextBox ID="txtTieuDe" runat="server" Width="100%" />

    <label>Tóm tắt:</label>
    <asp:TextBox ID="txtTomTat" runat="server" TextMode="MultiLine" Rows="3" Width="100%" />

    <label>Nội dung đầy đủ:</label>
    <RTE:Editor ID="EditorNoiDung" runat="server" Height="400px" Width="100%" />

    

    <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật bài viết" OnClick="btnUpdate_Click" CssClass="btn-update" />
    <asp:Button ID="btnExit" runat="server" Text="Quay lại" OnClick="btnExit_Click" />
</div>

</asp:Content>