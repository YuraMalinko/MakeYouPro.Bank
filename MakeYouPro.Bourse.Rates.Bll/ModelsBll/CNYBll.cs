using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.Rates.Bll.ModelsBll
{
    public class CNYBll
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal CNYRUB { get; set; }
        public decimal CNYUSD { get; set; }
        public decimal CNYEUR { get; set; }
        public decimal CNYJPY { get; set; }
        public decimal CNYRSD { get; set; }
        public decimal CNYBGN { get; set; }
        public decimal CNYARS { get; set; }
        [Required]
        public DateTime dateTime { get; set; }

    }
}
