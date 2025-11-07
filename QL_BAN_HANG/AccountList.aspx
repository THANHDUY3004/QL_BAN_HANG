<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_admin.Master" AutoEventWireup="true" CodeBehind="AccountList.aspx.cs" Inherits="QL_BAN_HANG.AccountList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="layout/accountlist.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="container">
    <h2 style="text-align: center;"> QUẢN LÝ TÀI KHOẢN</h2>
    
    <div style="text-align: center; margin: 20px 0; border-bottom: 1px solid #eee; padding-bottom: 20px;">
        

        <asp:Button ID="btnLoginPage" 
                    runat="server" 
                    Text="Đăng Xuất Tài Khoản" 
                    OnClick="btnLoginPage_Click" 
                    CssClass="admin-button btn-login" />
    </div>
    <br />
        <br />
        <table style="width:100%;">
            <tr>
                <td>
                    <h3 style="text-align: left;">👤 Account List</h3>
                </td>
                <td>
    <asp:DropDownList ID="ddlPhanQuyen" runat="server" width="246px" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" style="margin-top: 19px" Height="52px">
    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Họ Và Tên :</td>
                <td><asp:TextBox ID="txtht" runat="server" Width="200px"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td>Số Điện Thoại : </td>
                <td> <asp:TextBox ID="txtsdt" runat="server" Width="200px"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td>Địa Chỉ :</td>
                <td><asp:TextBox ID="txtdchi" runat="server" Width="200px"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td>Mật Khẩu:</td>
                <td> <asp:TextBox ID="txtmk" runat="server" Width="200px"></asp:TextBox> 

                </td>
            </tr>
            <tr>
                <td>

    <b><asp:Label ID="lblMessage" runat="server" Text="" ></asp:Label>
    </b>
                </td>
                <td> <asp:Button ID="butAdd" runat="server" Text="Thêm tài khoản" OnClick="butAdd_Click" Width="100px" />

                </td>
            </tr>
        </table>
    <h3 style="text-align: left;">
        <asp:GridView ID="GridViewAccounts" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="So_dien_thoai" OnRowDeleting="GridView1_RowDeleting" Width="840px" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridViewAccounts_SelectedIndexChanged">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Ho_va_ten" HeaderText="Họ và Tên">
                <FooterStyle Width="80px" />
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="So_dien_thoai" HeaderText="Số Điện Thoại">
                <FooterStyle Width="80px" />
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Dia_Chi" HeaderText="Địa Chỉ">
                <FooterStyle Width="80px" />
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Mat_khau" HeaderText="Mật Khẩu">
                <FooterStyle Width="80px" />
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:HyperLinkField DataNavigateUrlFields="So_dien_thoai" Text="Sửa" />
                <asp:CommandField DeleteText="Xóa" ShowDeleteButton="True" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Button ID="butDelete" runat="server" OnClick="butDelete_Click" Text="Xóa" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ckhDelete" runat="server"  />
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Center" Width="80px" />
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </h3>
</div>
</asp:Content>
