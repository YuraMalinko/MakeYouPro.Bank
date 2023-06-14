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
    public class RSDDto: IRsdRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double RSD_RUR { get; set; }
        public double RSD_USD { get; set; }
        public double RSD_EUR { get; set; }
        public double RSD_CNY { get; set; }
        public double RSD_JPY { get; set; }
        public double RSD_BGN { get; set; }
        public double RSD_ARS { get; set; }

        [Required]
        [ForeignKey(nameof(DateTimeId))]
        public DateTimeDto DateTime { get; set; }

        public int DateTimeId { get; set; }
    }
}
