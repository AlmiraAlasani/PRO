﻿using PRO.DTOs;

namespace PRO.Services.ExpenseService
{
    public interface IExpenseService 
    {
        Task<IEnumerable<ExpenseDTO>> GetAllExpensesAsync();
        Task<ExpenseDTO> GetExpenseByIdAsync(int id);
        Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto);
        Task UpdateExpenseAsync(int id, ExpenseDTO expenseDto);
        Task DeleteExpenseAsync(int id);
    }
}
