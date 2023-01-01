using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment2Party.Migrations
{
    /// <inheritdoc />
    public partial class SeedInvitation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Invitations",
                columns: new[] { "InvitationId", "GuestEmail", "GuestName", "PartyId", "Status" },
                values: new object[] { 2, "superman@krypton.com", "Superman", 1, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invitations",
                keyColumn: "InvitationId",
                keyValue: 2);
        }
    }
}
