namespace Aeon_Web.Models.Entities;

public class Vacancy
{
    public Guid Id { get; }
    
    public string Name { get; private set; }
    
    public string? Description { get; private set; }
    
    public string? ShortDescription { get; private set; }
    
    public string WhoNeeded { get; private set; }

    private Vacancy(
        string name,
        string? description,
        string? shortDescription,
        string whoNeeded)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ShortDescription = shortDescription;
        WhoNeeded = whoNeeded;
    }

    public static Vacancy CreateVacancy(
        string name,
        string? description,
        string? shortDescription,
        string whoNeeded)
    {
        return new Vacancy(
            name,
            description,
            shortDescription,
            whoNeeded);
    }
}