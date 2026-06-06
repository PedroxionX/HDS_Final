using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Data.Migrations
{
    /// <inheritdoc />
    public partial class EvolucionAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Evoluciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Evoluciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeCreacion",
                table: "Evoluciones",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeEliminacion",
                table: "Evoluciones",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeModificacion",
                table: "Evoluciones",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Evoluciones");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Evoluciones");

            migrationBuilder.DropColumn(
                name: "FechaDeCreacion",
                table: "Evoluciones");

            migrationBuilder.DropColumn(
                name: "FechaDeEliminacion",
                table: "Evoluciones");

            migrationBuilder.DropColumn(
                name: "FechaDeModificacion",
                table: "Evoluciones");
        }
    }
}
