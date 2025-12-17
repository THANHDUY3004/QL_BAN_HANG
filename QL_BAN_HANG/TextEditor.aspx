<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TextEditor.aspx.cs" Inherits="QL_BAN_HANG.TextEditor" validateRequest="false"%>
<%@ Register Assembly="RichTextEditor" Namespace="RTE" TagPrefix="RTE" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Editor</title>
    <style>
        .container {
            width: 80%;
            margin: 20px auto;
            font-family: Arial, sans-serif;
        }
        h2 {
            color: #507CD1;
        }
        .btn {
            margin: 10px 5px 20px 0;
            padding: 6px 12px;
            background: #507CD1;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn:hover {
            background: #365fa3;
        }
        .result {
            margin-top: 20px;
            padding: 15px;
            border: 1px solid #ddd;
            background: #f9f9f9;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <div class="container">
            <h2>🖋️ Rich Text Editor Demo</h2>

            <!-- Editor -->
            <RTE:Editor ID="Editor1" runat="server" Height="400px" Width="100%" />

            <!-- Nút thao tác -->
            <asp:Button ID="btnSave" runat="server" Text="Lưu & Hiển thị" CssClass="btn"
                        OnClick="btnSave_Click" />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <!-- Hiển thị kết quả -->
            <div class="result">
                <asp:Literal ID="litResult" runat="server"></asp:Literal>
            </div>
        </div>
    </form>
</body>
</html>
