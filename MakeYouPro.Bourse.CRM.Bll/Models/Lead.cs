
using MakeYouPro.Bource.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bank.CRM.Bll.Models
{
    public class Lead
    {
        public int Id { get; set; }

        public int? Role { get; set; }

        public int? Status { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Name { get; set; }

        public string? MiddleName { get; set; }

        public string? Surname { get; set; }

        public string? PhoneNumber { get; set; }
        
        public string? Email { get; set; }
        
        public string? Citizenship { get; set; }

        public string? PassportSeries { get; set; }

        public string? PassportNumber { get; set; }

        public string? Comment { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
