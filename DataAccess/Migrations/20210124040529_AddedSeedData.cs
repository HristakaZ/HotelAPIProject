using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddedSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "e104aa09-0b45-4a69-975d-3d402b805934", "PositionApplicationRole", "Admin", null },
                    { 2, "6fe2a390-4abd-4fa6-8f3c-870763b75db7", "PositionApplicationRole", "No Position!", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "bf1c97b7-db41-4007-b0b4-a5d7e8e17d2e", null, false, false, null, null, null, "AQAAAAEAACcQAAAAEDCUhXvWG87JiLCeG4vyffNoWZZIheI5/Wo8JxD23u4BAiPoIVimT4oeMeOkRLEkjQ==", null, false, 1, null, false, "Admin" }, //1 = PositionId(Admin), password = "Admin1/"
                    { 2, 0, "581cb19b-68a3-4501-a8fe-ee5dba91dce9", null, false, false, null, null, null, null, null, false, null, null, false, "NoEmployee" }
                });

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "No Guest!" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
