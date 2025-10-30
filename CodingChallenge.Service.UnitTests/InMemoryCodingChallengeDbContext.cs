using CodingChallenge.Data;
using CodingChallenge.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Service.UnitTests
{
    public class InMemoryCodingChallengeDbContext : IDisposable
    {
        private readonly ICodingChallengeDbContext _codingChallengeDbContex;

        public InMemoryCodingChallengeDbContext()
        {
            var builder = new DbContextOptionsBuilder<CodingChallengeDbContext>();
            builder.UseInMemoryDatabase("CodingChallenge");
            var options = builder.Options;

            _codingChallengeDbContex = new CodingChallengeDbContext(options);
        }

        public ICodingChallengeDbContext CodingChallengeDbContext { get { return _codingChallengeDbContex; } }

        /// <summary>
        /// Gets the Users DbSet for testing purposes
        /// </summary>
        public DbSet<UserDataModel> Users => ((CodingChallengeDbContext)_codingChallengeDbContex).Set<UserDataModel>();

        public void Dispose()
        {
            _codingChallengeDbContex.Dispose();
        }
    }
}

namespace CodingChallenge.Data.DataModels
{
    public class UserDataModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        // Add other properties as needed
    }
}
