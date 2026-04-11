document.addEventListener('DOMContentLoaded', () => {
    // KHỞI TẠO LUCIDE ICONS
    lucide.createIcons();

    const excelUpload = document.getElementById('excel-upload');
    const checkAll = document.getElementById('check-all');
    const itemChecks = document.querySelectorAll('.question-check');
    const deleteBtn = document.getElementById('btn-delete-multiple');
    const countSpan = document.getElementById('selected-count');
    const singleDeletes = document.querySelectorAll('.single-delete');

    // --- LOGIC UPLOAD EXCEL ---
    if (excelUpload) {
        excelUpload.addEventListener('change', async function (e) {
            const file = e.target.files[0];
            if (!file) return;

            const validExtensions = ['xlsx', 'xls'];
            const fileExtension = file.name.split('.').pop().toLowerCase();

            if (!validExtensions.includes(fileExtension)) {
                showError('Sai định dạng!', 'Vui lòng chọn file Excel (.xlsx hoặc .xls)');
                this.value = '';
                return;
            }

            showLoading('Đang nạp dữ liệu...', 'Vui lòng không đóng trình duyệt.');

            const formData = new FormData();
            formData.append('excelFile', file);

            try {
                const response = await fetch('/Student/UploadPersonalExam', {
                    method: 'POST',
                    body: formData
                });
                const result = await response.json();

                if (result.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công!',
                        text: 'Đã tạo bộ đề thi mới.',
                        timer: 2000,
                        showConfirmButton: false
                    }).then(() => {
                        window.location.href = `/Student/ClassifyExam?examId=${result.examId}`;
                    });
                } else {
                    let errorMsg = result.errors.map(err => `Dòng ${err.dong}: ${err.loi}`).join('<br>');
                    showError('File Excel có lỗi!', errorMsg, true);
                }
            } catch (error) {
                showError('Lỗi mạng!', 'Không thể kết nối đến máy chủ.');
            } finally {
                this.value = '';
            }
        });
    }

    // --- LOGIC CHECKBOX & TOOLBAR ---
    if (checkAll) {
        checkAll.addEventListener('change', () => {
            itemChecks.forEach(c => {
                c.checked = checkAll.checked;
                toggleRowHighlight(c);
            });
            updateDeleteToolbar();
        });
    }

    itemChecks.forEach(c => {
        c.addEventListener('change', () => {
            toggleRowHighlight(c);
            updateDeleteToolbar();
            if (!c.checked) checkAll.checked = false;
            if (document.querySelectorAll('.question-check:checked').length === itemChecks.length) {
                checkAll.checked = true;
            }
        });
    });

    function updateDeleteToolbar() {
        const checkedCount = document.querySelectorAll('.question-check:checked').length;
        if (countSpan) countSpan.innerText = checkedCount;
        if (deleteBtn) {
            deleteBtn.style.display = checkedCount > 0 ? 'inline-flex' : 'none';
        }
    }

    function toggleRowHighlight(checkbox) {
        const row = checkbox.closest('tr');
        if (checkbox.checked) row.classList.add('row-selected');
        else row.classList.remove('row-selected');
    }

    // --- LOGIC XÓA ---
    singleDeletes.forEach(btn => {
        btn.addEventListener('click', function () {
            const id = this.getAttribute('data-id');
            confirmAndPathDelete([id]);
        });
    });

    if (deleteBtn) {
        deleteBtn.addEventListener('click', () => {
            const selectedIds = Array.from(document.querySelectorAll('.question-check:checked'))
                .map(cb => cb.value);
            confirmAndPathDelete(selectedIds);
        });
    }

    async function confirmAndPathDelete(ids) {
        const result = await Swal.fire({
            title: ids.length > 1 ? `Xóa ${ids.length} câu hỏi?` : 'Xóa câu hỏi này?',
            text: "Toàn bộ đáp án liên quan cũng sẽ bị xóa vĩnh viễn!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc2626',
            cancelButtonColor: '#64748b',
            confirmButtonText: 'Đúng, xóa đi!',
            cancelButtonText: 'Hủy'
        });

        if (result.isConfirmed) {
            showLoading('Đang xử lý...', 'Vui lòng chờ giây lát.');

            try {
                const response = await fetch('/Student/DeleteQuestions', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ ids: ids })
                });

                const data = await response.json();
                if (data.success) {
                    Swal.fire('Đã xóa!', data.message, 'success').then(() => location.reload());
                } else {
                    showError('Lỗi!', data.message);
                }
            } catch (err) {
                showError('Lỗi hệ thống!', 'Không thể thực hiện yêu cầu.');
            }
        }
    }

    // Helpers
    function showLoading(title, text) {
        Swal.fire({ title, text, allowOutsideClick: false, didOpen: () => { Swal.showLoading(); } });
    }

    function showError(title, text, isHtml = false) {
        Swal.fire({ icon: 'error', title, [isHtml ? 'html' : 'text']: text, confirmButtonColor: '#dc2626' });
    }
});