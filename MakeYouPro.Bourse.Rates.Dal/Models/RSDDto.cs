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
    public class RSDDto: IRsdRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double RSDRUB { get; set; }
        public double RSDUSD { get; set; }
        public double RSDEUR { get; set; }
        public double RSDCNY { get; set; }
        public double RSDJPY { get; set; }
        public double RSDBGN { get; set; }
        public double RSDARS { get; set; }


        [Required]
        public DateTime dateTime { get; set; }
    }
}
