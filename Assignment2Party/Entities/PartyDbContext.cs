using Microsoft.EntityFrameworkCore;
using Assignment2Party.Entities;

namespace Assignment2Party.Entities
{
    public class PartyDbContext : DbContext
    {
        public PartyDbContext(DbContextOptions<PartyDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invitation>().HasOne(i => i.Party).WithMany(m => m.Invitations).HasForeignKey(i => i.PartyId);
            modelBuilder.Entity<Invitation>().HasData(
                new Invitation()
                {
                    InvitationId = 1,
                    GuestName = "Dany",
                    GuestEmail = "dwang6934@conestogac.on.ca",
                    Status = Status.RespondedYes,
                    PartyId = 1
                },
                new Invitation()
                {
                    InvitationId = 2,
                    GuestName = "Superman",
                    GuestEmail = "superman@krypton.com",
                    Status = Status.InviteNotSent,
                    PartyId = 1
                }
             );
            modelBuilder.Entity<Party>().HasData(
                new Party()
                {
                    PartyId = 1,
                    Description = "Dany's birthday party",
                    EventDate = DateTime.Parse("11/19/2022"),
                    Location = "Nowhere"
                }
            );
        }
    }
}
