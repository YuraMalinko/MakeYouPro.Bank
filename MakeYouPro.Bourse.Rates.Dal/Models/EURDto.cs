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
    public class EURDto: IEurRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double EURRUB { get; set; }
        public double EURUSD { get; set; }
        public double EURJPY { get; set; }
        public double EURCNY { get; set; }
        public double EURRSD { get; set; }
        public double EURBGN { get; set; }
        public double EURARS { get; set; }

        [Required]
        public DateTime dateTime { get; set; }
    }
}
