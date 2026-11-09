using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class LoanFineRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanId",
                table: "Fines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fines_LoanId",
                table: "Fines",
                column: "LoanId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_Loans_LoanId",
                table: "Fines",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fines_Loans_LoanId",
                table: "Fines");

            migrationBuilder.DropIndex(
                name: "IX_Fines_LoanId",
                table: "Fines");

            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "Fines");
        }
    }
}
