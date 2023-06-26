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
    public class USDDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal USDRUB { get; set; }
        public decimal USDEUR { get; set; }
        public decimal USDJPY { get; set; }
        public decimal USDCNY { get; set; }
        public decimal USDRSD { get; set; }
        public decimal USDBGN { get; set; }
        public decimal USDARS { get; set; }


        [Required]
        public DateTime dateTime { get; set; }

    }
}
