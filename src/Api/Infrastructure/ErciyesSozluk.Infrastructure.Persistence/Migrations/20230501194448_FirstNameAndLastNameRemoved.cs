using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErciyesSozluk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FirstNameAndLastNameRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "dbo",
                table: "user");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "dbo",
                table: "user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
