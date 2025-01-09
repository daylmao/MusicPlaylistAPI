using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlaylistAPI.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Playlists",
                newName: "CreateAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Playlists",
                newName: "DateCreated");
        }
    }
}
