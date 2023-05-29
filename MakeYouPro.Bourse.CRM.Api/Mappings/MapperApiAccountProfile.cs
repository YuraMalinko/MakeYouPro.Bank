using AutoMapper;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bank.CRM.Models.Account.Request;
using MakeYouPro.Bank.CRM.Models.Account.Response;

namespace MakeYouPro.Bourse.CRM.Api.Mappings
{
    public class MapperApiAccountProfile : Profile
    {
        public MapperApiAccountProfile()
        {
            CreateMap<AccountRequest, Account>();
            CreateMap<Account, AccountResponse>();
        }
    }
}
