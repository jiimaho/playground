// ReSharper disable EqualExpressionComparison
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable PossibleUnintendedReferenceComparison

using Equals;

Console.WriteLine("Don't focus too much on reading the actual output, but instead read the code and the comments");

Console.WriteLine("TLDR; Always override Equals and GetHashCode on structs and classes. " +
                  "Also implement IEquatable<T> on classes for performance reasons. " +
                  "Also implement == operator for less weirdness." +
                  "Also implement != operator for less weirdness." +
                  "Also implement IComparable<T> for sorting and ordering in for example list.Sort");

EqualityDemonstrationStructs();
EqualityDemonstrationMixed();
EqualityDemonstrationClasses();

void EqualityDemonstrationStructs()
{
    Console.WriteLine("Demonstration of equality with struct");

    var point1 = new Point(10, 20);
    var point2 = new Point(10, 20);

    Console.WriteLine(point1.Equals(point2)); // True.
// Console.WriteLine(point1 == point2); Does not compile as strangely structs do not have == operator by default
    Console.WriteLine(1 == 1); // True. Strangely works since int has overriden == operator
}

void EqualityDemonstrationMixed()
{
    Console.WriteLine("Demonstration of mixed equality");

    var point1 = new Point(10, 20);
    
    Console.WriteLine(point1.Equals(Person.Empty)); // False. Calls the overriden method on Point
    Console.WriteLine(Person.Empty.Equals(point1)); // False. Calls the overriden method on Person
}

void EqualityDemonstrationClasses()
{
    Console.WriteLine("Demonstration of equality with classes");

    var people = new List<Person>().Contains(Person.Empty); // False. But still very PERFORMANT since we use the power of IEquatable<Person>
    var people2 = new List<Person>().Contains(new Point(1,1) as object); // False. But still NOT very performant since we must use the Equals method on Person taking an object and must use boxing
    Console.WriteLine(Person.Empty == Person.Empty); // False. A bit tricky since we have not overriden the == operator and therefore it will do a reference comparison and NOT use any of the Equals methods
}