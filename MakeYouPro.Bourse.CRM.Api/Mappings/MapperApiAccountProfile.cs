﻿using AutoMapper;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Models.Account.Response;

namespace MakeYouPro.Bourse.CRM.Api.Mappings
{
    public class MapperApiAccountProfile : Profile
    {
        public MapperApiAccountProfile()
        {
            CreateMap<AccountRequest, Account>();
            CreateMap<AccountCreateRequest, Account>();
            CreateMap<Account, AccountResponse>();
            CreateMap<AccountFilterRequest, AccountFilter>().ReverseMap();
            CreateMap<AccountUpdateRequest, Account>();
        }
    }
}
