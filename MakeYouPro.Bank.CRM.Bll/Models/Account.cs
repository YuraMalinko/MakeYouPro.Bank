using MakeYouPro.Bank.CRM.Dal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bank.CRM.Bll.Models
{
    public class Account
    {
        public int Id { get; set; }
        
        public Lead Lead { get; set; }

        public int LeadId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public int Status { get; set; }

        public string? Comment { get; set; }
    }
}
