using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class RUBDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double RUBUSD { get; set; }
        public double RUBEUR { get; set; }
        public double RUBJPY { get; set; }
        public double RUBCNY { get; set; }
        public double RUBRSD { get; set; }
        public double RUBBGN { get; set; }
        public double RUBARS { get; set; }


        [Required]
        public DateTime DateTime { get; set; }

    }
}
