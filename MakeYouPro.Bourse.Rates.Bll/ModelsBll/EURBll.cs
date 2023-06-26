﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.Rates.Bll.ModelsBll
{
    public class EURBll
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal EURRUB { get; set; }
        public decimal EURUSD { get; set; }
        public decimal EURJPY { get; set; }
        public decimal EURCNY { get; set; }
        public decimal EURRSD { get; set; }
        public decimal EURBGN { get; set; }
        public decimal EURARS { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

    }
}
