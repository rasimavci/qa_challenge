using System.ComponentModel.DataAnnotations;
using CodingChallenge.Data.DataModels;

namespace CodingChallenge.Service.UnitTests.DataModels
{
    /// <summary>
    /// A test-only user model that uses BaseDataModel to validate base constraints.
    /// </summary>
    internal class TestUserDataModel : BaseDataModel
    {
        [Required]
        public required string UserName { get; set; }
    }

    public class UserDataModelUnitTests
    {
        [Fact]
        public void Validate_ShouldThrow_WhenCreatedAtMissing()
        {
            var model = new TestUserDataModel
            {
                // CreatedAt not set
                UpdatedAt = DateTime.UtcNow,
                UserName = "TestUser"
            };

            var ctx = new ValidationContext(model);

            Action act = () => Validator.ValidateObject(model, ctx, validateAllProperties: true);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Validate_ShouldThrow_WhenUpdatedAtMissing()
        {
            var model = new TestUserDataModel
            {
                CreatedAt = DateTime.UtcNow,
                // UpdatedAt not set
                UserName = "TestUser"
            };

            var ctx = new ValidationContext(model);

            Action act = () => Validator.ValidateObject(model, ctx, validateAllProperties: true);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Validate_ShouldThrow_WhenUserNameMissing()
        {
            var model = new TestUserDataModel
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                // UserName missing
            };

            var ctx = new ValidationContext(model);

            Action act = () => Validator.ValidateObject(model, ctx, validateAllProperties: true);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllRequiredFieldsProvided()
        {
            var model = new TestUserDataModel
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserName = "TestUser"
            };

            var ctx = new ValidationContext(model);

            Action act = () => Validator.ValidateObject(model, ctx, validateAllProperties: true);

            act.Should().NotThrow();
            model.RowVersion.Should().NotBeNull();
            model.RowVersion.Length.Should().Be(0);
        }

        [Fact]
        public void InMemoryDb_CanAddAndRetrieve_UserDataModel()
        {
            using var helper = new InMemoryCodingChallengeDbContext();

            var user = new CodingChallenge.Data.DataModels.UserDataModel
            {
                UserName = "DbUser"
            };

            helper.Users.Add(user);
            helper.CodingChallengeDbContext.SaveChanges();

            user.Id.Should().BeGreaterThan(0);

            var fetched = helper.Users.Find(user.Id);
            fetched.Should().NotBeNull();
            fetched!.UserName.Should().Be("DbUser");
        }
    }
}


