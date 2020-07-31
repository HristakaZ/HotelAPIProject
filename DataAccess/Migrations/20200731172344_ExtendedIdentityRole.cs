using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ExtendedIdentityRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Positions_PositionID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.RenameColumn(
                name: "PositionID",
                table: "AspNetUsers",
                newName: "PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_PositionID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_PositionId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_PositionId",
                table: "AspNetUsers",
                column: "PositionId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_PositionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "AspNetUsers",
                newName: "PositionID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_PositionId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_PositionID");

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Positions_PositionID",
                table: "AspNetUsers",
                column: "PositionID",
                principalTable: "Positions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
