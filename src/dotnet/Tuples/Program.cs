Console.WriteLine("Don't bother read output that much. Read the code and comments instead. Everything has a meaning.");

var tuple1 = new Tuple<string, int>("Jim", 1);
var tuple2 = new Tuple<string, int>("Jim", 1);

// Tuples are reference types
Console.WriteLine(tuple1 == tuple2); // False
Console.WriteLine(tuple1.Equals(tuple2)); // True. Very weird. Equals is overriden on Tuple<T1,T2> but == is not

// Doesn't compile, tuples are immutable
// tuple1.Item1 = "Bob";

var tuple3 = new ValueTuple<string, int>("Jim", 1);
var tuple4 = new ValueTuple<string, int>("Jim", 1);

// ValueTuples are value types

Console.WriteLine(tuple3 == tuple4); // True
Console.WriteLine(tuple3.Equals(tuple4)); // True

var valueTuplesCanBeCreatedAsThis = ("Jim", 1);
valueTuplesCanBeCreatedAsThis.Item1 = "Bob"; // Weird. Works since value tuples are mutable

var thisIsNice = (Name: "Jim", Age: 1); // Nice way of creating a ValueTuple with explicit property names

Console.WriteLine(tuple3.Equals(valueTuplesCanBeCreatedAsThis)); // True
Console.WriteLine(tuple3.Equals(thisIsNice)); // True
Console.WriteLine(thisIsNice.GetType()); // System.ValueTuple`2[System.String,System.Int32]
Console.WriteLine(thisIsNice.Name); // Explicit property name