using ReportingService.Bll.Models.CRM;

namespace ReportingService.Bll.IServices
{
    public interface IAccountServices
    {
        Task<List<Account>> GetAccountsByAmountOfTransactionsForPeriodAsync(int numberDays, int numberOfTransactions);
        Task<List<Account>> GetAccountsByBirthdayLeadsAsync(int numberDays);
    }
}