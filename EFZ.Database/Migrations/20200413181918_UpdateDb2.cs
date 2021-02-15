using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFZ.Database.Migrations
{
    public partial class UpdateDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var basePath = typeof(UpdateDb2).Namespace;

            using var streamFillDbData = assembly.GetManifestResourceStream(basePath + ".Scripts.UpdateDb2_script.sql");
            if (streamFillDbData == null) return;
            using var readerFillDbData = new StreamReader(streamFillDbData);
            var sqlResult = readerFillDbData.ReadToEnd();
            migrationBuilder.Sql(sqlResult);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
