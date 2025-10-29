using CodingChallenge.Common.Constants;
using CodingChallenge.Data;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace CodingChallenge.Service.UnitTests
{
    [TestCaseOrderer("CodingChallenge.Service.UnitTests.PriorityOrderer", "CodingChallenge.Service.UnitTests")]
    public class TransactionSummaryServiceUnitTests : IClassFixture<InMemoryCodingChallengeDbContext>
    {
        private readonly TransactionSummaryService _transactionSummaryServiceUnderTest;

        private readonly ICodingChallengeDbContext _codingChallengeDbContext;

        public TransactionSummaryServiceUnitTests(InMemoryCodingChallengeDbContext inMemoryCodingChallengeDbContext)
        {
            _codingChallengeDbContext = inMemoryCodingChallengeDbContext.CodingChallengeDbContext;

            _transactionSummaryServiceUnderTest = new TransactionSummaryService(
                _codingChallengeDbContext);
        }

        [Fact, TestPriority(51)]
        public async Task CallingGetTransactionsByTransactionTypeMethod_ShouldReturnExtendData()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionByTransactionTypeDto> transactionByTransactionTypeDtos = await _transactionSummaryServiceUnderTest.GetTransactionsByTransactionType(
                tokenSource.Token);

            // Assert
            transactionByTransactionTypeDtos.Should().NotBeEmpty();
        }

        [Fact, TestPriority(52)]
        public async Task CallingGetTransactionsByUserMethod_ShouldReturnExtendData()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionByUserDto> transactionByUserDtos = await _transactionSummaryServiceUnderTest.GetTransactionsByUser(
                tokenSource.Token);

            // Assert
            transactionByUserDtos.Should().NotBeEmpty();
        }


        //Data Grouping & Aggregation Tests

        [Fact, TestPriority(53)]
        public async Task CallingGetTransactionsByTransactionTypeMethod_ShouldReturnCorrectGroupedData_WhenTransactionDataIsAvailable()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionByTransactionTypeDto> transactionByTransactionTypeDtos = await _transactionSummaryServiceUnderTest.GetTransactionsByTransactionType(
                tokenSource.Token);

            // Assert Deb't ^ cred'tGroups and their total transaction amounts
            using (new AssertionScope())
            {
                transactionByTransactionTypeDtos.Should().NotBeEmpty();
                transactionByTransactionTypeDtos.Should().HaveCount(2); // Should have both Debit and Credit
                
                var debitGroup = transactionByTransactionTypeDtos.FirstOrDefault(x => x.TransactionType == CodingChallenge.Common.Enums.TransactionTypes.Debit);
                var creditGroup = transactionByTransactionTypeDtos.FirstOrDefault(x => x.TransactionType == CodingChallenge.Common.Enums.TransactionTypes.Credit);
                
                debitGroup.Should().NotBeNull();
                creditGroup.Should().NotBeNull();
                debitGroup!.TotalTransactionAmount.Should().BeGreaterThan(0);
                creditGroup!.TotalTransactionAmount.Should().BeGreaterThan(0);
            }
        }

        [Fact, TestPriority(54)]
        public async Task CallingGetTransactionsByUserMethod_ShouldReturnCorrectGroupedData_WhenTransactionDataIsAvailable()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionByUserDto> transactionByUserDtos = await _transactionSummaryServiceUnderTest.GetTransactionsByUser(
                tokenSource.Token);

            // Assert userId and TotalTransactionAmount
            using (new AssertionScope())
            {
                transactionByUserDtos.Should().NotBeEmpty();
                transactionByUserDtos.Should().HaveCount(5); // Should have 5 users (TestUser1 to TestUser5)
                
                foreach (var userGroup in transactionByUserDtos)
                {
                    userGroup.UserId.Should().StartWith("TestUser");
                    userGroup.TotalTransactionAmount.Should().BeGreaterThan(0);
                }
            }
        }





        private async Task SeedDataForTests()
        {
            if (!_codingChallengeDbContext.Transactions.Any())
            {
                IList<TransactionDataModel> transactionDataModels = TransactionDataModelHelper.CreateTransactionDataModels(5);
                await _codingChallengeDbContext.Transactions.AddRangeAsync(transactionDataModels);
                await _codingChallengeDbContext.SaveChangesAsync();
            }
        }



    }
}
