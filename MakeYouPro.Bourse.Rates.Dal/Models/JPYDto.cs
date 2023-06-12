using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class JPYDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double JPY_RUR { get; set; }
        public double JPY_USD { get; set; }
        public double JPY_EUR { get; set; }
        public double JPY_CNY { get; set; }
        public double JPY_RSD { get; set; }
        public double JPY_BGN { get; set; }
        public double JPY_ARS { get; set; }

        [Required]
        [ForeignKey(nameof(DateTimeId))]
        public DateTimeDto DateTime { get; set; }

        public int DateTimeId { get; set; }
    }
}
