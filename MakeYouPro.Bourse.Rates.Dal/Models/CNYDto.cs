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
    public class CNYDto: ICnyRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double CNY_RUR { get; set; }
        public double CNY_USD { get; set; }
        public double CNY_EUR { get; set; }
        public double CNY_JPY { get; set; }
        public double CNY_RSD { get; set; }
        public double CNY_BGN { get; set; }
        public double CNY_ARS { get; set; }

        [Required]
        public DateTime dateTime { get; set; }
    }
}
