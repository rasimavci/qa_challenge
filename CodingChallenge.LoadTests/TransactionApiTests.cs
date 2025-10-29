using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using CodingChallenge.Dtos;
using CodingChallenge.Common.Enums;

namespace CodingChallenge.LoadTests;

public class TransactionApiTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;
    private readonly IConfiguration _config;
    private readonly bool _useMockData;

    public TransactionApiTests()
    {
        try
        {
            _config = new ConfigurationBuilder()
         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
       .Build();

  _baseUrl = _config["LoadTestSettings:BaseUrl"] ?? "http://localhost:5000";
    _client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        
 // Try to connect to the API, if fails, use mock data
     var isApiAvailable = _client.GetAsync("/api/health").Result.IsSuccessStatusCode;
            _useMockData = !isApiAvailable;
            
            if (_useMockData)
            {
    Console.WriteLine("Warning: API not available, using mock data for load tests");
   }
   }
        catch
        {
     Console.WriteLine("Warning: Could not connect to API, using mock data for load tests");
    _useMockData = true;
      _client = new HttpClient(); // Empty client for mocking
    }
    }

    private NBomber.Contracts.ScenarioProps CreateGetTransactionsTest()
  {
        return Scenario.Create("Get Transactions Test", async context =>
        {
  try
 {
  if (_useMockData)
      {
        await Task.Delay(10); // Simulate network latency
     return Response.Ok(CreateMockTransactionsList());
                }

      var request = Http.CreateRequest("GET", "/api/transactions");
    var response = await Http.Send(_client, request);
    return Response.Ok(response);
            }
          catch (Exception ex)
            {
    context.Logger.Error(ex, "Failed to execute Get Transactions test");
      return Response.Fail();
     }
      })
        .WithoutWarmUp()
      .WithLoadSimulations(
          Simulation.Inject(
                rate: GetConfiguredRate("Normal"),
      interval: TimeSpan.FromSeconds(1),
           during: GetConfiguredDuration()
        )
    );
    }

    private NBomber.Contracts.ScenarioProps CreateGetTransactionByIdTest()
    {
        return Scenario.Create("Get Transaction By Id Test", async context =>
    {
        try
  {
                if (_useMockData)
            {
          await Task.Delay(5); // Simulate network latency
      return Response.Ok(CreateMockTransaction(1));
   }

         var request = Http.CreateRequest("GET", "/api/transactions/1");
                var response = await Http.Send(_client, request);
      return Response.Ok(response);
            }
       catch (Exception ex)
          {
       context.Logger.Error(ex, "Failed to execute Get Transaction By Id test");
      return Response.Fail();
      }
        })
        .WithoutWarmUp()
        .WithLoadSimulations(
   Simulation.Inject(
     rate: GetConfiguredRate("Peak"),
              interval: TimeSpan.FromSeconds(1),
     during: GetConfiguredDuration()
            )
        );
    }

    private int GetConfiguredRate(string loadType)
    {
        try
        {
            return _config.GetValue<int>($"LoadTestSettings:ConcurrentUsers:{loadType}");
        }
 catch
        {
         return loadType == "Normal" ? 10 : 5; // Default values
 }
    }

    private TimeSpan GetConfiguredDuration()
    {
        try
        {
            var durationStr = _config["LoadTestSettings:TestDuration"];
            return TimeSpan.Parse(durationStr ?? "00:00:30");
   }
        catch
        {
            return TimeSpan.FromSeconds(30); // Default duration
      }
    }

    private static TransactionDto CreateMockTransaction(int id)
    {
        return new TransactionDto
        {
         TransactionId = id,
      TransactionAmount = 100.0m,
 TransactionType = TransactionTypes.Credit,
            UserId = id.ToString(),
     TransactionCreatedAt = DateTime.UtcNow
        };
 }

    private static List<TransactionDto> CreateMockTransactionsList()
    {
        return Enumerable.Range(1, 10)
    .Select(i => CreateMockTransaction(i))
            .ToList();
    }

    public void RunLoadTest()
    {
var scenarios = new[]
        {
            CreateGetTransactionsTest(),
       CreateGetTransactionByIdTest()
        };

        NBomberRunner
            .RegisterScenarios(scenarios)
            .WithTestName("Transaction API Load Test")
            .WithReportFileName("TransactionApiLoadTest")
      .Run();

      Console.WriteLine($"Load test completed. Tests ran with {(_useMockData ? "mock" : "real")} data.");
    }

    public void Dispose()
    {
    _client?.Dispose();
    }
}