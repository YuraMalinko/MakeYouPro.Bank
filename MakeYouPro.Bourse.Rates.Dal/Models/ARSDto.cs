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
    public class ARSDto: IArsRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double ARS_RUR { get; set; }
        public double ARS_USD { get; set; }
        public double ARS_EUR { get; set; }
        public double ARS_CNY { get; set; }
        public double ARS_RSD { get; set; }
        public double ARS_BGN { get; set; }
        public double ARS_JPY { get; set; }

        [Required]
        [ForeignKey(nameof(DateTimeId))]
        public DateTimeDto DateTime { get; set; }

        public int DateTimeId { get; set; }
    }
}
