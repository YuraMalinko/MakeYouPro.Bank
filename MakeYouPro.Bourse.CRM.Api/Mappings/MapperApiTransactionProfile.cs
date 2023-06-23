using AutoMapper;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Response;
using MakeYouPro.Bourse.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Api.Mappings
{
    public class MapperApiTransactionProfile : Profile
    {
        public MapperApiTransactionProfile()
        {
            CreateMap<TransactionRequest, Transaction>();
            CreateMap<Transaction, TransactionResponse>();
        }
    }
}
