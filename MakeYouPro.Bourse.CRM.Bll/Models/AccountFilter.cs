using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Bll.Models
{
    public class AccountFilter
    {
        public DateTime? FromDateCreate { get; set; }

        public DateTime? ToDateCreate { get; set; }

        public decimal? FromBalace { get; set; }

        public decimal? ToBalace { get; set; }

        public List<string>? Currencies { get; set; } = new List<string>();

        public List<AccountStatusEnum>? Statuses { get; set; } = new List<AccountStatusEnum>();

        public List<int>? LeadsId { get; set; } = new List<int>();

        public bool? IsDeleted { get; set; }
    }
}