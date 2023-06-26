using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bourse.Rates.Dal.Models
{
    public class RSDDto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public decimal RSDRUB { get; set; }
        public decimal RSDUSD { get; set; }
        public decimal RSDEUR { get; set; }
        public decimal RSDCNY { get; set; }
        public decimal RSDJPY { get; set; }
        public decimal RSDBGN { get; set; }
        public decimal RSDARS { get; set; }


        [Required]
        public DateTime dateTime { get; set; }
    }
}
