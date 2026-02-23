// auth.js

function switchTab(tabName) {
    // 1. Xác định các Element
    const loginForm = document.getElementById('tab-content-login');
    const registerForm = document.getElementById('tab-content-register');
    const loginBtn = document.getElementById('btn-tab-login');
    const registerBtn = document.getElementById('btn-tab-register');

    // 2. Xử lý Logic ẩn/hiện Form
    if (tabName === 'login') {
        loginForm.classList.remove('hidden');
        registerForm.classList.add('hidden');

        // Style cho nút
        loginBtn.classList.add('active');
        registerBtn.classList.remove('active');
    } else {
        loginForm.classList.add('hidden');
        registerForm.classList.remove('hidden');

        // Style cho nút
        registerBtn.classList.add('active');
        loginBtn.classList.remove('active');
    }
}
// --- LOGIC ROLE SELECTOR ---

// 1. Hàm bật/tắt dropdown
function toggleRoleList() {
    const dropdown = document.getElementById('roleDropdown');
    const options = document.getElementById('roleOptions');

    dropdown.classList.toggle('open');
    options.classList.toggle('active');
}

// 2. Hàm chọn vai trò
function selectRole(event, value, text, iconName) {
    // a. Cập nhật giao diện hiển thị
    document.getElementById('current-role-text').innerText = text;

    // Cập nhật icon (Cần gọi lucide.createIcons sau khi đổi DOM)
    // Cách đơn giản nhất: Đổi class hoặc innerHTML nếu dùng thư viện khác
    // Nhưng với Lucide JS, ta đổi attribute và gọi lại render
    const iconEl = document.getElementById('current-role-icon');
    iconEl.setAttribute('data-lucide', iconName);
    lucide.createIcons(); // Re-render icon mới

    // b. Cập nhật dấu tích (Check icon) trong danh sách
    document.getElementById('check-student').classList.add('hidden');
    document.getElementById('check-instructor').classList.add('hidden');
    document.getElementById('check-' + value).classList.remove('hidden');

    // c. Cập nhật giá trị cho Input ẩn (Quan trọng để gửi về Server)
    const inputLogin = document.getElementById('input-role-login');
    const inputRegister = document.getElementById('input-role-register');

    if (inputLogin) inputLogin.value = value;
    if (inputRegister) inputRegister.value = value;

    // d. Ngăn sự kiện nổi bọt (để không kích hoạt toggleRoleList lần nữa)
    event.stopPropagation();

    // e. Đóng dropdown
    toggleRoleList();
}

// 3. Đóng dropdown khi click ra ngoài
document.addEventListener('click', function (event) {
    const dropdown = document.getElementById('roleDropdown');
    const options = document.getElementById('roleOptions');

    // Nếu click không nằm trong dropdown thì đóng nó
    if (!dropdown.contains(event.target)) {
        dropdown.classList.remove('open');
        options.classList.remove('active');
    }
});