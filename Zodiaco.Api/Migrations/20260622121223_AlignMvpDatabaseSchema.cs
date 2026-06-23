using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zodiaco.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlignMvpDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Trucks",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "UNDER_REVIEW",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Trucks",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "MXN",
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Trucks",
                type: "character varying(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercialObservations",
                table: "Trucks",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentationStatus",
                table: "Trucks",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalNumber",
                table: "Trucks",
                type: "character varying(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MechanicalCondition",
                table: "Trucks",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentOptions",
                table: "Trucks",
                type: "character varying(80)",
                maxLength: 80,
                nullable: true,
                defaultValue: "CASH_AND_FINANCING");

            migrationBuilder.AddColumn<bool>(
                name: "PriceIncludesVat",
                table: "Trucks",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "VinOrSerial",
                table: "Trucks",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SellRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "PENDING_REVIEW",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DocumentationStatus",
                table: "SellRequests",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "QuoteRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "PENDING_REVIEW",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Leads",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "PENDING_REVIEW",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "LeadType",
                table: "Leads",
                type: "character varying(80)",
                maxLength: 80,
                nullable: true,
                defaultValue: "GENERAL");

            migrationBuilder.AddColumn<Guid>(
                name: "TruckId",
                table: "Leads",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FinancingRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "PENDING_REVIEW",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_Price",
                table: "Trucks",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_Year",
                table: "Trucks",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_LeadType",
                table: "Leads",
                column: "LeadType");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_TruckId",
                table: "Leads",
                column: "TruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Trucks_TruckId",
                table: "Leads",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Trucks_TruckId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_Price",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_Year",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Leads_LeadType",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_TruckId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "CommercialObservations",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "DocumentationStatus",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "InternalNumber",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "MechanicalCondition",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "PaymentOptions",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "PriceIncludesVat",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "VinOrSerial",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "LeadType",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TruckId",
                table: "Leads");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Trucks",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "UNDER_REVIEW");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Trucks",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldDefaultValue: "MXN");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SellRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "PENDING_REVIEW");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentationStatus",
                table: "SellRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "QuoteRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "PENDING_REVIEW");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Leads",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "PENDING_REVIEW");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FinancingRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "PENDING_REVIEW");
        }
    }
}
