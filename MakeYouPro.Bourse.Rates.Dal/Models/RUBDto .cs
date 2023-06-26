using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class RUBDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal? RUBUSD { get; set; }
        public decimal? RUBEUR { get; set; }
        public decimal? RUBJPY { get; set; }
        public decimal? RUBCNY { get; set; }
        public decimal? RUBRSD { get; set; }
        public decimal? RUBBGN { get; set; }
        public decimal? RUBARS { get; set; }


        [Required]
        public DateTime DateTime { get; set; }

    }
}
