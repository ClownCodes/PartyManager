using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Assignment2Party.Entities
{
    public class Party
    {
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Please enter the description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please choose the date.")]
        public DateTime? EventDate { get; set; }

        public string? Location { get; set; }

     
        public List<Invitation>? Invitations { get; set; }
    }
}
