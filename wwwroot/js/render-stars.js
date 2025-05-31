document.addEventListener('DOMContentLoaded', () => {
    renderStars();
});

function renderStars() {
    const containers = document.querySelectorAll('.star-display');
    containers.forEach(container => {
        const rating = parseFloat(container.dataset.rating) / 2;
        const stars = container.querySelectorAll('.star');

        stars.forEach((star, index) => {
            const value = index + 1;
            if (rating >= value) {
                star.style.backgroundPosition = 'left'; // full star
            } else if (rating >= value - 0.5) {
                star.style.backgroundPosition = 'center'; // half star
            } else {
                star.style.backgroundPosition = 'right'; // empty star
            }
        });
    });
}