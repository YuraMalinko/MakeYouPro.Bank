using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models;

namespace MakeYouPro.Bourse.CRM.Bll.Mappings
{
    public class MapperClientTransactionProfile : Profile
    {
        public MapperClientTransactionProfile()
        {
            CreateMap<Transaction, WithdrawRequest>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => -src.Amount));
            CreateMap<Transaction, DepositRequest>();
        }
    }
}
