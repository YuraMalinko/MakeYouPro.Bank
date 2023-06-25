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

        public decimal JPYRUB { get; set; }
        public decimal JPYUSD { get; set; }
        public decimal JPYEUR { get; set; }
        public decimal JPYCNY { get; set; }
        public decimal JPYRSD { get; set; }
        public decimal JPYBGN { get; set; }
        public decimal JPYARS { get; set; }


        [Required]
        public DateTime dateTime { get; set; }
    }
}
