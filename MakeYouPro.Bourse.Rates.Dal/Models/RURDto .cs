using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class RURDto: IRurRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double RUR_USD { get; set; }
        public double RUR_EUR { get; set; }
        public double RUR_JPY { get; set; }
        public double RUR_CNY { get; set; }
        public double RUR_RSD { get; set; }
        public double RUR_BGN { get; set; }
        public double RUR_ARS { get; set; }

        [Required]
        [ForeignKey(nameof(DateTimeId))]
        public DateTimeDto DateTime { get; set; }

        public int DateTimeId { get; set; }

    }
}
