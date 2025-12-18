<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEdittor.aspx.cs" Inherits="QL_BAN_HANG.TestEdittor" ValidateRequest="false" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>CKEditor Miễn Phí Vĩnh Viễn</title>
    <script src="https://cdn.ckeditor.com/4.22.1/standard/ckeditor.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 90%; margin: 20px auto; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;">
            <h2 style="color: #2c3e50;">Trình soạn thảo mã nguồn mở</h2>
            
            <div style="margin-bottom: 10px;">
                <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="10" Width="220px"></asp:TextBox>
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="Cập nhật dữ liệu" OnClick="btnSubmit_Click" 
                Style="padding: 10px 20px; background-color: #007bff; color: white; border: none; border-radius: 4px; cursor: pointer;" />

            <hr style="margin: 20px 0; border: 0; border-top: 1px solid #eee;" />
            
            <h3 style="color: #7f8c8d;">Nội dung đã lưu:</h3>
            <div style="padding: 15px; border: 1px solid #ddd; border-radius: 4px; background-color: #fafafa; min-height: 100px;">
                <asp:Literal ID="litResult" runat="server"></asp:Literal>
            </div>
        </div>

        <script type="text/javascript">
            // Khởi tạo CKEditor
            // Bản này dùng giấy phép mã nguồn mở (Open Source), chạy vĩnh viễn
            CKEDITOR.replace('<%= txtContent.ClientID %>', {
                language: 'vi',
                height: 350,
                // Loại bỏ thông báo nhắc nhở về phiên bản (nếu có)
                versionCheck: false,
                // Giữ nguyên định dạng HTML khi dán từ Word
                forcePasteAsPlainText: false
            });
        </script>
    </form>
</body>
</html>