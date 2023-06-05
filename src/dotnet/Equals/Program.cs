// ReSharper disable EqualExpressionComparison
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable PossibleUnintendedReferenceComparison

Console.WriteLine("Don't focus too much on reading the actual output, but instead read the code and the comments");

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
    
    Console.WriteLine(point1.Equals(new Person())); // False. Calls the overriden method on Point
    Console.WriteLine(new Person().Equals(point1)); // False. Calls the overriden method on Person
}

void EqualityDemonstrationClasses()
{
    Console.WriteLine("Demonstration of equality with classes");

    var people = new List<Person>().Contains(new Person()); // False. But still very PERFORMANT since we use the power of IEquatable<Person>
    var people2 = new List<Person>().Contains(new Point(1,1) as object); // False. But still NOT very performant since we must use the Equals method on Person taking an object and must use boxing
    Console.WriteLine(new Person() == new Person()); // False. A bit tricky since we have not overriden the == operator and therefore it will do a reference comparison and NOT use any of the Equals methods
}