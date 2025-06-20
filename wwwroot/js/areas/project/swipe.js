let remainingVacancy = 10;

async function swipeVacancy(button, direction) {
    const card = button.closest('.job-card');
    const vacancyId = card.dataset.vacancyId;
    
    remainingVacancy--;
    
    if (remainingVacancy <= 2) {
        loadMoreVacancies();
    }

    if (direction === 'right') {
        try {
            const response = await fetch('/api/like/vacancy', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ ToVacancyId: vacancyId })
            });

            const result = await response.json();
            if (!result.success) {
                console.error('Ошибка при лайке:', result.error);
            } else if (result.isMatch) {
                alert('У вас мэтч!');
            }
        } catch (err) {
            console.error('Ошибка сети:', err);
        }
    }

    // Убираем карточку из UI
    card.classList.add(direction === 'right' ? 'swipe-right' : 'swipe-left');
    setTimeout(() => {
        card.remove()

        const remainingCards = document.querySelectorAll('.job-card').length;
        
        if (remainingCards === 0) {
            document.getElementById('noVacanciesMessage').style.display = 'block';
        }
    } , 500);
}

let loading = false;
let skip = 10;

async function loadMoreVacancies() {
    /*if (loading) return;*/
    loading = true;

    try {
        const response = await fetch(`/api/project/vacancy?skip=1&take=10`, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });

        const html = await response.text();
        const container = document.getElementById('cardContainer');
        container.insertAdjacentHTML('beforeend', html);

        const addedCount = (html.match(/class="job-card"/g) || []).length;
        skip += addedCount;

        if (addedCount > 0) {
            renderStars(); // Если ты используешь отложенный рендер звёзд
        }
    } finally {
        loading = false;
    }
}

async function swipeResume(button, direction) {
    const card = button.closest('.job-card');
    const resumeId = card.dataset.resumeId;

    if (direction === 'right') {
        try {
            const response = await fetch('/api/like/resume', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ ToResumeId: resumeId })
            });

            const result = await response.json();
            if (!result.success) {
                console.error('Ошибка при лайке:', result.error);
            } else if (result.isMatch) {
                alert('У вас мэтч!');
            }
        } catch (err) {
            console.error('Ошибка сети:', err);
        }
    }

    // Убираем карточку из UI
    console.log("свайп")
    card.classList.add(direction === 'right' ? 'swipe-right' : 'swipe-left');
    setTimeout(() => card.remove(), 500);
}