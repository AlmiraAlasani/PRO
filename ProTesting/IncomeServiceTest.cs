using AutoMapper;
using Moq;
using PRO.DTOs;
using PRO.Models;
using PRO.Repositories;
using PRO.Repositories.AccountRepository;
using PRO.Repositories.IncomeRepository;
using PRO.Services.IncomeService;

namespace ProTesting
{
    public class IncomeServiceTests
    {
        [Fact]
        public async Task GetAllIncomesAsync_ReturnsListOfIncomes()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var incomeRepositoryMock = new Mock<IIncomeRepository>();
            var mapperMock = new Mock<IMapper>();

            var expectedIncomes = new List<Income> { new Income(), new Income() };
            var expectedIncomeDTOs = new List<IncomeDTO> { new IncomeDTO(), new IncomeDTO() };

            incomeRepositoryMock.Setup(repo => repo.GetAllIncomesAsync())
                .ReturnsAsync(expectedIncomes);

            mapperMock.Setup(mapper => mapper.Map<IEnumerable<IncomeDTO>>(expectedIncomes))
                .Returns(expectedIncomeDTOs);

            unitOfWorkMock.SetupGet(uow => uow.IncomeRepository)
                .Returns(incomeRepositoryMock.Object);

            var incomeService = new IncomeService(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await incomeService.GetAllIncomesAsync();

            // Assert
            Assert.Equal(expectedIncomeDTOs, result);
        }

        [Fact]
        public async Task CreateIncomeAsync_CreatesIncomeAndUpdatesAccountBalance()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var incomeRepositoryMock = new Mock<IIncomeRepository>();
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var incomeDto = new IncomeDTO { Amount = 100 };

            var account = new Account { Balance = 500 };
            var expectedIncome = new Income { Amount = 100 };
            var expectedIncomeDto = new IncomeDTO { Amount = 100 };

            incomeRepositoryMock.Setup(repo => repo.AddIncomeAsync(It.IsAny<Income>()))
                .Callback((Income income) =>
                {
                    income.Id = 1; // Set the id for the created income
                    expectedIncome.Id = 1; // Set the expected id for comparison
                });

            accountRepositoryMock.Setup(repo => repo.GetAccountByIdAsync(1))
                .ReturnsAsync(account);

            mapperMock.Setup(mapper => mapper.Map<Income>(incomeDto))
                .Returns(expectedIncome);

            mapperMock.Setup(mapper => mapper.Map<IncomeDTO>(expectedIncome))
                .Returns(expectedIncomeDto);

            unitOfWorkMock.SetupGet(uow => uow.IncomeRepository)
                .Returns(incomeRepositoryMock.Object);

            unitOfWorkMock.SetupGet(uow => uow.AccountRepository)
                .Returns(accountRepositoryMock.Object);

            var incomeService = new IncomeService(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await incomeService.CreateIncomeAsync(incomeDto);

            // Assert
            Assert.Equal(expectedIncomeDto, result);
            Assert.Equal(600, account.Balance); // Account balance should be updated to 600
        }

    }
}
