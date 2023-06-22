using AutoMapper;
using CoreRS.dto;
using CoreRS.Models;
using ReportingService.Bll.Models.Commission;
using ReportingService.Dal.Models.CRM;
using ReportingService.DAL.ModelsDAL.Commissions;

namespace ReportingService.Bll
{
    public class MapperBLL : Profile
    {
        public MapperBLL()
        {
            CreateMap<CommissionDto, CommissionInput>();
            CreateMap<CommissionInput, CommissionEntity>();
            CreateMap<CommissionPerDayInput, CommissionPerDayEntity>();
            CreateMap<CommissionPerMonthInput, CommissionPerMonthEntity>();
            CreateMap<CommissionPerYearInput, CommissionPerYearEntity>();
            CreateMap<CommissionInput, CommissionPerDayInput>()
                .ForMember(src => src.OperationDay, opt => opt.MapFrom(x => x.OperationDay))
                .ForMember(src => src.OperationMonth, opt => opt.MapFrom(x => x.OperationMonth))
                .ForMember(src => src.OperationYear, opt => opt.MapFrom(x => x.OperationYear))
                .ForMember(src => src.Type, opt => opt.MapFrom(x => x.Type))
                .ForMember(src => src.AmountCommission, opt => opt.MapFrom(x => x.Amount));
            CreateMap<CommissionInput, CommissionPerMonthInput>()
                .ForMember(src => src.OperationMonth, opt => opt.MapFrom(x => x.OperationMonth))
                .ForMember(src => src.OperationYear, opt => opt.MapFrom(x => x.OperationYear))
                .ForMember(src => src.Type, opt => opt.MapFrom(x => x.Type))
                .ForMember(src => src.AmountCommission, opt => opt.MapFrom(x => x.Amount));
            CreateMap<CommissionInput, CommissionPerYearInput>()
                .ForMember(src => src.OperationYear, opt => opt.MapFrom(x => x.OperationYear))
                .ForMember(src => src.Type, opt => opt.MapFrom(x => x.Type))
                .ForMember(src => src.AmountCommission, opt => opt.MapFrom(x => x.Amount));
            CreateMap<LeadDto, LeadEntity>()
                .ForMember(src => src.BirthdayDay, opt => opt.MapFrom(x => x.Birthday.Day))
                .ForMember(src => src.BirthdayMonth, opt => opt.MapFrom(x => x.Birthday.Month))
                .ForMember(src => src.BirthdayDay, opt => opt.MapFrom(x => x.Birthday.Year));
        }
    }
}
