// ==========================================
// 1. KHỞI TẠO ICON LUCIDE
// ==========================================
lucide.createIcons();

// ==========================================
// 2. BỘ ĐẾM NGƯỢC THỜI GIAN (COUNTDOWN TIMER)
// ==========================================

// Lấy biến TIME_LIMIT_MINUTES từ thẻ <script> trong HTML, nếu không có thì mặc định là 60
const limitMinutes = typeof TIME_LIMIT_MINUTES !== 'undefined' ? TIME_LIMIT_MINUTES : 60;
const totalTime = limitMinutes * 60;
let timeRemaining = totalTime;

const countdownDisplay = document.getElementById('countdownDisplay');
const timerBadge = countdownDisplay?.parentElement;

function startTimer() {
    if (!countdownDisplay) return;

    const timerInterval = setInterval(function () {
        let minutes = Math.floor(timeRemaining / 60);
        let seconds = timeRemaining % 60;

        let formattedMinutes = minutes < 10 ? "0" + minutes : minutes;
        let formattedSeconds = seconds < 10 ? "0" + seconds : seconds;

        countdownDisplay.textContent = formattedMinutes + ":" + formattedSeconds;

        // Tính phần trăm thời gian còn lại
        let percentRemaining = (timeRemaining / totalTime) * 100;

        // Đổi màu linh hoạt theo từng mốc %
        if (percentRemaining <= 10 && timeRemaining > 0) {
            timerBadge.style.backgroundColor = '#fef2f2';
            timerBadge.style.color = '#ef4444';
            timerBadge.style.borderColor = '#fecaca';
        }
        else if (percentRemaining <= 30) {
            timerBadge.style.backgroundColor = '#fff7ed';
            timerBadge.style.color = '#ea580c';
            timerBadge.style.borderColor = '#fed7aa';
        }
        else if (percentRemaining <= 50) {
            timerBadge.style.backgroundColor = '#fefce8';
            timerBadge.style.color = '#ca8a04';
            timerBadge.style.borderColor = '#fef08a';
        }

        // Xử lý khi HẾT GIỜ
        if (timeRemaining <= 0) {
            clearInterval(timerInterval);
            countdownDisplay.textContent = "00:00";
            alert("Đã hết thời gian làm bài! Hệ thống sẽ tự động lưu và nộp bài của bạn.");

            // Gọi hàm nộp bài và truyền tham số true (báo hiệu là nộp tự động, không cần confirm)
            submitExam(true);
        } else {
            timeRemaining--;
        }
    }, 1000);
}

// ==========================================
// 3. LOGIC CHỌN ĐÁP ÁN & THANH TIẾN ĐỘ
// ==========================================

function initQuestionLogic() {
    const allRadios = document.querySelectorAll('.answer-option input[type="radio"]');
    const gridItems = document.querySelectorAll('.grid-item');
    const totalQuestions = gridItems.length;

    const progressFill = document.querySelector('.progress-fill');
    const progressText = document.querySelector('.progress-header span:first-child');
    const percentText = document.querySelector('.progress-header .percent');
    const answeredLegendCount = document.querySelectorAll('.legend-count')[0];
    const unansweredLegendCount = document.querySelectorAll('.legend-count')[1];

    allRadios.forEach(radio => {
        radio.addEventListener('change', function () {
            // Lấy ID của khối câu hỏi chứa radio này (VD: question-block-1)
            const block = this.closest('.question-block');
            const blockId = block.id;
            const qIndex = parseInt(blockId.replace('question-block-', ''));
            const gridIndex = qIndex - 1;

            // Cập nhật màu ô vuông bên Sidebar
            if (gridItems[gridIndex]) {
                gridItems[gridIndex].classList.add('answered');
            }

            // Đếm số câu đã làm (dựa trên số lượng ô đã đổi class 'answered')
            const answeredCount = document.querySelectorAll('.grid-item.answered').length;

            // Cập nhật giao diện
            progressText.textContent = `Tiến độ: ${answeredCount}/${totalQuestions} câu`;
            const percent = Math.round((answeredCount / totalQuestions) * 100);
            percentText.textContent = `${percent}%`;
            progressFill.style.width = `${percent}%`;

            answeredLegendCount.textContent = answeredCount;
            unansweredLegendCount.textContent = totalQuestions - answeredCount;
        });
    });
}

