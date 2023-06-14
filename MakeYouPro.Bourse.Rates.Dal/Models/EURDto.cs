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

        public double EUR_RUR { get; set; }
        public double EUR_USD { get; set; }
        public double EUR_JPY { get; set; }
        public double EUR_CNY { get; set; }
        public double EUR_RSD { get; set; }
        public double EUR_BGN { get; set; }
        public double EUR_ARS { get; set; }

        [Required]
        [ForeignKey(nameof(DateTimeId))]
        public DateTimeDto DateTime { get; set; }

        public int DateTimeId { get; set; }
    }
}
