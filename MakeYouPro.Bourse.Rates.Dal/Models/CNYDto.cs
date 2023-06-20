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

        public double CNYRUB { get; set; }
        public double CNYUSD { get; set; }
        public double CNYEUR { get; set; }
        public double CNYJPY { get; set; }
        public double CNYRSD { get; set; }
        public double CNYBGN { get; set; }
        public double CNYARS { get; set; }

        [Required]
        public DateTime dateTime { get; set; }
    }
}
