using AutoMapper;
using Moq;
using PRO.DTOs;
using PRO.Models;
using PRO.Repositories;
using PRO.Repositories.AccountRepository;
using PRO.Services.AccountService;

namespace ProTesting
{
    public class AccountServiceTests
    {
        private List<AccountDTO> _expectedAccountDtOs;

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsListOfAccounts()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var expectedAccounts = new List<Account> { new Account(), new Account() };
     
            var expectedAccountDTOs = _expectedAccountDtOs;

            accountRepositoryMock.Setup(repo => repo.GetAllAccountsAsync())
                .ReturnsAsync(expectedAccounts);

            mapperMock.Setup(mapper => mapper.Map<IEnumerable<AccountDTO>>(expectedAccounts))
                .Returns(expectedAccountDTOs);

            unitOfWorkMock.SetupGet(uow => uow.AccountRepository)
                .Returns(accountRepositoryMock.Object);

            var accountService = new AccountService(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await accountService.GetAllAccountsAsync();

            // Assert
            Assert.Equal(expectedAccountDTOs, result);
        }

        [Fact]
        public async Task CreateAccountAsync_CreatesAccount()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var accountDto = new AccountDTO { Email = "Test Account" ,Balance = 0};

            var expectedAccount = new Account { Email = "Test Account" ,Balance = 0};
            var expectedAccountDto = new AccountDTO { Email = "Test Account" ,Balance = 0};

            accountRepositoryMock.Setup(repo => repo.AddAccountAsync(It.IsAny<Account>()))
                .Callback((Account account) =>
                {
                    account.Id = 1; // Set the id for the created account
                    expectedAccount.Id = 1; // Set the expected id for comparison
                });

            mapperMock.Setup(mapper => mapper.Map<Account>(accountDto))
                .Returns(expectedAccount);

            mapperMock.Setup(mapper => mapper.Map<AccountDTO>(expectedAccount))
                .Returns(expectedAccountDto);

            unitOfWorkMock.SetupGet(uow => uow.AccountRepository)
                .Returns(accountRepositoryMock.Object);

            var accountService = new AccountService(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await accountService.CreateAccountAsync(accountDto);

            // Assert
            Assert.Equal(expectedAccountDto, result);
        }

       
        
    }
}
