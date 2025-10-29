namespace CodingChallenge.LoadTests;

public class Program
{
    public static void Main(string[] args)
  {
        try
        {
            Console.WriteLine("Starting Load Tests...");
     
    using var tests = new TransactionApiTests();
        tests.RunLoadTest();
            
            Console.WriteLine("Load Tests Complete!");
      }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running load tests: {ex.Message}");
Environment.Exit(1);
        }
    }
}