using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MakeYouPro.Bourse.Rates.Bll;

namespace MakeYouPro.Bourse.Rates.Bll.ModelsBll
{
    public class USDBll
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
