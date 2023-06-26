using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class BGNDto
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


        [Required]
        public DateTime DateTime { get; set; }
    }
}