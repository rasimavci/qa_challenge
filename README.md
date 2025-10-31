\# qa\_challenge

QA challenge with for .NET project with C#

This C# project is a Transaction Management Web API with a layered architecture with Entity Framework and AutoMapper.

\## Layers

WebApi
Service
Data layers

\## Added new unit tests Transaction service



Pagination \& Data Retrieval Tests:

Test 12: GetTransactions with specific page number and page size

Test 13: GetTransactions when page number exceeds available data (returns empty)

Test 14: GetHighVolumeTransactions with threshold filtering verification

Test 15: GetHighVolumeTransactions with extremely high threshold (returns empty)



added



Error Handling \& Edge Cases:

Test 16: AddTransaction exception handling when factory throws errors



Test 17: GetTransactionById with zero transaction ID (returns null)



Test 18: GetTransactionById with negative transaction ID (returns null)



Test 19: UpdateTransaction exception handling when factory throws errors



Test 20: DeleteTransaction with zero transaction ID (returns false)



Test 21: DeleteTransaction with negative transaction ID (returns false)





\## Added new unit tests for Transaction summary service



Data Grouping \& Aggregation Tests:

Test 53: GetTransactionsByTransactionType correct grouping (Debit/Credit card groups)



Test 54: GetTransactionsByUser correct grouping (5 users: TestUser1-5)

\## Added Load Tests

Added 2 load test with automatic fallback to mock data if API is unavailable
Simulated network latency in mock mode (10ms and 5ms respectively)

Validate Response times

 

"

Rate: 10 requests/second (Normal load)

Duration: 30 seconds

No warmup



Name: "Get Transaction By Id Test"

Rate: 5 requests/second (Peak load)

Duration: 30 seconds

No warmup



## Added User data base model
UserDataModel


User Data model 
```csharp
public class UserDataModel : BaseDataModel

{

&nbsp;   public required int Id { get; set; }

&nbsp;   public required DateTime CreatedAt { get; set; }

&nbsp;   public string? DateTime { get; set; }

}
```

### Testing Complex Logic Operations

#### Total Amount Per User
- covered in TransactionSummaryServiceUnitTests with tests for GetTransactionsByUser

#### Total Amount Per Transaction Type
- covered in TransactionSummaryServiceUnitTests with tests for GetTransactionsByTransactionType

### Testing Transaction

#### High-Volume Transactions
- covered in TransactionServiceUnitTests with tests for GetHighVolumeTransactions

#### Total transaction amount per user is correct
- covered in GetTransactionsByUser_ShouldReturnCorrectSumsPerUser (verifies explicit sum for each user)

#### Total transaction amount per transaction type is correct
- covered in GetTransactionsByTransactionType_ShouldReturnCorrectSumsPerType (verifies explicit sum for each type)

####  Add Transaction:
- covered in CallingAddTransactionMethod_ShouldTransactionIdOfNewelyAddedTransaction (verifies insertion and returned ID)

####  Fetch Transactions:
- covered in CallingGetTransactionsMethod_ShouldReturnMappedCollection_WhenTransactionDataIsAvailable (verifies retrieval)
- covered in CallingGetTransactionsMethod_ShouldReturnEmptyCollection_WhenNoTransactionDataAvailable (verifies empty result)






