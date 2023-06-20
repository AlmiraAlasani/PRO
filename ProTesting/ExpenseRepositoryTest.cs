using Microsoft.EntityFrameworkCore;
using Moq;
using PRO.Data;
using PRO.Models;
using PRO.Repositories.ExpenseRepository;

namespace ProTesting
{
    public class ExpenseRepositoryTests
    {
        [Fact]
        public async Task GetAllExpensesAsync_ShouldReturnAllExpenses()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense { Id = 1, Amount = 10 },
                new Expense { Id = 2, Amount = 20 },
                new Expense { Id = 3, Amount = 30 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Expense>>();
            mockSet.As<IQueryable<Expense>>().Setup(m => m.Provider).Returns(expenses.Provider);
            mockSet.As<IQueryable<Expense>>().Setup(m => m.Expression).Returns(expenses.Expression);
            mockSet.As<IQueryable<Expense>>().Setup(m => m.ElementType).Returns(expenses.ElementType);
            mockSet.As<IQueryable<Expense>>().Setup(m => m.GetEnumerator()).Returns(expenses.GetEnumerator());

            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(db => db.Expenses).Returns(mockSet.Object);

            var expenseRepository = new ExpenseRepository(mockDbContext.Object);

            // Act
            var result = await expenseRepository.GetAllExpensesAsync();

            // Assert
            Assert.Equal(expenses.Count(), result.Count());
            Assert.Equal(expenses.ToList(), result.ToList());
        }

        
    }
}