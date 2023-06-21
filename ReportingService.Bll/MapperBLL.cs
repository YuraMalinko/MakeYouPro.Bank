using AutoMapper;
using ReportingService.Bll.Models.Commission;
using ReportingService.DAL.ModelsDAL.Commissions;

namespace ReportingService.Bll
{
    public class MapperBLL : Profile
    {
        public MapperBLL()
        {
            CreateMap<CommissionInput, CommissionEntity>();
        }
    }
}