// ==========================================
// 4. LOGIC ĐÁNH DẤU CÂU HỎI (FLAG)
// ==========================================

function initFlagLogic() {
    const flagButtons = document.querySelectorAll('.btn-flag');
    const gridItems = document.querySelectorAll('.grid-item');
    const flaggedLegendCount = document.querySelectorAll('.legend-count')[2];

    flagButtons.forEach(btn => {
        btn.addEventListener('click', function () {
            this.classList.toggle('active-flag');

            const questionNumText = this.closest('.card').querySelector('.question-number').textContent;
            const gridIndex = parseInt(questionNumText) - 1;

            if (gridItems[gridIndex]) {
                gridItems[gridIndex].classList.toggle('flagged');
            }

            const totalFlagged = document.querySelectorAll('.grid-item.flagged').length;
            flaggedLegendCount.textContent = totalFlagged;
        });
    });
}

// ==========================================
// 5. LOGIC ĐIỀU HƯỚNG CÂU HỎI (NAVIGATION)
// ==========================================

let currentQuestionIndex = 1;

function showQuestion(index) {
    const questions = document.querySelectorAll('.question-block');
    const totalQs = questions.length;

    questions.forEach(q => q.style.display = 'none');

    const targetQuestion = document.getElementById(`question-block-${index}`);
    if (targetQuestion) {
        targetQuestion.style.display = 'block';
    }

    document.querySelectorAll('.grid-item').forEach(item => {
        item.classList.remove('active');
    });
    const activeGridItem = document.querySelector(`.grid-item[onclick="jumpToQuestion(${index})"]`);
    if (activeGridItem) {
        activeGridItem.classList.add('active');
    }

    updateNavButtons(index, totalQs);
    currentQuestionIndex = index;
}

function updateNavButtons(index, totalQs) {
    const currentBlock = document.getElementById(`question-block-${index}`);
    if (!currentBlock) return;

    const btnPrev = currentBlock.querySelector('.btn-nav-prev');
    const btnNext = currentBlock.querySelector('.btn-nav-next');

    if (index === 1) {
        btnPrev.style.visibility = 'hidden';
    } else {
        btnPrev.style.visibility = 'visible';
    }

    // Nếu là câu cuối thì đổi chữ "Câu sau" thành "Nộp bài"
    if (index === totalQs) {
        btnNext.innerHTML = '<span>Nộp bài</span> <i data-lucide="send" width="18" height="18"></i>';
        btnNext.classList.add('btn-finish');
        btnNext.onclick = function () { submitExam(false); }; // Nộp bằng tay
    } else {
        btnNext.innerHTML = 'Câu sau <i data-lucide="chevron-right" width="18" height="18"></i>';
        btnNext.classList.remove('btn-finish');
        btnNext.onclick = nextQuestion;
    }

    lucide.createIcons();
}

function nextQuestion() {
    const totalQs = document.querySelectorAll('.question-block').length;
    if (currentQuestionIndex < totalQs) {
        showQuestion(currentQuestionIndex + 1);
    }
}

function prevQuestion() {
    if (currentQuestionIndex > 1) {
        showQuestion(currentQuestionIndex - 1);
    }
}

function jumpToQuestion(index) {
    showQuestion(index);
}

// ==========================================
// 6. LOGIC NỘP BÀI (SUBMIT EXAM VỚI MODAL)
// ==========================================

let pendingAnswers = []; // Biến mảng tạm để lưu đáp án chờ nộp

