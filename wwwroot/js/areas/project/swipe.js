function swipe(button, direction) {
    const card = button.closest('.job-card');
    if (!card) return;

    const className = direction === 'left' ? 'swipe-left' : 'swipe-right';
    card.classList.add(className);

    card.addEventListener('transitionend', () => {
        card.remove();
        /*renderStars();*/
    }, { once: true });
}
