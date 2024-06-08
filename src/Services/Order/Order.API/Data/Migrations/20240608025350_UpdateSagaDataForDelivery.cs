using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSagaDataForDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductsPreservedDate",
                table: "SagaData",
                newName: "InventoryPreservedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryCreatedDate",
                table: "SagaData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCreatedDate",
                table: "SagaData");

            migrationBuilder.RenameColumn(
                name: "InventoryPreservedDate",
                table: "SagaData",
                newName: "ProductsPreservedDate");
        }
    }
}
