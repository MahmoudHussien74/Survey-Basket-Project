using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey_Basket.Persistence.Migrations;

/// <inheritdoc />
public partial class addIsDisabledColumToUsersTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsDisabled",
            table: "AspNetUsers",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.UpdateData(
            table: "AspNetUsers",
            keyColumn: "Id",
            keyValue: "5f779d8e7d954b4dbf8b3dc2fc9e8b8a",
            column: "IsDisabled",
            value: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsDisabled",
            table: "AspNetUsers");
    }
}
