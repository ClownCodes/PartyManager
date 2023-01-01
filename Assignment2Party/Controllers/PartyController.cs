using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment2Party.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Assignment2Party.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;

namespace Assignment2Party.Controllers
{
    public class PartyController : Controller
    {
        private PartyDbContext _partyDbContext;
        public PartyController(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        [HttpGet("/parties")]
        public IActionResult GetAllParties()
        {
            List<Party> parties = _partyDbContext.Parties.Include(p => p.Invitations).ToList();
            return View("Items", parties);
        }

        [HttpGet("/parties/add")]
        public IActionResult AddPartyRequest()
        {
            Party newParty = new Party();
            return View("Add", newParty);
        }
        [HttpPost("/parties")]
        public IActionResult HandleAddParty(Party newParty)
        {
            if (ModelState.IsValid)
            {
                _partyDbContext.Parties.Add(newParty);
                _partyDbContext.SaveChanges();
                int id = newParty.PartyId;
                return RedirectToAction("ManagePartyRequest", new {id = id});
            }
            else
            {
                return View("Add", newParty);
            }
        }

        [HttpGet("/parties/{id}/edit")]
        public IActionResult EditPartyRequest(int id)
        {
            Party party = _partyDbContext.Parties.Find(id);
            return View("Edit", party);
        }
        [HttpPost("/parties/edit")]
        public IActionResult HandleEditParty(Party party)
        {
            if (ModelState.IsValid)
            {
                _partyDbContext.Parties.Update(party);
                _partyDbContext.SaveChanges();
                return RedirectToAction("ManagePartyRequest", new {id = party.PartyId});
            }
            else
            {
                return View("Edit", party);
            }
        }

        [HttpGet("/parties/{id}/manage")]
        public IActionResult ManagePartyRequest(int id)
        {
            Party party = _partyDbContext.Parties.Find(id);
            List<Invitation> invitations = _partyDbContext.Invitations.Where(i => i.PartyId == id).ToList();
            ManagePartyViewModel managePartyViewModel = new ManagePartyViewModel()
            {
                ActiveParty = party,
                Invitations = invitations,
                Invitation = new Invitation()
            };
            return View("Manage", managePartyViewModel);
        }

        [HttpPost("/parties/{id}/manage/create-new-invitation")]
        public IActionResult CreateInvitation(int id, ManagePartyViewModel managePartyViewModel)
        {
            if (ModelState.IsValid)
            {
                managePartyViewModel.Invitation.PartyId = id;
                _partyDbContext.Invitations.Add(managePartyViewModel.Invitation);
                _partyDbContext.SaveChanges();
                return RedirectToAction("ManagePartyRequest", new { id = id });
            }
            else
            {
                managePartyViewModel.ActiveParty = _partyDbContext.Parties.Find(id);
                managePartyViewModel.Invitations = _partyDbContext.Invitations.Where(i => i.PartyId == id).ToList();
                return View("Manage", managePartyViewModel);
            }
        }

        [HttpPost("/parties/{id}/invitation")]
        public IActionResult SendInvitations(int id)
        {
            List<Invitation> invitations = _partyDbContext.Invitations.Where(i => i.PartyId == id).ToList();
            Party activeParty = _partyDbContext.Parties.Find(id);
            foreach (Invitation invitation in invitations)
            {
                if (invitation.Status == Status.InviteNotSent)
                {
                    string guestName = invitation.GuestName;
                    string partyDescription = activeParty.Description;
                    string partyLocation = activeParty.Location;
                    DateTime? partyDate = activeParty.EventDate;

                    string uri = HttpContext.Request.GetDisplayUrl();

                    string fromAddress = "dannie.uwu.dannie@gmail.com";
                    string toAddress = invitation.GuestEmail;
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromAddress, ""), //add password here
                        EnableSsl = true,
                    };
                    var mailMessage = new MailMessage()
                    {
                        From = new MailAddress(fromAddress),
                        Subject = "Party Invitation!",
                        Body = $"<h3>Hello {guestName},</h3><p>You have been invited to the party \"{partyDescription}\" at {partyLocation} on {partyDate}!</p><p>We would be thrill to have you, so please <a href=\"{uri}/{invitation.InvitationId}/response\">let us know</a> if you can attend.</p><p>Sincerely,</p><p>The Party Manager App</p>",
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toAddress);
                    smtpClient.Send(mailMessage);

                    invitation.Status = Status.InviteSent;
                    _partyDbContext.Invitations.Update(invitation);
                    _partyDbContext.SaveChanges();
                }
            }
            return RedirectToAction("ManagePartyRequest", new { id = id });

        }

        [HttpGet("/parties/{partyId}/invitation/{invitationId}/response")]
        public IActionResult SubmitResponse(int partyId, int invitationId)
        {
            Party party = _partyDbContext.Parties.Find(partyId);
            Invitation invitation = _partyDbContext.Invitations.Where(i => i.InvitationId == invitationId).First();
            ManagePartyViewModel managePartyViewModel = new ManagePartyViewModel()
            {
                ActiveParty = party,
                Invitation = invitation
            };
            return View("Response", managePartyViewModel);
        }

        [HttpPost("/parties/{id}/invitation/response/submit")]
        public IActionResult HandleSubmitResponse(int id, Invitation invitation)
        {
            _partyDbContext.Invitations.Update(invitation);
            _partyDbContext.SaveChanges();
            return View("Confirmation", invitation);
        }
    }
}
