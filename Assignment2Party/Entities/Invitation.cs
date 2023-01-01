using System.ComponentModel.DataAnnotations;

namespace Assignment2Party.Entities
{
    public enum Status
    {
        InviteNotSent,
        InviteSent,
        RespondedYes,
        RespondedNo
    }
    public class Invitation
    {
        public int InvitationId { get; set; }

        [Required(ErrorMessage = "Please enter the guest's name.")]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "Please enter the guest's email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string GuestEmail { get; set; }

        [Required()]
        public Status Status { get; set; } = Status.InviteNotSent;


        public int PartyId { get; set; }
        public Party? Party { get; set; }
    }
}
