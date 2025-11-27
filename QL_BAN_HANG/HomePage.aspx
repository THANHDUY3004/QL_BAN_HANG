<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="QL_BAN_HANG.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chào Mừng Đến Cửa Hàng Trà Sữa Đồng Tháp</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Tải Tailwind CSS -->
    <script src="https://cdn.tailwindcss.com"></script>
    <!-- Tải Swiper.js (Thư viện Slider) -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/swiper@11/swiper-bundle.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/swiper@11/swiper-bundle.min.js"></script>

    <style>
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@100..900&display=swap');
        body {
            font-family: 'Inter', sans-serif;
            background-color: #f7f3f3;
        }
        .swiper-pagination-bullet-active {
            background-color: #4c673d !important;
        }
        .slider-image {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            object-fit: cover;
            opacity: 0;
            transition: opacity 1.5s ease-in-out;
            z-index: 0;
        }
        .slider-image.active {
            opacity: 1;
            z-index: 1;
        }
        .slider-overlay {
            position: absolute;
            inset: 0;
            background-color: rgba(0, 0, 0, 0.4);
            z-index: 2;
            display: flex;
            align-items: center;
            justify-content: center;
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="max-w-6xl mx-auto p-4 sm:p-6 lg:p-8 font-[Inter]">
        
        <!-- === BANNER SLIDER (Sử dụng Repeater để load từ C#) === -->
        <div class="relative h-64 sm:h-80 md:h-[500px] rounded-2xl overflow-hidden shadow-2xl mb-12 group">
            <div id="heroSlider">
                <asp:Repeater ID="rptSlider" runat="server">
                    <ItemTemplate>
                        <img src='<%# ResolveUrl("~/uploads/images/" + Eval("ImageUrl")) %>' 
                             class="slider-image <%# Container.ItemIndex == 0 ? "active" : "" %>" 
                             alt='<%# Eval("Title") %>'
                             onerror="this.src='https://placehold.co/600x400/e2e8f0/64748b?text=No+Image'">
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="slider-overlay">
                <div class="text-center px-4 animate-fade-in-up">
                    <h1 class="text-white text-4xl sm:text-6xl font-extrabold tracking-tight drop-shadow-lg mb-4">
                        Trà Sữa Milk Cat
                    </h1>
                    <p class="text-gray-100 text-lg sm:text-2xl font-light tracking-wide drop-shadow-md">
                        Đậm vị trà - Thơm vị sữa - Thỏa đam mê
                    </p>
                </div>
            </div>
            <!-- Nút điều hướng (Tùy chỉnh dựa trên số slide) -->
            <div class="absolute bottom-5 left-0 right-0 z-10 flex justify-center gap-2">
                <asp:Repeater ID="rptSliderDots" runat="server">
                    <ItemTemplate>
                        <span class="w-3 h-3 bg-white rounded-full opacity-50 cursor-pointer hover:opacity-100 transition" onclick="changeSlide(<%# Container.ItemIndex %>)"></span>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        
        <!-- Giới thiệu -->
        <section class="text-center mb-12">
            <h2 class="text-3xl font-bold text-[#4c673d] mb-4">Hương Vị Tươi Mới Mỗi Ngày</h2>
            <p class="text-gray-600 text-lg">Chúng tôi mang đến những ly trà sữa thơm ngon, nguyên liệu tự nhiên, phục vụ tận tâm.</p>
        </section>

        <!-- === PHẦN TIN TỨC (Giữ nguyên, load từ C# qua rptNews) === -->
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
                                    <a href='TB_BAI_VIET.aspx?id=<%# Eval("ID_BV") %>'><%# Eval("Tieu_de") %></a>
                                </h4>
                                <p class="text-gray-600 text-sm mb-4 flex-grow line-clamp-2"><%# Eval("Tom_tac") %></p>
                                <div class="mt-auto pt-4 border-t border-gray-100 flex justify-between items-center">
                                    <a href='Default.aspx?ID_BV=<%# Eval("ID_BV") %>' class="text-[#4c673d] font-semibold text-sm hover:underline flex items-center gap-1">
                                        Xem chi tiết <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 8l4 4m0 0l-4 4m4-4H3"></path></svg>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblEmpty" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="Chưa có bài viết nào." CssClass="text-center col-span-3 text-gray-500 py-10" />
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="text-center mt-8">
                <asp:Label ID="lblPaging" runat="server" CssClass="inline-block"></asp:Label>
            </div>
            <div class="text-center mt-8">
                <a href="Default.aspx" class="inline-block px-6 py-2 border border-[#4c673d] text-[#4c673d] rounded-full font-medium hover:bg-[#4c673d] hover:text-white transition">Xem tất cả tin tức</a>
            </div>
        </section>

        <!-- === SẢN PHẨM NỔI BẬT (Sử dụng Repeater để load từ C#) === -->
        <section class="mb-16">
            <h3 class="text-2xl font-semibold text-gray-800 mb-6 text-center">Sản Phẩm Nổi Bật</h3>
            <div class="swiper product-slider pb-10">
                <div class="swiper-wrapper">
                    <asp:Repeater ID="rptFeaturedProducts" runat="server">
                        <ItemTemplate>
                            <div class="swiper-slide">
                                <div class="relative group cursor-pointer overflow-hidden rounded-xl shadow-lg bg-white">
                                    <img src='<%# ResolveUrl("~/uploads/images/" + Eval("HinhAnh")) %>' 
                                         alt='<%# Eval("TenSanPham") %>' 
                                         class="w-full h-64 object-cover transition transform duration-500 group-hover:scale-110"
                                         onerror="this.src='https://placehold.co/600x400/f8a5c3/ffffff?text=No+Image'">
                                    <div class="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black/70 to-transparent p-4 pt-10">
                                        <h4 class="text-white font-bold text-lg"><%# Eval("TenSanPham") %></h4>
                                        <p class="text-gray-200 text-sm opacity-0 group-hover:opacity-100 transition-opacity duration-300"><%# Eval("MoTa") %></p>
                                    </div>
                                    <asp:Label ID="lblHot" runat="server" Visible='<%# Eval("IsHot") %>' CssClass="absolute top-3 right-3 bg-red-500 text-white text-xs font-bold px-2 py-1 rounded-full shadow">HOT</asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="swiper-pagination"></div>
            </div>
        </section>

        <!-- === KHÔNG GIAN QUÁN (Sử dụng Repeater để load từ C#) === -->
        <section class="mb-12">
            <h3 class="text-2xl font-semibold text-gray-800 mb-6 text-center">Không Gian Quán</h3>
            <div class="grid grid-cols-1 sm:grid-cols-3 gap-6">
                <asp:Repeater ID="rptShopImages" runat="server">
                    <ItemTemplate>
                        <img src='<%# ResolveUrl("~/uploads/images/" + Eval("ImageUrl")) %>' 
                             alt='<%# Eval("AltText") %>' 
                             class="w-full h-80 object-cover rounded-lg shadow-lg hover:shadow-xl transition duration-300"
                             onerror="this.src='https://placehold.co/600x320/a8c2c6/ffffff?text=No+Image'">
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </section>

        <!-- Call to Action -->
        <div class="flex flex-col sm:flex-row gap-4 justify-center mt-10">
            <a href="ProductUser.aspx" class="px-6 py-3 bg-[#4c673d] text-white font-semibold rounded-lg shadow-md hover:bg-green-700 transition duration-300 transform hover:scale-105 text-center">
                XEM MENU
            </a>
            <a href="ShoppingUser.aspx" class="px-6 py-3 bg-indigo-500 text-white font-semibold rounded-lg shadow-md hover:bg-indigo-700 transition duration-300 transform hover:scale-105 text-center">
                ĐẶT HÀNG NGAY
            </a>
        </div>

        <!-- Footer đơn giản -->
        <footer class="text-center mt-12 pt-6 border-t border-gray-300">
            <p class="text-gray-500 text-sm">&copy; 2025 Cửa Hàng Trà Sữa Đồng Tháp. Vui lòng liên hệ 0337335364 để được hỗ trợ.</p>
        </footer>
    </div>

    <!-- Script xử lý Slider -->
    <script>
        // --- SCRIPT CHO SLIDER ---
        let currentSlide = 0;
        const slides = document.querySelectorAll('.slider-image');
        const dots = document.querySelectorAll('.absolute.bottom-5 span');
        const totalSlides = slides.length;

        // Hàm chuyển slide
        function changeSlide(index) {
            // Xóa class active cũ
            slides[currentSlide].classList.remove('active');
            dots[currentSlide].classList.replace('opacity-100', 'opacity-50');

            // Cập nhật index mới (nếu không truyền index thì tự tăng)
            if (typeof index === 'number') {
                currentSlide = index;
            } else {
                currentSlide = (currentSlide + 1) % totalSlides;
            }

            // Thêm class active mới
            slides[currentSlide].classList.add('active');
            dots[currentSlide].classList.replace('opacity-50', 'opacity-100');
        }

        // Tự động chạy slider mỗi 4 giây
        let slideInterval = setInterval(changeSlide, 4000);

        // Reset timer khi người dùng click thủ công
        function manualSlide(index) {
            clearInterval(slideInterval);
            changeSlide(index);
            slideInterval = setInterval(changeSlide, 4000);
        }

        // Gán sự kiện click cho các chấm tròn
        dots.forEach((dot, index) => {
            dot.onclick = () => manualSlide(index);
        });

        // Khởi tạo Swiper Slider cho phần sản phẩm nổi bật
        var productSwiper = new Swiper('.product-slider', {
            slidesPerView: 1,
            spaceBetween: 20,
            loop: true,
            autoplay: {
                delay: 3000,
                disableOnInteraction: false,
            },
            pagination: {
                el: '.swiper-pagination',
                clickable: true,
            },
            breakpoints: {
                640: {
                    slidesPerView: 2,
                    spaceBetween: 20,
                },
                1024: {
                    slidesPerView: 3,
                    spaceBetween: 30,
                },
            },
        });
    </script>
</asp:Content>