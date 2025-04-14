using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAppts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserStatusBaseUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserStatus",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserStatus",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "Doctors");
        }
    }
}
