using PRO.DTOs;

namespace PRO.Services.IncomeService
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeDTO>> GetAllIncomesAsync();
        Task<IncomeDTO> GetIncomeByIdAsync(int id);
        
        Task<IncomeDTO> CreateIncomeAsync(IncomeDTO incomeDto);
        Task UpdateIncomeAsync(int id, IncomeDTO incomeDto);
        Task DeleteIncomeAsync(int id);
    }
}
