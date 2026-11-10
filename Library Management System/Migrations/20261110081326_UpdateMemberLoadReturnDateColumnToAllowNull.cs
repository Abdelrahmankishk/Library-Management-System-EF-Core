using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMemberLoadReturnDateColumnToAllowNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberLoan_Books_BookId",
                table: "MemberLoan");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberLoan_Loans_LoanId",
                table: "MemberLoan");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberLoan_Members_MemberId",
                table: "MemberLoan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberLoan",
                table: "MemberLoan");

            migrationBuilder.RenameTable(
                name: "MemberLoan",
                newName: "MembersLoans");

            migrationBuilder.RenameIndex(
                name: "IX_MemberLoan_MemberId",
                table: "MembersLoans",
                newName: "IX_MembersLoans_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_MemberLoan_BookId",
                table: "MembersLoans",
                newName: "IX_MembersLoans_BookId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "MembersLoans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembersLoans",
                table: "MembersLoans",
                columns: new[] { "LoanId", "BookId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MembersLoans_Books_BookId",
                table: "MembersLoans",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembersLoans_Loans_LoanId",
                table: "MembersLoans",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembersLoans_Members_MemberId",
                table: "MembersLoans",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembersLoans_Books_BookId",
                table: "MembersLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_MembersLoans_Loans_LoanId",
                table: "MembersLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_MembersLoans_Members_MemberId",
                table: "MembersLoans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembersLoans",
                table: "MembersLoans");

            migrationBuilder.RenameTable(
                name: "MembersLoans",
                newName: "MemberLoan");

            migrationBuilder.RenameIndex(
                name: "IX_MembersLoans_MemberId",
                table: "MemberLoan",
                newName: "IX_MemberLoan_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_MembersLoans_BookId",
                table: "MemberLoan",
                newName: "IX_MemberLoan_BookId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "MemberLoan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberLoan",
                table: "MemberLoan",
                columns: new[] { "LoanId", "BookId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MemberLoan_Books_BookId",
                table: "MemberLoan",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberLoan_Loans_LoanId",
                table: "MemberLoan",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberLoan_Members_MemberId",
                table: "MemberLoan",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
