﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class BGNDto: IBgnRepository
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
        public DateTime dateTime { get; set; }
    }
}