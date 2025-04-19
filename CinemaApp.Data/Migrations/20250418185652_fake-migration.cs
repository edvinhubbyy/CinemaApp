using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class fakemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("3afe574d-9b9d-427e-9b5b-41af1d59e7a6"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("62147fdc-efa1-4bb4-8a8d-9eba39c728fe"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("b6e8fa59-7fe0-4b46-9b1f-efa46eff6b64"));

            migrationBuilder.AlterTable(
                name: "ApplicationUserMovies",
                comment: "Movie watchlist for system user");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationUserMovies",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Shows if movie from user watchlist is deleted",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "MovieId",
                table: "ApplicationUserMovies",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign key to the movie",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "ApplicationUserMovies",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Foreign key to the user",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("7a49e2e2-efce-47f6-80b9-37392a17dad8"), "Sofia", "Cinema city" },
                    { new Guid("85da53f3-6f58-426f-b134-5e05891dfc3e"), "Plovdiv", "Cinema city" },
                    { new Guid("ee839549-58ca-4711-b119-9877ea00d83c"), "Varna", "Cinemax" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("7a49e2e2-efce-47f6-80b9-37392a17dad8"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("85da53f3-6f58-426f-b134-5e05891dfc3e"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("ee839549-58ca-4711-b119-9877ea00d83c"));

            migrationBuilder.AlterTable(
                name: "ApplicationUserMovies",
                oldComment: "Movie watchlist for system user");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationUserMovies",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Shows if movie from user watchlist is deleted");

            migrationBuilder.AlterColumn<Guid>(
                name: "MovieId",
                table: "ApplicationUserMovies",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign key to the movie");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "ApplicationUserMovies",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Foreign key to the user");

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("3afe574d-9b9d-427e-9b5b-41af1d59e7a6"), "Plovdiv", "Cinema city" },
                    { new Guid("62147fdc-efa1-4bb4-8a8d-9eba39c728fe"), "Varna", "Cinemax" },
                    { new Guid("b6e8fa59-7fe0-4b46-9b1f-efa46eff6b64"), "Sofia", "Cinema city" }
                });
        }
    }
}
