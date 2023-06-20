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

        public double BGNRUB { get; set; }
        public double BGNUSD { get; set; }
        public double BGNEUR { get; set; }
        public double BGNCNY { get; set; }
        public double BGNRSD { get; set; }
        public double BGNJPY { get; set; }
        public double BGNARS { get; set; }


        [Required]
        public DateTime dateTime { get; set; }
    }
}