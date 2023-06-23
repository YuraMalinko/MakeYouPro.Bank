using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Bll.Mappings
{
    public class MapperClientTransactionProfile : Profile
    {
        public MapperClientTransactionProfile()
        {
            CreateMap<Transaction, WithdrawDtoRequest>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => -src.Amount));
        }
    }
}
