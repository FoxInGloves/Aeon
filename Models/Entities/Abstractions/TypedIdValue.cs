namespace Aeon_Web.Models.Entities.Abstractions;

public abstract class TypedIdValue : IEquatable<TypedIdValue>
{
    public Guid Value { get; }

    public TypedIdValue(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Value cannot be empty.", nameof(value));

        Value = value;
    }
    
    public bool Equals(TypedIdValue? other)
    {
        return Value == other?.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((TypedIdValue)obj);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public static bool operator ==(TypedIdValue? left, TypedIdValue? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TypedIdValue? left, TypedIdValue? right)
    {
        return !Equals(left, right);
    }
}