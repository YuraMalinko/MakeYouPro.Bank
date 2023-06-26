using System.ComponentModel.DataAnnotations;

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
