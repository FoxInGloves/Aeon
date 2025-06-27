let remainingResume = 10;

async function swipeResume(button, direction) {
    const card = button.closest('.job-card');
    const resumeId = card.dataset.resumeId;

    remainingResume--;

    if (remainingResume <= 2) {
        loadMoreResume();
    }

    if (direction === 'right') {
        try {
            const response = await fetch('/api/like/resume', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ ToResumeId: resumeId })
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

let loadingResume = false;
let skip = 10;

async function loadMoreResume(){
    if (loadingResume) return;
    loadingResume = true;

    try {
        const response = await fetch(`/api/project/resume?skip=${skip}&take=10`, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });

        const html = await response.text();
        const container = document.getElementById('cardContainer');
        container.insertAdjacentHTML('beforeend', html);

        const addedCount = (html.match(/class="job-card"/g) || []).length;
        console.log(addedCount);
        skip += addedCount;

        if (addedCount > 0) {
            renderStars();
        }
    } catch (err) {
        console.error("Ошибка загрузки:", err);
    } finally {
        loadingResume = false;
    }
}