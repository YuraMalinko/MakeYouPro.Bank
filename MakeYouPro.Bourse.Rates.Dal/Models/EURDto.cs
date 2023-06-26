using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class EURDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal? EURRUB { get; set; }
        public decimal? EURUSD { get; set; }
        public decimal? EURJPY { get; set; }
        public decimal? EURCNY { get; set; }
        public decimal? EURRSD { get; set; }
        public decimal? EURBGN { get; set; }
        public decimal? EURARS { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}
