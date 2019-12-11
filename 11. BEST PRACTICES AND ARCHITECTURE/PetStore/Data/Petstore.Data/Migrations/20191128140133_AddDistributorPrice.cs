using Microsoft.EntityFrameworkCore.Migrations;

namespace Petstore.Data.Migrations
{
    public partial class AddDistributorPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DestributorPrice",
                table: "Toys",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DestributorPrice",
                table: "Foods",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestributorPrice",
                table: "Toys");

            migrationBuilder.DropColumn(
                name: "DestributorPrice",
                table: "Foods");
        }
    }
}
