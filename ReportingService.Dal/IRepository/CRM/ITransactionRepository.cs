using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Dal.IRepository.CRM
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> CreateTransactionAsync(TransactionEntity transaction);

        Task<TransactionEntity> UpdateTransactionAsync(TransactionEntity transaction);
    }
}
