namespace Collections;

public static class ExplainTheYieldKeyWord
{
    public class MyLogger
    {
        private readonly bool _enableLogging;

        public MyLogger(bool enableLogging)
        {
            _enableLogging = enableLogging;
        }
        
        public void WriteLine(string message)
        {
            if (_enableLogging)
            {
                Console.WriteLine(message);
            }
        }
    }
    
    public static YieldResult Run(int iterations, bool enableLogging = false)
    {
        var log = new MyLogger(enableLogging);
        log.WriteLine("The yield keyword is something you can use inside a method to essentially" +
                      "let the compiler transform the entire method body into returning an iterator." +
                      "It can be extremely useful when looping over a very large set of values. Consider" +
                      "the example below where only a portion of the loop is executed because of Skip and Take," +
                      "compared to forcing execution of the entire loop. Genious!");
        log.WriteLine("");

        var nrOfGenerateNumbersIterator = 0;
        var numbers = GenerateNumbersIterator();
        var numbersFinal = numbers.Skip(5).Take(10); // numbers is an iterator, and NOT a list of numbers (!)

        var nrOfGenerateNumbers = 0;
        var numbers2 = GenerateNumbers();
        var numbers2Final = numbers2.Skip(5).Take(10); // numbers is a list of numbers (!)

        var result = new YieldResult
        {
            First = new YieldSingleResult
            {
                TypeAfterMethod = numbers.GetType(),
                TypeAfterTake = numbersFinal.GetType(),
                Numbers = numbersFinal.ToList(),
                NumberOfExecutions = nrOfGenerateNumbersIterator
            },
            Second = new YieldSingleResult
            {
                TypeAfterMethod = numbers2.GetType(),
                TypeAfterTake = numbers2Final.GetType(),
                Numbers = numbers2Final.ToList(),
                NumberOfExecutions = nrOfGenerateNumbers
            }
        };

        return result;

        // Whenever someone calls this method, execution is deferred until it is ITERATED over
        IEnumerable<int> GenerateNumbersIterator()
        {
            for (int i = 0; i < iterations; i++)
            {
                ++nrOfGenerateNumbersIterator;
                yield return i;
            }
        }    

        // Whenever someone calls this method, execution of the method body is done directly
        IEnumerable<int> GenerateNumbers()
        {
            var result = new HashSet<int>();
            
            for (var i = 0; i < iterations; i++)
            {
                ++nrOfGenerateNumbers;
                result.Add(i);
            }

            return result;
        }
    }

    public record YieldResult
    {
        public YieldSingleResult First { get; set; }
        public YieldSingleResult Second { get; set; }
    }

    public record YieldSingleResult
    {
        public Type TypeAfterMethod { get; set; }
        public Type TypeAfterTake { get; set; }
        public IEnumerable<int> Numbers { get; set; }
        public int NumberOfExecutions { get; set; }
    }
}