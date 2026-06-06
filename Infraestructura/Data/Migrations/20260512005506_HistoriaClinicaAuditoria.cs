using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Data.Migrations
{
    /// <inheritdoc />
    public partial class HistoriaClinicaAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "HistoriasClinicas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "HistoriasClinicas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeCreacion",
                table: "HistoriasClinicas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeEliminacion",
                table: "HistoriasClinicas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeModificacion",
                table: "HistoriasClinicas",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "HistoriasClinicas");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "HistoriasClinicas");

            migrationBuilder.DropColumn(
                name: "FechaDeCreacion",
                table: "HistoriasClinicas");

            migrationBuilder.DropColumn(
                name: "FechaDeEliminacion",
                table: "HistoriasClinicas");

            migrationBuilder.DropColumn(
                name: "FechaDeModificacion",
                table: "HistoriasClinicas");
        }
    }
}
