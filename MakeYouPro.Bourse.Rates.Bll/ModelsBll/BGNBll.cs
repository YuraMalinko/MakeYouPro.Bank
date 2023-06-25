using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.Rates.Bll.ModelsBll
{
    public class BGNBll
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal BGNRUB { get; set; }
        public double BGNUSD { get; set; }
        public decimal BGNEUR { get; set; }
        public decimal BGNCNY { get; set; }
        public decimal BGNRSD { get; set; }
        public decimal BGNJPY { get; set; }
        public decimal BGNARS { get; set; }
        public DateTime dateTime { get; set; }

    }
}