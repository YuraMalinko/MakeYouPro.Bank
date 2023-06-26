using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bourse.Rates.Bll.ModelsBll
{
    public class BGNBll
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal BGNRUB { get; set; }
        public double BGNUSD { get; set; }
        public decimal BGNEUR { get; set; }
        public decimal BGNCNY { get; set; }
        public decimal BGNRSD { get; set; }
        public decimal BGNJPY { get; set; }
        public decimal BGNARS { get; set; }
        public DateTime DateTime { get; set; }
    }
}