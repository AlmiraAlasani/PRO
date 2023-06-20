using AutoMapper;
using Moq;
using PRO.DTOs;
using PRO.Models;
using PRO.Repositories;
using PRO.Repositories.AccountRepository;
using PRO.Repositories.ExpenseRepository;
using PRO.Services.ExpenseService;

namespace ProTesting
{
    public class ExpenseServiceTests
    {
        [Fact]
        public async Task GetAllExpensesAsync_ReturnsListOfExpenses()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var mapperMock = new Mock<IMapper>();

            var expectedExpenses = new List<Expense> { new Expense(), new Expense() };
            var expectedExpenseDTOs = new List<ExpenseDTO> { new ExpenseDTO(), new ExpenseDTO() };

            expenseRepositoryMock.Setup(repo => repo.GetAllExpensesAsync())
                .ReturnsAsync(expectedExpenses);

            mapperMock.Setup(mapper => mapper.Map<IEnumerable<ExpenseDTO>>(expectedExpenses))
                .Returns(expectedExpenseDTOs);

            unitOfWorkMock.SetupGet(uow => uow.ExpenseRepository)
                .Returns(expenseRepositoryMock.Object);

            var expenseService = new ExpenseService(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await expenseService.GetAllExpensesAsync();

            // Assert
            Assert.Equal(expectedExpenseDTOs, result);
        }

        [Fact]
        public async Task CreateExpenseAsync_CreatesExpenseAndUpdatesAccountBalance()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var expenseDto = new ExpenseDTO { Amount = 100 };

            var account = new Account { Balance = 500 };
            var expectedExpense = new Expense { Amount = 100 };
            var expectedExpenseDto = new ExpenseDTO { Amount = 100 };

            expenseRepositoryMock.Setup(repo => repo.AddExpenseAsync(It.IsAny<Expense>()))
                .Callback((Expense expense) =>
                {
                    expense.Id = 1; // Set the id for the created expense
                    expectedExpense.Id = 1; // Set the expected id for comparison
                });

            accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(1))
                .ReturnsAsync(account);

            mapperMock.Setup(mapper => mapper.Map<Expense>(expenseDto))
                .Returns(expectedExpense);

            mapperMock.Setup(mapper => mapper.Map<ExpenseDTO>(expectedExpense))
                .Returns(expectedExpenseDto);

            unitOfWorkMock.SetupGet(uow => uow.ExpenseRepository)
                .Returns(expenseRepositoryMock.Object);

            unitOfWorkMock.SetupGet(uow => uow.AccountRepository)
                .Returns(accountRepositoryMock.Object);

            var expenseService = new ExpenseService(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await expenseService.CreateExpenseAsync(expenseDto);

            // Assert
            Assert.Equal(expectedExpenseDto, result);
            Assert.Equal(400, account.Balance); // Account balance should be updated to 400
        }

        

    }
}
