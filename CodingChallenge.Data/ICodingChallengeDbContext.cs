using CodingChallenge.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Data
{
    public interface ICodingChallengeDbContext : IDisposable
    {
        /// <summary>
        /// The Transactions DbSet.
        /// </summary>
        DbSet<TransactionDataModel> Transactions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
