using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProzorroDataMining.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tenders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    BudgetAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BudgetCurrency = table.Column<string>(type: "varchar(3)", nullable: false, defaultValue: "UAH"),
                    ProcuringEntityId = table.Column<string>(type: "varchar(50)", nullable: true),
                    ProcuringEntityName = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    DateModified = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    FetchedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tender_awards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", nullable: false),
                    TenderId = table.Column<string>(type: "varchar(32)", nullable: false),
                    AwardValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", nullable: true),
                    SupplierName = table.Column<string>(type: "text", nullable: false),
                    SupplierId = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tender_awards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tender_awards_tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tender_contracts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", nullable: false),
                    TenderId = table.Column<string>(type: "varchar(32)", nullable: false),
                    ContractValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "varchar(3)", nullable: false, defaultValue: "UAH"),
                    Status = table.Column<string>(type: "varchar(20)", nullable: true),
                    DateSigned = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tender_contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tender_contracts_tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tender_items",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", nullable: false),
                    TenderId = table.Column<string>(type: "varchar(32)", nullable: false),
                    CpvCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    CpvDescription = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    UnitName = table.Column<string>(type: "varchar(100)", nullable: true),
                    UnitCode = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tender_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tender_items_tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_tender_awards_supplier_name",
                table: "tender_awards",
                column: "SupplierName");

            migrationBuilder.CreateIndex(
                name: "idx_tender_awards_tender_id",
                table: "tender_awards",
                column: "TenderId");

            migrationBuilder.CreateIndex(
                name: "idx_tender_contracts_tender_id",
                table: "tender_contracts",
                column: "TenderId");

            migrationBuilder.CreateIndex(
                name: "idx_tender_items_cpv_code",
                table: "tender_items",
                column: "CpvCode");

            migrationBuilder.CreateIndex(
                name: "idx_tender_items_tender_id",
                table: "tender_items",
                column: "TenderId");

            migrationBuilder.CreateIndex(
                name: "idx_tenders_date_created",
                table: "tenders",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "idx_tenders_entity_name",
                table: "tenders",
                column: "ProcuringEntityName");

            migrationBuilder.CreateIndex(
                name: "idx_tenders_status",
                table: "tenders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "idx_tenders_status_date",
                table: "tenders",
                columns: new[] { "Status", "DateCreated" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tender_awards");

            migrationBuilder.DropTable(
                name: "tender_contracts");

            migrationBuilder.DropTable(
                name: "tender_items");

            migrationBuilder.DropTable(
                name: "tenders");
        }
    }
}
