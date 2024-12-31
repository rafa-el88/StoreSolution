using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreSolution.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableMovies_addCollumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityCopies",
                table: "AppMovies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityCopies",
                table: "AppMovies");
        }
    }
}
