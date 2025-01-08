namespace Domain.Models;

public class Severity(string value) : DomainPrimitive<string>(value)
{
   public static Severity Low => new("Low");
   public static Severity Medium => new("Medium");
   public static Severity High => new("High");
}