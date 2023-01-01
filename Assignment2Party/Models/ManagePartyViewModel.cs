using Assignment2Party.Entities;

namespace Assignment2Party.Models
{
    public class ManagePartyViewModel
    {
        public Party? ActiveParty { get; set; }
        public List<Invitation>? Invitations { get; set; }
        public Invitation Invitation { get; set; }
    }
}
