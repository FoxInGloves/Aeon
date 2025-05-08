namespace Aeon_Web.Models.Entities.Resume;

public class Resume
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;

    public List<Experience> WorkExperiences { get; set; } = new();
    //public List<Education> Educations { get; set; } = new();
    public List<string> Skills { get; set; } = new();
    public ContactInfo Contact { get; set; } = new();
}

public class Experience
{
    public string CompanyName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // null — по настоящее время
    public string Description { get; set; } = string.Empty;
}

/*public class Education
{
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}*/