function submitExam(isAutoSubmit = false) {
    pendingAnswers = [];
    const questionBlocks = document.querySelectorAll('.question-block');
    const totalQs = questionBlocks.length;
    let answeredCount = 0;
    let flaggedCount = document.querySelectorAll('.grid-item.flagged').length;

    // 1. Thu thập dữ liệu
    questionBlocks.forEach(block => {
        const radio = block.querySelector('input[type="radio"]:checked');
        const firstRadio = block.querySelector('input[type="radio"]');
        if (firstRadio) {
            const questionId = firstRadio.name.replace('question_', '');
            pendingAnswers.push({
                QuestionId: parseInt(questionId),
                SelectedAnswerId: radio ? parseInt(radio.value) : 0
            });
            if (radio) answeredCount++;
        }
    });

    // Nếu hết giờ (isAutoSubmit = true), bỏ qua Modal và nộp thẳng luôn
    if (isAutoSubmit) {
        executeSubmit();
        return;
    }

    // 2. Tính toán hiển thị lên Modal
    const unansweredCount = totalQs - answeredCount;

    document.getElementById('modal-answered').textContent = `${answeredCount}/${totalQs} câu`;
    document.getElementById('modal-unanswered').textContent = `${unansweredCount} câu`;
    document.getElementById('modal-flagged').textContent = `${flaggedCount} câu`;

    const warningBox = document.getElementById('modal-warning-box');
    const warningText = document.getElementById('modal-warning-text');

    // Nếu còn câu chưa làm thì hiện cảnh báo cam, nếu làm full rồi thì ẩn đi cho đẹp
    if (unansweredCount > 0) {
        warningBox.style.display = 'flex';
        warningText.textContent = `Bạn còn ${unansweredCount} câu chưa trả lời. Bài làm sẽ không thể chỉnh sửa sau khi nộp.`;
    } else {
        warningBox.style.display = 'none';
    }

    // 3. Hiển thị Modal
    document.getElementById('submitModal').style.display = 'flex';
    lucide.createIcons(); // Load icon cho Modal
}

function closeSubmitModal() {
    document.getElementById('submitModal').style.display = 'none';
}

let targetRedirectUrl = '/'; // Biến lưu URL chuyển hướng sau khi đóng modal kết quả

async function executeSubmit() {
    // 1. Ẩn modal xác nhận và làm mờ nút Nộp bài ngoài màn hình chính
    closeSubmitModal();
    const mainSubmitBtn = document.querySelector('.btn-submit');
    if (mainSubmitBtn) {
        mainSubmitBtn.disabled = true;
        mainSubmitBtn.innerHTML = '<span>Đang nộp...</span>';
    }

    const pathParts = window.location.pathname.split('/');
    const examId = parseInt(pathParts[pathParts.length - 1]) || 1;

    try {
        const response = await fetch('/TestAttempt/SubmitExam', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
            },
            body: JSON.stringify({
                ExamId: examId,
                UserAnswers: pendingAnswers
            })
        });

        const result = await response.json();

        if (result.success) {
            // 2. Gán dữ liệu trả về vào Modal Kết quả
            document.getElementById('result-score').textContent = result.score;
            document.getElementById('result-correct').textContent = `${result.correctCount} / ${result.totalQuestions}`;

            // THÊM MỚI: Gán dữ liệu thống kê độ khó
            // ĐỔ DỮ LIỆU THỐNG KÊ ĐỘ KHÓ VÀO MODAL
            document.getElementById('result-easy').textContent = `${result.correctEasy} câu`;
            document.getElementById('result-medium').textContent = `${result.correctMedium} câu`;
            document.getElementById('result-hard').textContent = `${result.correctHard} câu`;
            // Nếu bạn có URL kết quả cụ thể (ví dụ /TestAttempt/Result/ID), thì cập nhật vào biến
            if (result.attemptId) {
                targetRedirectUrl = '/TestAttempt/Result/' + result.attemptId;
            }

            // 3. Hiển thị Modal Kết quả
            document.getElementById('resultModal').style.display = 'flex';
            lucide.createIcons(); // Load lại icon trong modal mới

        } else {
            alert("Có lỗi xảy ra: " + result.message);
            if (mainSubmitBtn) resetSubmitBtn(mainSubmitBtn);
        }
    } catch (error) {
        console.error("Lỗi khi nộp bài:", error);
        alert("Không thể gửi dữ liệu nộp bài. Vui lòng kiểm tra kết nối mạng!");
        if (mainSubmitBtn) resetSubmitBtn(mainSubmitBtn);
    }
}

// Hàm kích hoạt khi người dùng bấm nút "Xem chi tiết" trên Modal kết quả
function goToResultPage() {
    window.location.href = targetRedirectUrl;
}
// Hàm phụ để reset lại nút nộp bài khi gặp lỗi mạng
function resetSubmitBtn(btn) {
    btn.disabled = false;
    btn.innerHTML = '<i data-lucide="send" width="16" height="16"></i><span>Nộp bài</span>';
    lucide.createIcons();
}

// ==========================================
// 7. KHỞI CHẠY KHI TRANG VỪA LOAD XONG
// ==========================================
window.onload = function () {
    startTimer();
    initQuestionLogic();
    initFlagLogic();
    showQuestion(1);
};