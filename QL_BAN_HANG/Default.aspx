<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QL_BAN_HANG.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" Runat="Server">
    <style>
        /* ================================
           Vùng chứa toàn bộ nội dung
        ================================ */
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

        /* ================================
           Phần bài viết hot với nền trắng
        ================================ */
        .hot-article {
            background-color: #fff; /* Nền trắng */
            border-radius: 1rem;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            overflow: hidden;
            margin-bottom: 3rem;
        }
        .hot-image {
            width: 100%;
            height: 300px; /* Chiều cao cố định cho hình ảnh */
            object-fit: cover;
            display: block;
        }
        .hot-content {
            padding: 20px;
            text-align: left;
        }
        .hot-title {
            font-size: 2rem;
            font-weight: 800;
            color: #2c3e50;
            margin-bottom: 10px;
        }
        .hot-summary {
            font-size: 1.125rem;
            color: #7f8c8d;
            margin-bottom: 15px;
        }
        .hot-body {
            font-size: 1rem;
            line-height: 1.6;
            color: #333;
        }

        /* ================================
           Danh sách bài viết (Repeater)
        ================================ */
        .mb-16 {
            margin-bottom: 4rem;
        }
        .text-2xl {
            font-size: 1.5rem;
        }
        .font-semibold {
            font-weight: 600;
        }
        .text-gray-800 {
            color: #1f2937;
        }
        .mb-6 {
            margin-bottom: 1.5rem;
        }
        .border-b-4 {
            border-bottom-width: 4px;
        }
        .border-\$#4c673d\$ {
            border-color: #4c673d;
        }
        .pb-2 {
            padding-bottom: 0.5rem;
        }
        .grid {
            display: grid;
        }
        .grid-cols-1 {
            grid-template-columns: repeat(1, minmax(0, 1fr));
        }
        .md\:grid-cols-2 {
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }
        .lg\:grid-cols-3 {
            grid-template-columns: repeat(3, minmax(0, 1fr));
        }
        .gap-8 {
            gap: 2rem;
        }
        .bg-white {
            background-color: #fff;
        }
        .rounded-xl {
            border-radius: 0.75rem;
        }
        .shadow-md {
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
        }
        .overflow-hidden {
            overflow: hidden;
        }
        .hover\:shadow-xl:hover {
            box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
        }
        .transition {
            transition-property: color, background-color, border-color, text-decoration-color, fill, stroke, opacity, box-shadow, transform, filter, backdrop-filter;
            transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
            transition-duration: 150ms;
        }
        .duration-300 {
            transition-duration: 300ms;
        }
        .flex {
            display: flex;
        }
        .flex-col {
            flex-direction: column;
        }
        .h-full {
            height: 100%;
        }
        .h-48 {
            height: 12rem;
        }
        .relative {
            position: relative;
        }
        .transform {
            transform: translate(0, 0);
        }
        .hover\:scale-110:hover {
            transform: scale(1.1);
        }
        .duration-500 {
            transition-duration: 500ms;
        }
        .top-2 {
            top: 0.5rem;
        }
        .right-2 {
            right: 0.5rem;
        }
        .bg-red-500 {
            background-color: #ef4444;
        }
        .text-white {
            color: #fff;
        }
        .text-xs {
            font-size: 0.75rem;
        }
        .font-bold {
            font-weight: 700;
        }
        .px-2 {
            padding-left: 0.5rem;
            padding-right: 0.5rem;
        }
        .py-1 {
            padding-top: 0.25rem;
            padding-bottom: 0.25rem;
        }
        .rounded-full {
            border-radius: 9999px;
        }
        .p-5 {
            padding: 1.25rem;
        }
        .flex-grow {
            flex-grow: 1;
        }
        .text-lg {
            font-size: 1.125rem;
        }
        .mb-2 {
            margin-bottom: 0.5rem;
        }
        .hover\:text-\$#4c673d\$:hover {
            color: #4c673d;
        }
        .text-gray-600 {
            color: #4b5563;
        }
        .text-sm {
            font-size: 0.875rem;
        }
        .mb-4 {
            margin-bottom: 1rem;
        }
        .line-clamp-2 {
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            overflow: hidden;
        }
        .mt-auto {
            margin-top: auto;
        }
        .pt-4 {
            padding-top: 1rem;
        }
        .border-t {
            border-top-width: 1px;
        }
        .border-gray-100 {
            border-color: #f3f4f6;
        }
        .justify-between {
            justify-content: space-between;
        }
        .items-center {
            align-items: center;
        }
        .text-\$#4c673d\$ {
            color: #4c673d;
        }
        .font-semibold {
            font-weight: 600;
        }
        .hover\:underline:hover {
            text-decoration: underline;
        }
        .gap-1 {
            gap: 0.25rem;
        }
        .w-4 {
            width: 1rem;
        }
        .h-4 {
            height: 1rem;
        }
        .fill-none {
            fill: none;
        }
        .stroke {
            stroke: currentColor;
        }
        .stroke-current {
            stroke: currentColor;
        }
        .col-span-3 {
            grid-column: span 3 / span 3;
        }
        .py-10 {
            padding-top: 2.5rem;
            padding-bottom: 2.5rem;
        }
        .text-center {
            text-align: center;
        }
        .mt-8 {
            margin-top: 2rem;
        }
        .inline-block {
            display: inline-block;
        }
        /* Container phân trang */
        .pagination {
            display: flex;
            justify-content: center;
            gap: 8px;
            margin-top: 20px;
        }

        /* Link phân trang */
        .pagination a {
            display: inline-block;
            padding: 6px 12px;
            border: 1px solid #4c673d;
            color: #4c673d;
            border-radius: 9999px; /* bo tròn */
            font-size: 14px;
            font-weight: 500;
            text-decoration: none;
            transition: all 0.3s ease;
        }

        /* Hover */
        .pagination a:hover {
            background-color: #4c673d;
            color: #fff;
        }

        /* Trang hiện tại */
        .pagination span {
            display: inline-block;
            padding: 6px 12px;
            background-color: #4c673d;
            color: #fff;
            border-radius: 9999px;
            font-size: 14px;
            font-weight: 600;
        }
    </style>

    <div class="container">
        <!-- Bài viết hot với nền trắng -->
        <div class="hot-article">
            <asp:Image ID="Image1" runat="server" CssClass="hot-image" 
                       AlternateText="Bài viết hot"
                       onerror="this.src='https://placehold.co/600x400/e2e8f0/64748b?text=No+Image'" />
            <div class="hot-content">
                <h2 class="hot-title">
                    <asp:Label ID="Label1" runat="server" />
                </h2>
                <p class="hot-summary">
                    <asp:Label ID="Label2" runat="server" />
                </p>
                <div class="hot-body">
                    <asp:Label ID="Label3" runat="server" />
                </div>
            </div>
        </div>

        <!-- Danh sách bài viết -->
        <section class="mb-16">
            <h3 class="text-2xl font-semibold text-gray-800 mb-6 text-center relative">
                <span class="border-b-4 border-[#4c673d] pb-2">Tin Tức & Sự Kiện Mới</span>
            </h3>
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                <asp:Repeater ID="rptNews" runat="server">
                    <ItemTemplate>
                        <div class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-xl transition duration-300 flex flex-col h-full border border-gray-100">
                            <div class="h-48 overflow-hidden relative">
                                <img src='<%# ResolveUrl("~/uploads/images/" + Eval("Hinh_anh_page")) %>' 
                                     alt='<%# Eval("Tieu_de") %>'
                                     class="w-full h-full object-cover transform hover:scale-110 transition duration-500"
                                     onerror="this.src='https://placehold.co/600x400/e2e8f0/64748b?text=No+Image'">
                                <span class="absolute top-2 right-2 bg-red-500 text-white text-xs font-bold px-2 py-1 rounded-full">NEW</span>
                            </div>
                            <div class="p-5 flex flex-col flex-grow">
                                <h4 class="font-bold text-lg text-gray-800 mb-2 hover:text-[#4c673d] transition">
                                    <a href='Default.aspx?ID_BV=<%# Eval("ID_BV") %>'><%# Eval("Tieu_de") %></a>
                                </h4>
                                <p class="text-gray-600 text-sm mb-4 flex-grow line-clamp-2"><%# Eval("Tom_tac") %></p>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblEmpty" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="Chưa có bài viết nào." CssClass="text-center col-span-3 text-gray-500 py-10" />
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="text-center mt-8">
                <asp:Label ID="lblPaging" runat="server" CssClass="pagination"></asp:Label>
            </div>
        </section>
    </div>
</asp:Content>