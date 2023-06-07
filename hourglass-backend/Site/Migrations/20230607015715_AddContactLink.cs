using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hourglass.Site.Migrations
{
    /// <inheritdoc />
    public partial class AddContactLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactLink",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactLink",
                table: "Services");
        }
    }
}
