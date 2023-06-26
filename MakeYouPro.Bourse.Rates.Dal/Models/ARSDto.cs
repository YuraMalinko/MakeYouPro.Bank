using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class ARSDto
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
        public DateTime DateTime { get; set; }
    }
}
