using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class FixSmppSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SystemId",
                table: "SMPPSettings",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Host",
                table: "SMPPSettings",
                newName: "HostName");

            migrationBuilder.AddColumn<string>(
                name: "DataEncoding",
                table: "SMPPSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryUserAckRequest",
                table: "SMPPSettings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestAddressNpi",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DestAddressTon",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnquireLinkInterval",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InterfaceVersion",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IntermediateNotification",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxPendingSubmits",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReceivePort",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SourceAddressAutodetect",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SourceAddressNpi",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SourceAddressTon",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SystemType",
                table: "SMPPSettings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Throughput",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransceiverMode",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValidityPeriod",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaitAckExpire",
                table: "SMPPSettings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEncoding",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "DeliveryUserAckRequest",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "DestAddressNpi",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "DestAddressTon",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "EnquireLinkInterval",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "InterfaceVersion",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "IntermediateNotification",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "MaxPendingSubmits",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "ReceivePort",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "SourceAddressAutodetect",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "SourceAddressNpi",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "SourceAddressTon",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "SystemType",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "Throughput",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "TransceiverMode",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "ValidityPeriod",
                table: "SMPPSettings");

            migrationBuilder.DropColumn(
                name: "WaitAckExpire",
                table: "SMPPSettings");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "SMPPSettings",
                newName: "SystemId");

            migrationBuilder.RenameColumn(
                name: "HostName",
                table: "SMPPSettings",
                newName: "Host");
        }
    }
}
