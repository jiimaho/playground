// ReSharper disable MemberCanBePrivate.Global
namespace Equals;

// Instead of
public class Home : IEquatable<Home>
{
    public string StreetAddress { get; }
    public string ZipCode { get; }
    public string CountryCode { get; }
    
    public Home(string streetAddress, string zipCode, string countryCode)
    {
        StreetAddress = streetAddress;
        ZipCode = zipCode;
        CountryCode = countryCode;
    }

    public bool Equals(Home? other) =>
        other != null &&
        StreetAddress == other.StreetAddress &&
        ZipCode == other.ZipCode &&
        CountryCode == other.CountryCode;

    public override bool Equals(object? obj) => obj is Home other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(StreetAddress, ZipCode, CountryCode);

    public static bool operator ==(Home? left, Home? right) => Equals(left, right);

    public static bool operator !=(Home? left, Home? right) => !Equals(left, right);
}

// We can do this, called a positional record
// Gives us a value-based equality
// Compiler generates a constructor, Equals(object), Equals(HomeRecord), GetHashCode, ToString, == and != for us
// Without using reflection
public record HomeRecord(string StreetAddress, string ZipCode, string CountryCode);

// So why not just use a struct like this?
public readonly struct HomeStruct
{
    public string StreetAddress { get; }
    public string ZipCode { get; }
    public string CountryCode { get; }
}