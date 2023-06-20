using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PRO.Controllers;
using PRO.DTOs;
using PRO.Models;
using PRO.Services.IncomeService;

namespace ProTesting
{
    public class IncomeControllerTests
    {
        private readonly IncomeController _controller;
        private readonly Mock<IIncomeService> _incomeServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        public IncomeControllerTests()
        {
            _incomeServiceMock = new Mock<IIncomeService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new IncomeController(_incomeServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllIncomesAsync_ReturnsOkResult()
        {
            // Arrange
            var incomes = new List<Income>
            {
                new Income { Id = 1, Description = "Income 1", Amount = 100 },
                new Income { Id = 2, Description = "Income 2", Amount = 200 }
            };

            var incomeDTOs = new List<IncomeDTO>
            {
                new IncomeDTO { Id = 1, Description = "Income 1", Amount = 100 },
                new IncomeDTO { Id = 2, Description = "Income 2", Amount = 200 }
            };

            _incomeServiceMock.Setup(x => x.GetAllIncomesAsync()).ReturnsAsync(incomes);
            _mapperMock.Setup(x => x.Map<IEnumerable<IncomeDTO>>(incomes)).Returns(incomeDTOs);

            // Act
            var result = await _controller.GetAllIncomesAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedIncomeDTOs = Assert.IsAssignableFrom<IEnumerable<IncomeDTO>>(okResult.Value);
            Assert.Equal(incomeDTOs.Count, returnedIncomeDTOs.Count());
        }

     
    }
}
