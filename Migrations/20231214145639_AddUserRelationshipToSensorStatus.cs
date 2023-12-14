using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace water_level_dotnetcore_api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRelationshipToSensorStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SensorStatusItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SensorStatusItems_UserId",
                table: "SensorStatusItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorStatusItems_AspNetUsers_UserId",
                table: "SensorStatusItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorStatusItems_AspNetUsers_UserId",
                table: "SensorStatusItems");

            migrationBuilder.DropIndex(
                name: "IX_SensorStatusItems_UserId",
                table: "SensorStatusItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SensorStatusItems");
        }
    }
}
