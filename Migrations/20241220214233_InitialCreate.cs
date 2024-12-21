using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    INN = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.INN);
                });

            migrationBuilder.CreateTable(
                name: "Founders",
                columns: table => new
                {
                    INN = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientINN = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Founders", x => x.INN);
                    table.ForeignKey(
                        name: "FK_Founders_Clients_ClientINN",
                        column: x => x.ClientINN,
                        principalTable: "Clients",
                        principalColumn: "INN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_INN",
                table: "Clients",
                column: "INN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Founders_ClientINN",
                table: "Founders",
                column: "ClientINN");

            migrationBuilder.CreateIndex(
                name: "IX_Founders_INN",
                table: "Founders",
                column: "INN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Founders");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
