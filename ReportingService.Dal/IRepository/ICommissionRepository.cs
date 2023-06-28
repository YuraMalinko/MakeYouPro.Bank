using ReportingService.DAL.ModelsDAL.Commissions;

namespace ReportingService.Dal.IRepository
{
    public interface ICommissionRepository
    {
        Task<CommissionEntity> AddCommission(CommissionEntity commisNew);
        Task AddCommissionInTableDay(CommissionPerDayEntity commisNew);
        Task AddCommissionInTableMonth(CommissionPerMonthEntity commisNew);
        Task AddCommissionInTableYear(CommissionPerYearEntity commisNew);
    }
}