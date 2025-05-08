using Aeon_Web.Models.Entities.Abstractions;

namespace Aeon_Web.Models.Entities;

public class ApplicationUserId : TypedIdValue
{
    public ApplicationUserId(Guid value) : base(value)
    {
    }
}