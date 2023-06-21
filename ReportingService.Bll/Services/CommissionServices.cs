using AutoMapper;
using CoreRS.dto;
using CoreRS.Logger;
using ReportingService.Bll.Models.Commission;
using ReportingService.Dal.IRepository;
using ReportingService.DAL.ModelsDAL.Commissions;

namespace ReportingService.Bll.Services
{
    public class CommissionServices
    {
        private readonly IMapper _map;
        private readonly ILoggerManager _log;
        private readonly ICommissionRepository _commissionRepository;
        private readonly ITransactionRepository _transactionRepository;

        public CommissionServices(IMapper map, ILoggerManager log, ICommissionRepository commis, 
            ITransactionRepository trans)
        {
            _map = map;
            _log = log;
            _commissionRepository = commis;
            _transactionRepository = trans;
        }

        public async Task AddCommission(CommissionDto commisNew)
        {
            CommissionInput commisNewBll = new CommissionInput();

            var searchTrans = await _transactionRepository.GetTransactionByIdOutsideAsync(commisNew.TransactionId);

            if(commisNew.AmountCommission > 0)
            {
                commisNewBll.Type = searchTrans.Type;
                commisNewBll.Amount = commisNewBll.Amount;
                commisNewBll.TransactionId = searchTrans.IdOutside;
                commisNewBll.OperationDay = searchTrans.DataTime.Day;
                commisNewBll.OperationMonth = searchTrans.DataTime.Month;
                commisNewBll.OperationYear = searchTrans.DataTime.Year;
                commisNewBll.OperationTime = TimeOnly.FromDateTime(searchTrans.DataTime);
            }
            else
            {
                throw new ArgumentException("Passed a value less than zero");
            }

            await AddCommissionInTableDay(_map.Map<CommissionPerDayInput>(commisNew));
            await AddCommissionInTableMonth(_map.Map<CommissionPerMonthInput>(commisNew));
            await AddCommissionInTableYear(_map.Map<CommissionPerYearInput>(commisNew));
        }

        public async Task AddCommissionInTableDay(CommissionPerDayInput commisNew)
        {
            await _commissionRepository.AddCommissionInTableDay(_map.Map<CommissionPerDayEntity>(commisNew));
        }

        public async Task AddCommissionInTableMonth(CommissionPerMonthInput commisNew)
        {
            await _commissionRepository.AddCommissionInTableMonth(_map.Map<CommissionPerMonthEntity>(commisNew));
        }
        
        public async Task AddCommissionInTableYear(CommissionPerYearInput commisNew)
        {
            await _commissionRepository.AddCommissionInTableYear(_map.Map<CommissionPerYearEntity>(commisNew));
        }
    }
}
