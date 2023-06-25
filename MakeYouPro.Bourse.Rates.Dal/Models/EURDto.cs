using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class EURDto: IEurRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal EURRUB { get; set; }
        public decimal EURUSD { get; set; }
        public decimal EURJPY { get; set; }
        public decimal EURCNY { get; set; }
        public decimal EURRSD { get; set; }
        public decimal EURBGN { get; set; }
        public decimal EURARS { get; set; }

        [Required]
        public DateTime dateTime { get; set; }
    }
}
