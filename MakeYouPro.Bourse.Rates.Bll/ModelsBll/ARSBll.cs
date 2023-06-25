using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.Rates.Bll.ModelsBll
{
    public class ARSBll
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal ARSRUB { get; set; }
        public decimal ARSUSD { get; set; }
        public decimal ARSEUR { get; set; }
        public decimal ARSCNY { get; set; }
        public decimal ARSRSD { get; set; }
        public decimal ARSBGN { get; set; }
        public decimal ARSJPY { get; set; }
        [Required]
        public DateTime dateTime { get; set; }


    }
}
