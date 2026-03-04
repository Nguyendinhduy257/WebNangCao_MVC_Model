document.addEventListener("DOMContentLoaded", () => {
    // Hiệu ứng nhảy số điểm
    const scoreElement = document.getElementById("animatedScore");
    if (scoreElement) {
        const target = parseFloat(scoreElement.getAttribute("data-target"));
        let current = 0;
        const increment = target / 50; // Tốc độ chạy

        const updateScore = () => {
            if (current < target) {
                current += increment;
                scoreElement.innerText = current.toFixed(1);
                setTimeout(updateScore, 20);
            } else {
                scoreElement.innerText = target;
            }
        };
        updateScore();
    }

    // Hiệu ứng hover nhẹ vào các câu hỏi
    const cards = document.querySelectorAll(".question-card");
    cards.forEach(card => {
        card.addEventListener("mouseenter", () => {
            card.style.transform = "translateY(0px) Scale(1.03)";
            card.style.transition = "0.3s";
        });
        card.addEventListener("mouseleave", () => {
            card.style.transform = "translateY(0) Scale(1)";
        });
    });
});