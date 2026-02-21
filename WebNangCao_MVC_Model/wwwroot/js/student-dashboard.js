document.addEventListener('DOMContentLoaded', function () {
    //  Khởi tạo Lucide Icons (Biến các thẻ <i> thành <svg>)
    //Khởi tạo giúp đọc và hiểu lệnh: "Data-lucide" trong HTML
    lucide.createIcons();
    // 1. Logic Ẩn/Hiện Bộ lọc nâng cao
    const btnToggleFilter = document.getElementById('btnToggleFilter');
    const advancedFilter = document.getElementById('advancedFilter');

    btnToggleFilter.addEventListener('click', function () {
        // Thêm/Xóa class 'active' để ẩn hiện (đã css display: grid)
        advancedFilter.classList.toggle('active');

        // Đổi màu nút lọc để tạo cảm giác đang được chọn
        if (advancedFilter.classList.contains('active')) {
            btnToggleFilter.style.backgroundColor = '#eaeaea';
        } else {
            btnToggleFilter.style.backgroundColor = 'var(--bg-main)';
        }
    });

    // 2. Logic chuyển đổi active cho các Tabs
    const tabButtons = document.querySelectorAll('.tab-btn');

    tabButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Xóa class active ở tất cả các nút
            tabButtons.forEach(btn => btn.classList.remove('active'));
            // Thêm class active vào nút vừa được click
            this.classList.add('active');

            // Ở đây bạn có thể thêm logic AJAX để filter bài thi theo Tab
            // ví dụ: fetchExamsByStatus(this.innerText);
        });
    });
    // --- LOGIC AVATAR DROPDOWN MENU ---
    const userAvatar = document.getElementById('userAvatar');
    const avatarDropdown = document.getElementById('avatarDropdown');

    if (userAvatar && avatarDropdown) {
        // 1. Click vào Avatar thì bật/tắt Menu
        userAvatar.addEventListener('click', function (event) {
            event.stopPropagation(); // Ngăn chặn sự kiện click lan ra ngoài body
            avatarDropdown.classList.toggle('show');
        });

        // 2. Click ra bất kỳ đâu trên màn hình thì tự đóng Menu
        document.addEventListener('click', function (event) {
            // Nếu menu đang mở VÀ chỗ click chuột KHÔNG NẰM TRONG menu hoặc avatar
            if (avatarDropdown.classList.contains('show') &&
                !avatarDropdown.contains(event.target) &&
                event.target !== userAvatar) {

                avatarDropdown.classList.remove('show'); // Đóng menu lại
            }
        });
    }
});