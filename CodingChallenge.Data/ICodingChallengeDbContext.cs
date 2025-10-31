using CodingChallenge.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CodingChallenge.Data
{
    public interface ICodingChallengeDbContext : IDisposable
    {
        /// <summary>
        /// The Transactions DbSet.
        /// </summary>
        DbSet<TransactionDataModel> Transactions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
