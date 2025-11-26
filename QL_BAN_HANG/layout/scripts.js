//
// FILE: layout/scripts.js
// Chứa chức năng Responsive Navigation
//

document.addEventListener('DOMContentLoaded', function () {
    // Đảm bảo nút toggle có ID là 'menuToggle' trong HTML
    const toggleButton = document.getElementById('menuToggle');
    const navMenu = document.getElementById('nav');

    // Chỉ chạy nếu cả hai phần tử tồn tại
    if (toggleButton && navMenu) {

        // Định nghĩa hàm bật/tắt menu
        function toggleNav() {
            // Thêm/xóa class 'show' để hiện/ẩn menu (dựa trên CSS trong nav.css)
            navMenu.classList.toggle('show');
        }

        // Gắn sự kiện click
        toggleButton.addEventListener('click', toggleNav);
    }
});