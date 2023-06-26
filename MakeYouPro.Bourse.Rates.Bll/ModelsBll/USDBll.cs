using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bourse.Rates.Bll.ModelsBll
{
    public class USDBll
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal USDRUB { get; set; }
        public decimal USDEUR { get; set; }
        public decimal USDJPY { get; set; }
        public decimal USDCNY { get; set; }
        public decimal USDRSD { get; set; }
        public decimal USDBGN { get; set; }
        public decimal USDARS { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}
