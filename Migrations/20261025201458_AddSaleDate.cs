using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace csharp_projet.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "sale_date",
                table: "car",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sale_date",
                table: "car");
        }
    }
}
