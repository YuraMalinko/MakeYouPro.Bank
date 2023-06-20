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
    public class USDDto: IUsdRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double USDRUB { get; set; }
        public double USDEUR { get; set; }
        public double USDJPY { get; set; }
        public double USDCNY { get; set; }
        public double USDRSD { get; set; }
        public double USDBGN { get; set; }
        public double USDARS { get; set; }


        [Required]
        public DateTime dateTime { get; set; }

    }
}
