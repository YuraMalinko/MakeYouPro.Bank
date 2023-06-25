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
    public class BGNDto: IBgnRepository
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


        [Required]
        public DateTime dateTime { get; set; }
    }
}