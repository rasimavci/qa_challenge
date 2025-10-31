using CodingChallenge.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CodingChallenge.Data
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public class CodingChallengeDbContext : DbContext, ICodingChallengeDbContext
    {
        public CodingChallengeDbContext(DbContextOptions<CodingChallengeDbContext> options) : base(options) { }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Ensure UserDataModel is included in the model for in-memory tests
            modelBuilder.Entity<UserDataModel>();
        }

        /// <inheritdoc />
        public DbSet<TransactionDataModel> Transactions { get; set; }

        public int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}