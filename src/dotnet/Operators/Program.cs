

// ReSharper disable MemberCanBePrivate.Global

using Operators;

Console.WriteLine("Some piece of code to demonstrate a bit how operators can be overloaded and used." +
                  "Please read the code and comments instead of the output as the output is not really" +
                  "very good atm.");


DemonstrateMathOperators();
DemonstrateImplicitExplicitOperators();

void DemonstrateMathOperators()
{
    Person veryStrangePerson = new Person("1", "Jim") + new Point(1, 1);
    
    Console.WriteLine(veryStrangePerson);
}

void DemonstrateImplicitExplicitOperators()
{
    Person implicitSuperWeirdCast = new Point(1, 2); 
    Person explicitSuperWeirdCast = (Person) new HttpClient
    {
        BaseAddress = new Uri("https://blog.steadycoding.com")
    };

    Console.WriteLine(implicitSuperWeirdCast);
}