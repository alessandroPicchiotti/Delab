using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delab.Backend.Migrations
{
    /// <inheritdoc />
    public partial class step1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Corporation_Countries_CountryId",
                table: "Corporation");

            migrationBuilder.DropForeignKey(
                name: "FK_Corporation_SoftPlan_SoftPlanId",
                table: "Corporation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleDetails_AspNetUsers_UserId",
                table: "UserRoleDetails");

            migrationBuilder.AddColumn<int>(
                name: "CorporationId",
                table: "UserRoleDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStart",
                table: "Corporation",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEnd",
                table: "Corporation",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Nro_Document = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false),
                    Job = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_Managers_Corporation_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporation",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDetails_CorporationId",
                table: "UserRoleDetails",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDetails_UserRoleDetailsId",
                table: "UserRoleDetails",
                column: "UserRoleDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDetails_UserType_UserId",
                table: "UserRoleDetails",
                columns: new[] { "UserType", "UserId" },
                unique: true,
                filter: "[UserType] IS NOT NULL AND [UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SoftPlan_Name",
                table: "SoftPlan",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Corporation_Name_NroDocument",
                table: "Corporation",
                columns: new[] { "Name", "NroDocument" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_CorporationId",
                table: "Managers",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_FullName_Nro_Document",
                table: "Managers",
                columns: new[] { "FullName", "Nro_Document" },
                unique: true,
                filter: "[FullName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserName",
                table: "Managers",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Corporation_Countries_CountryId",
                table: "Corporation",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Corporation_SoftPlan_SoftPlanId",
                table: "Corporation",
                column: "SoftPlanId",
                principalTable: "SoftPlan",
                principalColumn: "SoftPlanId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleDetails_AspNetUsers_UserId",
                table: "UserRoleDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleDetails_Corporation_CorporationId",
                table: "UserRoleDetails",
                column: "CorporationId",
                principalTable: "Corporation",
                principalColumn: "CorporationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Corporation_Countries_CountryId",
                table: "Corporation");

            migrationBuilder.DropForeignKey(
                name: "FK_Corporation_SoftPlan_SoftPlanId",
                table: "Corporation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleDetails_AspNetUsers_UserId",
                table: "UserRoleDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleDetails_Corporation_CorporationId",
                table: "UserRoleDetails");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_UserRoleDetails_CorporationId",
                table: "UserRoleDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserRoleDetails_UserRoleDetailsId",
                table: "UserRoleDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserRoleDetails_UserType_UserId",
                table: "UserRoleDetails");

            migrationBuilder.DropIndex(
                name: "IX_SoftPlan_Name",
                table: "SoftPlan");

            migrationBuilder.DropIndex(
                name: "IX_Corporation_Name_NroDocument",
                table: "Corporation");

            migrationBuilder.DropColumn(
                name: "CorporationId",
                table: "UserRoleDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStart",
                table: "Corporation",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEnd",
                table: "Corporation",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "FK_Corporation_Countries_CountryId",
                table: "Corporation",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Corporation_SoftPlan_SoftPlanId",
                table: "Corporation",
                column: "SoftPlanId",
                principalTable: "SoftPlan",
                principalColumn: "SoftPlanId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleDetails_AspNetUsers_UserId",
                table: "UserRoleDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
