using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class USDDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double USD_RUR { get; set; }
        public double USD_EUR { get; set; }
        public double USD_JPY { get; set; }
        public double USD_CNY { get; set; }
        public double USD_RSD { get; set; }
        public double USD_BGN { get; set; }
        public double USD_ARS { get; set; }

        [Required]
        [ForeignKey(nameof(DateTimeId))]
        public DateTimeDto DateTime { get; set; }

        public int DateTimeId { get; set; }

    }
}
