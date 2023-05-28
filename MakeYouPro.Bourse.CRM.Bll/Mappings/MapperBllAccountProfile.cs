using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Bll.Mappings
{
    public class MapperBllAccountProfile : Profile
    {
        public MapperBllAccountProfile() 
        {
            CreateMap<Account, AccountEntity>().ReverseMap();
        }
    }
}
