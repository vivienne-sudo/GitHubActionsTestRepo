using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CA2V6CP.Models
{
    public class TeeTimeBooking
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

      //  [DataType(DataType.DateTime)]
    //    [Display(Name = "End Time")]
      //  public DateTime? EndTime { get; set; }

        [Display(Name = "Player 1 Name")]
        public string Player1Name { get; set; }

        [Display(Name = "Player 1 Handicap")]
        public int? Player1Handicap { get; set; }

        [Display(Name = "Player 2 Name")]
        public string Player2Name { get; set; }

        [Display(Name = "Player 2 Handicap")]
        public int? Player2Handicap { get; set; }

        [Display(Name = "Player 3 Name")]
        public string Player3Name { get; set; }

        [Display(Name = "Player 3 Handicap")]
        public int? Player3Handicap { get; set; }

        [Display(Name = "Player 4 Name")]
        public string Player4Name { get; set; }

        [Display(Name = "Player 4 Handicap")]
        public int? Player4Handicap { get; set; }
    }


}
