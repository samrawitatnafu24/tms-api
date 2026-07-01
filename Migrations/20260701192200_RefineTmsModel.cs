using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tms_Api.Migrations
{
    /// <inheritdoc />
    public partial class RefineTmsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuedAt",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "CredentialUrl",
                table: "Certificates",
                newName: "CertificateNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CertificateNumber",
                table: "Certificates",
                newName: "CredentialUrl");

            migrationBuilder.AddColumn<DateTime>(
                name: "IssuedAt",
                table: "Certificates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
