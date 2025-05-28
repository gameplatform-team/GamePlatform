using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescricaoJogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Jogo",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Jogo");
        }
    }
}
