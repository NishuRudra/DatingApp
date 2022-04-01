using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migartions
{
    public partial class LikedUserEntity1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_LikedUserId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_SourceUserId",
                table: "Likes");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_LikedUserId",
                table: "Likes",
                column: "LikedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_SourceUserId",
                table: "Likes",
                column: "SourceUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_LikedUserId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_SourceUserId",
                table: "Likes");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_LikedUserId",
                table: "Likes",
                column: "LikedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_SourceUserId",
                table: "Likes",
                column: "SourceUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
