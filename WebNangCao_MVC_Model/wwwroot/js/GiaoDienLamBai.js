// ==========================================
// 1. KHỞI TẠO ICON LUCIDE
// ==========================================
lucide.createIcons();

// ==========================================
// 2. BỘ ĐẾM NGƯỢC THỜI GIAN (COUNTDOWN TIMER)
// ==========================================

// Cài đặt thời gian làm bài: Ví dụ 60 phút
const TIME_LIMIT_MINUTES = 60;
const totalTime = TIME_LIMIT_MINUTES * 60; // Lưu lại TỔNG SỐ GIÂY ban đầu
let timeRemaining = totalTime;

// Lấy thẻ HTML hiển thị thời gian
const countdownDisplay = document.getElementById('countdownDisplay');
const timerBadge = countdownDisplay.parentElement;

function startTimer() {
    const timerInterval = setInterval(function () {

        let minutes = Math.floor(timeRemaining / 60);
        let seconds = timeRemaining % 60;
        // Định dạng hiển thị: luôn 2 chữ số (ví dụ 09:05 thay vì 9:5)
        let formattedMinutes = minutes < 10 ? "0" + minutes : minutes;
        let formattedSeconds = seconds < 10 ? "0" + seconds : seconds;

        countdownDisplay.textContent = formattedMinutes + ":" + formattedSeconds;

        //  TÍNH PHẦN TRĂM THỜI GIAN CÒN LẠI
        let percentRemaining = (timeRemaining / totalTime) * 100;

        //  ĐỔI MÀU LINH HOẠT THEO TỪNG MỐC %
        if (percentRemaining <= 10 && timeRemaining > 0) {
            // Dưới 10% -> Báo động đỏ
            timerBadge.style.backgroundColor = '#fef2f2';
            timerBadge.style.color = '#ef4444';
            timerBadge.style.borderColor = '#fecaca';
        }
        else if (percentRemaining <= 30) {
            // Dưới 30% -> Cảnh báo cam
            timerBadge.style.backgroundColor = '#fff7ed';
            timerBadge.style.color = '#ea580c';
            timerBadge.style.borderColor = '#fed7aa';
        }
        else if (percentRemaining <= 50) {
            // Dưới 50% -> Nhắc nhở vàng
            timerBadge.style.backgroundColor = '#fefce8';
            timerBadge.style.color = '#ca8a04';
            timerBadge.style.borderColor = '#fef08a';
        }

        // Xử lý khi HẾT GIỜ
        if (timeRemaining <= 0) {
            clearInterval(timerInterval);
            countdownDisplay.textContent = "00:00";

            alert("Đã hết thời gian làm bài! Hệ thống sẽ tự động lưu và nộp bài của bạn.");
            autoSubmitExam();
        } else {
            timeRemaining--;
        }

    }, 1000); // 1000ms = 1 giây
}

// ==========================================
// 3. XỬ LÝ NỘP BÀI
// ==========================================

function submitExam() {
    if (confirm("Bạn có chắc chắn muốn nộp bài sớm không? Thời gian vẫn còn.")) {
        processSubmission();
    }
}

function autoSubmitExam() {
    processSubmission();
}

function processSubmission() {
    console.log("Đang thu thập đáp án...");
    // TODO: Viết code lấy dữ liệu các câu đã chọn và gửi về Server

    alert("Nộp bài thành công! Đang chuyển hướng về trang kết quả...");
    window.location.href = "/Student/Dashboard";
}

// ==========================================
// 4. KHỞI CHẠY CÁC HÀM KHI TRANG VỪA LOAD XONG
// ==========================================
window.onload = function () {
    startTimer();
    initQuestionLogic(); // Gọi thêm hàm này để kích hoạt tai nghe sự kiện
    initFlagLogic(); // Bổ sung hàm cắm cờ vào đây
};
// ==========================================
// 5. LOGIC CHỌN ĐÁP ÁN & THANH TIẾN ĐỘ
// ==========================================

