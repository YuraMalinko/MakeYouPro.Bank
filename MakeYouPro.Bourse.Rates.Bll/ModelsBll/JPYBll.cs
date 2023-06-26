using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bourse.Rates.Bll.ModelsBll
{
    public class JPYBll
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
        public DateTime DateTime { get; set; }
    }
}
