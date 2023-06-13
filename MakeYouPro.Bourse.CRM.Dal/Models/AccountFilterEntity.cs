using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Dal.Models
{
    public class AccountFilterEntity
    {
        public DateTime? FromDateCreate { get; set; }

        public DateTime? ToDateCreate { get; set; }

        public List<string>? Currencies { get; set; } = new List<string>();

        public List<AccountStatusEnum>? Statuses { get; set; } = new List<AccountStatusEnum>();

        public List<int>? LeadsId { get; set; } = new List<int>();

        public bool? AccountIsDeleted { get; set; }
    }
}
