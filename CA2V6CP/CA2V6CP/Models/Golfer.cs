using System.ComponentModel.DataAnnotations;

namespace CA2V6CP.Models
{
    public class Golfer
    {
        [Key]
        public int Id { get; set; }
        public int MembershipNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(1)]
        [RegularExpression("[MF]")]
        public string Sex { get; set; } = "M"; // default value is "M"

        public int Handicap { get; set; }
    }


}
