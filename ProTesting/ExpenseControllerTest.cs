using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PRO.Controllers;
using PRO.DTOs;
using PRO.Models;
using PRO.Services.ExpenseService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProTesting
{
    public class ExpenseControllerTests
    {
        private readonly Mock<IExpenseService> _expenseServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ExpenseController _controller;

        public ExpenseControllerTests()
        {
            _expenseServiceMock = new Mock<IExpenseService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ExpenseController(_expenseServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetExpenseByIdAsync_ExistingId_ReturnsOkResultWithExpenseDTO()
        {
            // Arrange
            var expenseId = 1;
            var expense = new Expense { Id = expenseId, Description = "Expense 1" };
            var expenseDTO = new ExpenseDTO { Id = expenseId, Description = "Expense 1" };

            _expenseServiceMock.Setup(s => s.GetExpenseByIdAsync(expenseId)).ReturnsAsync(expense);
            _mapperMock.Setup(m => m.Map<ExpenseDTO>(expense)).Returns(expenseDTO);

            // Act
            var result = await _controller.GetExpenseByIdAsync(expenseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedExpenseDTO = Assert.IsType<ExpenseDTO>(okResult.Value);

            Assert.Equal(expenseDTO.Id, returnedExpenseDTO.Id);
            Assert.Equal(expenseDTO.Description, returnedExpenseDTO.Description);
        }
    }
}