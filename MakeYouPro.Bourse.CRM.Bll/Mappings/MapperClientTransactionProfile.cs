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
            CreateMap<TransferTransaction, TransferRequest>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountSource.AccountId))
                .ForMember(dest => dest.TargetAccountId, opt => opt.MapFrom(src => src.AccountDestination.AccountId))
                .ForMember(dest => dest.MoneyType, opt => opt.MapFrom(src => src.AccountSource.Currency))
                .ForMember(dest => dest.TargetMoneyType, opt => opt.MapFrom(src => src.AccountDestination.Currency));
        }
    }
}
