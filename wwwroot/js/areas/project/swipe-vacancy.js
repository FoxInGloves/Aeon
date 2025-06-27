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

            if (response.status === 401) {
                window.location.href = '/Identity/Account/Login';
            }
            
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

let loadingVacancy = false;
let skip = 10;

async function loadMoreVacancies() {
    if (loadingVacancy) return;
    loadingVacancy = true;

    try {
        const response = await fetch(`/api/project/vacancy?skip=${skip}&take=10`, {
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
            renderStars();
        }
    } catch (err) {
        console.error("Ошибка загрузки:", err);
    } finally {
        loadingVacancy = false;
    }
}