function initQuestionLogic() {
    // Lấy tất cả các nút radio chọn đáp án
    const allRadios = document.querySelectorAll('.answer-option input[type="radio"]');

    // Tổng số câu hỏi (đếm số lượng ô vuông bên sidebar)
    const gridItems = document.querySelectorAll('.grid-item');
    const totalQuestions = gridItems.length;

    // Các phần tử cần cập nhật trên giao diện
    const progressFill = document.querySelector('.progress-fill');
    const progressText = document.querySelector('.progress-header span:first-child');
    const percentText = document.querySelector('.progress-header .percent');
    const answeredLegendCount = document.querySelectorAll('.legend-count')[0]; // Ô đếm "Đã trả lời"
    const unansweredLegendCount = document.querySelectorAll('.legend-count')[1]; // Ô đếm "Chưa trả lời"

    // Gắn "tai nghe" sự kiện cho TẤT CẢ các đáp án
    allRadios.forEach(radio => {
        radio.addEventListener('change', function () {

            //  CẬP NHẬT MÀU Ô VUÔNG BÊN SIDEBAR
            // Lấy tên của nhóm câu hỏi. Ví dụ name="question1" -> Cắt chữ "question" đi, còn lại số "1"
            const questionNum = this.name.replace('question', '');
            const gridIndex = parseInt(questionNum) - 1; // Vì mảng bắt đầu từ 0, câu 1 sẽ là index 0

            if (gridItems[gridIndex]) {
                gridItems[gridIndex].classList.add('answered'); // Thêm class xanh lá vào
            }

            //  TÍNH TOÁN VÀ CẬP NHẬT THANH TIẾN ĐỘ
            // Đếm số lượng câu đã làm (bằng cách đếm số lượng radio đang được tick chọn)
            const answeredCount = document.querySelectorAll('.answer-option input[type="radio"]:checked').length;

            // Cập nhật dòng chữ "Tiến độ: x/y câu"
            progressText.textContent = `Tiến độ: ${answeredCount}/${totalQuestions} câu`;

            // Tính phần trăm và cập nhật độ dài thanh bar
            const percent = Math.round((answeredCount / totalQuestions) * 100);
            percentText.textContent = `${percent}%`;
            progressFill.style.width = `${percent}%`;

            // 3️⃣ CẬP NHẬT CHÚ THÍCH (LEGEND) BÊN DƯỚI
            answeredLegendCount.textContent = answeredCount;
            unansweredLegendCount.textContent = totalQuestions - answeredCount;
        });
    });
}
// ==========================================
// 6. LOGIC ĐÁNH DẤU CÂU HỎI (FLAG)
// ==========================================

function initFlagLogic() {
    const flagButtons = document.querySelectorAll('.btn-flag');
    const gridItems = document.querySelectorAll('.grid-item');

    // Lấy ô hiển thị số lượng "Đánh dấu" ở phần chú thích (Legend)
    // Nó là phần tử .legend-count thứ 3 (index 2)
    const flaggedLegendCount = document.querySelectorAll('.legend-count')[2];

    flagButtons.forEach(btn => {
        btn.addEventListener('click', function () {

            // 1️ Đổi trạng thái class của nút bấm (Bật/Tắt màu cam)
            this.classList.toggle('active-flag');

            // 2️ Tìm xem nút cờ này đang thuộc về câu số mấy
            // Lấy thẻ div chứa số câu hỏi (ví dụ: <div class="question-number">1</div>)
            const questionNumText = this.closest('.card').querySelector('.question-number').textContent;
            const gridIndex = parseInt(questionNumText) - 1; // Câu 1 thì tương ứng ô index 0

            // 3️ Cập nhật hiệu ứng cho ô vuông bên Sidebar
            if (gridItems[gridIndex]) {
                // Bật/Tắt class 'flagged' (chấm cam) vừa viết trong CSS
                gridItems[gridIndex].classList.toggle('flagged');
            }

            // 4️ Đếm lại tổng số câu đang cắm cờ và cập nhật phần Chú thích
            const totalFlagged = document.querySelectorAll('.grid-item.flagged').length;
            flaggedLegendCount.textContent = totalFlagged;
        });
    });
}
