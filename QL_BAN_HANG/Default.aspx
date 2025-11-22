<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QL_BAN_HANG.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" Runat="Server">
    <style>
        /* Vùng chứa toàn bộ nội dung */
        .container {
            margin: 30px auto;
            width: 95%;
            max-width: 1400px;
            background-color: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            text-align: center;
        }

        /* Ảnh bài viết hot */
        .hot-image {
            display: block;
            margin: 0 auto 15px auto;
            max-width: 500px;
            height: auto;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.2);
        }

        /* Tiêu đề bài viết hot */
        .title {
            font-size: 28px;
            font-weight: bold;
            color: #2c3e50;
            margin: 10px 0;
        }

        /* Tóm tắt bài viết hot */
        .summary {
            font-size: 16px;
            color: #7f8c8d;
            margin-bottom: 15px;
        }

        /* Nội dung bài viết hot */
        .content {
            font-size: 18px;
            line-height: 1.6;
            color: #333;
            margin-bottom: 30px;
            text-align: center;
        }

        /* Vùng chứa GridView */
        .grid-container {
            display: flex;
            justify-content: center;
            margin-top: 30px;
        }

        /* GridView hiện đại, không viền */
        #GridView1 {
            width: 90%;
            max-width: 1000px;
            border-collapse: collapse;
            border: none;
            box-shadow: none;
        }

        /* Header GridView */
        #GridView1 th {
            background-color: transparent;
            color: #2c3e50;
            font-size: 16px;
            padding: 10px;
            border: none;
            text-align: center;
        }

        /* Ô dữ liệu GridView */
        #GridView1 td {
            padding: 10px;
            vertical-align: top;
            border: none;
            text-align: center;
        }

        /* Hover dòng GridView */
        #GridView1 tr:hover {
            background-color: #f9f9f9;
        }

        /* Ảnh trong GridView */
        #GridView1 img {
            max-width: 120px;
            border-radius: 6px;
            box-shadow: 0 1px 4px rgba(0,0,0,0.2);
        }

        /* Link tiêu đề trong GridView */
        #GridView1 a {
            font-size: 18px;
            font-weight: bold;
            color: #2980b9;
            text-decoration: none;
        }

        #GridView1 a:hover {
            text-decoration: underline;
            color: #e74c3c;
        }

        /* Tóm tắt trong GridView */
        #GridView1 .summary {
            font-size: 14px;
            color: #555;
            display: block;
            margin-top: 5px;
        }

    </style>

    <div class="container">
        <asp:Label ID="Tieu_de_hot" runat="server" CssClass="title"></asp:Label><br />
        <asp:Image ID="Image_hot" runat="server" CssClass="hot-image" /><br />
        <asp:Label ID="Tom_tac_hot" runat="server" CssClass="summary"></asp:Label><hr />
        <div class="content">
            <asp:Literal ID="Noi_dung_hot" runat="server"></asp:Literal>
        </div>


        <div class="grid-container">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeader="False">
                <Columns>
                    <asp:TemplateField HeaderText="Hình ảnh">
                        <ItemTemplate>
                            <asp:HyperLink ID="imgLink" runat="server" NavigateUrl='<%# "~/Default.aspx?ID_BV=" + Eval("ID_BV") %>'>
                                <asp:Image ID="img" runat="server" ImageUrl='<%# "~/uploads/images/" + Eval("Hinh_anh_page") %>' Width="100px" />
                            </asp:HyperLink>
                        </ItemTemplate>
                        <HeaderStyle Width="100px" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Tiêu đề">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkTitle" runat="server" 
                                NavigateUrl='<%# "~/Default.aspx?ID_BV=" + Eval("ID_BV") %>' 
                                Text='<%# Eval("Tieu_de") %>'></asp:HyperLink>
                            <br />
                            <asp:Label ID="lblSummary" runat="server" Text='<%# Eval("Tom_tac") %>' CssClass="summary"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    
</asp:Content>
