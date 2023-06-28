using CoreRS.Logger;
using Microsoft.EntityFrameworkCore;
using ReportingService.Dal.IRepository;
using ReportingService.DAL.ModelsDAL.Commissions;

namespace ReportingService.Dal.Repository
{
    public class CommissionRepository : ICommissionRepository
    {
        private readonly Context _context;
        private readonly ILoggerManager _log;

        public CommissionRepository(Context context, ILoggerManager log)
        {
            _context = context;
            _log = log;
        }

        public async Task<CommissionEntity> AddCommission(CommissionEntity commisNew)
        {
            var result = await _context.Commissions.AddAsync(commisNew);
            await _context.SaveChangesAsync();

            return await _context.Commissions
                .Include(k => k.Transaction)
                .SingleAsync();
        }

        public async Task AddCommissionInTableDay(CommissionPerDayEntity commisNew)
        {
            var result = await _context.SumCommissionsPerDay
                .SingleOrDefaultAsync(k => k.Type == commisNew.Type &&
                    k.OperationYear == commisNew.OperationYear &&
                    k.OperationMonth == commisNew.OperationMonth &&
                    k.OperationDay == commisNew.OperationDay);

            if (result != null)
            {
                result.AmountCommission += commisNew.AmountCommission;
            }

            else
            {
                await _context.SumCommissionsPerDay.AddAsync(commisNew);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddCommissionInTableMonth(CommissionPerMonthEntity commisNew)
        {
            var result = await _context.SumCommissionsPerMonth
                .SingleOrDefaultAsync(k => k.Type == commisNew.Type &&
                    k.OperationYear == commisNew.OperationYear &&
                    k.OperationMonth == commisNew.OperationMonth);

            if (result != null)
            {
                result.AmountCommission += commisNew.AmountCommission;
            }

            else
            {
                await _context.SumCommissionsPerMonth.AddAsync(commisNew);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddCommissionInTableYear(CommissionPerYearEntity commisNew)
        {
            var result = await _context.SumCommissionsPerYear
                .SingleOrDefaultAsync(k => k.Type == commisNew.Type &&
                    k.OperationYear == commisNew.OperationYear);

            if (result != null)
            {
                result.AmountCommission += commisNew.AmountCommission;
            }

            else
            {
                await _context.SumCommissionsPerYear.AddAsync(commisNew);
            }

            await _context.SaveChangesAsync();
        }
    }
}
