using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PersonalPlans.Infrastructure.Persistence;

#nullable disable

namespace PersonalPlans.Infrastructure.Migrations;

[DbContext(typeof(PersonalPlansContext))]
[Migration("20261030180000_AddLoginAttemptLockout")]
public partial class AddLoginAttemptLockout : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "FailedLoginAttempts",
            table: "Users",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<DateTime>(
            name: "LockoutEndUtc",
            table: "Users",
            type: "datetime(6)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "FailedLoginAttempts", table: "Users");
        migrationBuilder.DropColumn(name: "LockoutEndUtc", table: "Users");
    }
}
