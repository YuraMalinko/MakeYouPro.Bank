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
    public class JPYDto: IJpyRepository
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public double JPYRUB { get; set; }
        public double JPYUSD { get; set; }
        public double JPYEUR { get; set; }
        public double JPYCNY { get; set; }
        public double JPYRSD { get; set; }
        public double JPYBGN { get; set; }
        public double JPYARS { get; set; }


        [Required]
        public DateTime dateTime { get; set; }
    }
}
