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
    public class ARSDto: IArsRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double ARSRUB { get; set; }
        public double ARSUSD { get; set; }
        public double ARSEUR { get; set; }
        public double ARSCNY { get; set; }
        public double ARSRSD { get; set; }
        public double ARSBGN { get; set; }
        public double ARSJPY { get; set; }


        [Required]
        public DateTime dateTime { get; set; }
    }
}
