using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Jobs;

public class SwipeModel : PageModel
{
    public List<JobVacancy> Vacancies { get; set; } = new();

    public void OnGet()
    {
        // Пример заполнения — замени на реальные данные из БД
        Vacancies = new List<JobVacancy>();

        for (var i = 0; i < 10; i++)
        {
            Vacancies.AddRange([
                new JobVacancy
                {
                    Id = Guid.NewGuid(),
                    Title = "Backend Developer",
                    CompanyName = "TechCorp",
                    Location = "Москва",
                    ExperienceLevel = ExperienceLevel.Mid,
                    Description = "Ищешь возможность прокачать свои навыки Go и работать над сложными, но интересными задачами? Наша компания, успешно реализующая проекты для лидеров российского и международного рынков, приглашает в свою команду начинающего Go-разработчика. Это твой шанс получить ценный опыт и стремительно развиваться в IT! Что мы предлагаем: Полностью удаленная работа с гибким графиком. Увлекательные проекты, способствующие росту твоих компетенций. Конкурентная заработная плата. Поддержка команды и наставничество. Что мы ожидаем от тебя: Понимание ООП, базовых алгоритмов и структур данных. Опыт работы с SQL или NoSQL базами данных. Знание принципов RESTful API (опыт с gRPC будет преимуществом). Общее представление о микросервисной архитектуре и паттернах проектирования. Базовые навыки работы с Git. Английский язык на уровне чтения технической документации. Готов к новым вызовам и развитию в backend? Присоединяйся к нашей команде!",
                    PostedDate = DateTime.UtcNow.AddDays(-2),
                    SkillsRequired = ["c#", "c", "java", "RabbitMQ"]
                },
                new JobVacancy
                {
                    Id = Guid.NewGuid(),
                    Title = "Junior Frontend Developer",
                    CompanyName = "WebStart",
                    Location = "Удалённо",
                    ExperienceLevel = ExperienceLevel.Junior,
                    Description = "auto - This value will look at the width and height of the box. If they are defined, it won't let the box expand past those boundaries. Instead (if the content exceeds those boundaries), it will create a scrollbar for either boundary (or both) that exceeds its length. aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaa aaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaa aaaaaa aaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaa aaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaa aaaaaa aaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaa aaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaa aaaaaa aaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaa aaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaa aaaaaa aaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaa aaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaa aaaaaa aaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaa aaaaaaaaaaaa aaaaaaaaaaaaaa aaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaa aaaaaa aaaaaaaaaaaaaaaaaaa",
                    PostedDate = DateTime.UtcNow.AddDays(-1),
                    SkillsRequired = ["c#", "c", "java", "RabbitMQ"]
                }
            ]);
        }
    }
}