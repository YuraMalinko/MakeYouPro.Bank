using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class BGNDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double BGN_RUR { get; set; }
        public double BGN_USD { get; set; }
        public double BGN_EUR { get; set; }
        public double BGN_CNY { get; set; }
        public double BGN_RSD { get; set; }
        public double BGN_JPY { get; set; }
        public double BGN_ARS { get; set; }

        [Required]
        [ForeignKey(nameof(DateTimeId))]
        public DateTimeDto DateTime { get; set; }

        public int DateTimeId { get; set; }
    }
}