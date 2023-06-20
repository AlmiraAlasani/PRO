using Microsoft.EntityFrameworkCore;
using Moq;
using PRO.Data;
using PRO.Models;
using PRO.Repositories.AccountRepository;

namespace ProTesting
{
    public class AccountRepositoryTests
    {
        [Fact]
        public async Task GetAllAccountsAsync_ShouldReturnAllAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { Id = 1, Email = "Account 1" , Balance = 0},
                new Account { Id = 2, Email = "Account 2", Balance = 0},
                new Account { Id = 3, Email = "Account 3", Balance = 0}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Account>>();
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(accounts.Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(accounts.Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(accounts.ElementType);
            mockSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(accounts.GetEnumerator());

            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(db => db.Accounts).Returns(mockSet.Object);

            var accountRepository = new AccountRepository(mockDbContext.Object);

            // Act
            var result = await accountRepository.GetAllAccountsAsync();

            // Assert
            Assert.Equal(accounts.Count(), result.Count());
            Assert.Equal(accounts.ToList(), result.ToList());
        }

        
    }
}