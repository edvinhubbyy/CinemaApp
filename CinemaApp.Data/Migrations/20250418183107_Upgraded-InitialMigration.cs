using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpgradedInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersMovies_AspNetUsers_ApplicationUserId",
                table: "UsersMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersMovies_Movies_MovieId",
                table: "UsersMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersMovies",
                table: "UsersMovies");

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("8af629f8-3d2a-4d2b-86c9-c7096cbf8f9f"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("d4776193-0c79-4f8d-a188-4864fa4ae0b4"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("f494d69d-ae13-47bd-b9fa-6dd4438ab9b4"));

            migrationBuilder.RenameTable(
                name: "UsersMovies",
                newName: "ApplicationUserMovies");

            migrationBuilder.RenameIndex(
                name: "IX_UsersMovies_MovieId",
                table: "ApplicationUserMovies",
                newName: "IX_ApplicationUserMovies_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserMovies",
                table: "ApplicationUserMovies",
                columns: new[] { "ApplicationUserId", "MovieId" });

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("3afe574d-9b9d-427e-9b5b-41af1d59e7a6"), "Plovdiv", "Cinema city" },
                    { new Guid("62147fdc-efa1-4bb4-8a8d-9eba39c728fe"), "Varna", "Cinemax" },
                    { new Guid("b6e8fa59-7fe0-4b46-9b1f-efa46eff6b64"), "Sofia", "Cinema city" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserMovies_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserMovies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserMovies_Movies_MovieId",
                table: "ApplicationUserMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserMovies_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserMovies_Movies_MovieId",
                table: "ApplicationUserMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserMovies",
                table: "ApplicationUserMovies");

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

            migrationBuilder.RenameTable(
                name: "ApplicationUserMovies",
                newName: "UsersMovies");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserMovies_MovieId",
                table: "UsersMovies",
                newName: "IX_UsersMovies_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersMovies",
                table: "UsersMovies",
                columns: new[] { "ApplicationUserId", "MovieId" });

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("8af629f8-3d2a-4d2b-86c9-c7096cbf8f9f"), "Sofia", "Cinema city" },
                    { new Guid("d4776193-0c79-4f8d-a188-4864fa4ae0b4"), "Plovdiv", "Cinema city" },
                    { new Guid("f494d69d-ae13-47bd-b9fa-6dd4438ab9b4"), "Varna", "Cinemax" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersMovies_AspNetUsers_ApplicationUserId",
                table: "UsersMovies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersMovies_Movies_MovieId",
                table: "UsersMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
