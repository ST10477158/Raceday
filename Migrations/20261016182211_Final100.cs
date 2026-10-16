using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raceday.Migrations
{
    /// <inheritdoc />
    public partial class Final100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "Organisers",
                columns: table => new
                {
                    Organiser_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    User_ID1 = table.Column<int>(type: "int", nullable: true),
                    Organisation_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organisation_Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisers", x => x.Organiser_ID);
                    table.ForeignKey(
                        name: "FK_Organisers_Users_User_ID1",
                        column: x => x.User_ID1,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Participant_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    User_ID1 = table.Column<int>(type: "int", nullable: true),
                    Participant_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Participant_Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Participant_ID);
                    table.ForeignKey(
                        name: "FK_Participants_Users_User_ID1",
                        column: x => x.User_ID1,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Event_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Organiser_ID = table.Column<int>(type: "int", nullable: false),
                    Organiser_ID1 = table.Column<int>(type: "int", nullable: true),
                    Event_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Event_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Event_Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Event_StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Event_ID);
                    table.ForeignKey(
                        name: "FK_Events_Organisers_Organiser_ID1",
                        column: x => x.Organiser_ID1,
                        principalTable: "Organisers",
                        principalColumn: "Organiser_ID");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Category_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event_ID = table.Column<int>(type: "int", nullable: false),
                    Event_ID1 = table.Column<int>(type: "int", nullable: true),
                    Category_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category_EntryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category_QualifyAge = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Category_ID);
                    table.ForeignKey(
                        name: "FK_Categories_Events_Event_ID1",
                        column: x => x.Event_ID1,
                        principalTable: "Events",
                        principalColumn: "Event_ID");
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Route_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event_ID = table.Column<int>(type: "int", nullable: false),
                    Event_ID1 = table.Column<int>(type: "int", nullable: true),
                    Route_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Route_Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Route_Map = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Route_ID);
                    table.ForeignKey(
                        name: "FK_Routes_Events_Event_ID1",
                        column: x => x.Event_ID1,
                        principalTable: "Events",
                        principalColumn: "Event_ID");
                });

            migrationBuilder.CreateTable(
                name: "Enrolments",
                columns: table => new
                {
                    Enrolment_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Participant_ID = table.Column<int>(type: "int", nullable: false),
                    Participant_ID1 = table.Column<int>(type: "int", nullable: true),
                    Category_ID = table.Column<int>(type: "int", nullable: false),
                    Category_ID1 = table.Column<int>(type: "int", nullable: true),
                    Enrolment_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Race_Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrolments", x => x.Enrolment_ID);
                    table.ForeignKey(
                        name: "FK_Enrolments_Categories_Category_ID1",
                        column: x => x.Category_ID1,
                        principalTable: "Categories",
                        principalColumn: "Category_ID");
                    table.ForeignKey(
                        name: "FK_Enrolments_Participants_Participant_ID1",
                        column: x => x.Participant_ID1,
                        principalTable: "Participants",
                        principalColumn: "Participant_ID");
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Results_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enrolment_ID = table.Column<int>(type: "int", nullable: false),
                    Enrolment_ID1 = table.Column<int>(type: "int", nullable: true),
                    Finish_Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    Finish_Position = table.Column<int>(type: "int", nullable: true),
                    Average_Pace = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Category_Position = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Results_ID);
                    table.ForeignKey(
                        name: "FK_Results_Enrolments_Enrolment_ID1",
                        column: x => x.Enrolment_ID1,
                        principalTable: "Enrolments",
                        principalColumn: "Enrolment_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Event_ID1",
                table: "Categories",
                column: "Event_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_Category_ID1",
                table: "Enrolments",
                column: "Category_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_Participant_ID1",
                table: "Enrolments",
                column: "Participant_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Organiser_ID1",
                table: "Events",
                column: "Organiser_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Organisers_User_ID1",
                table: "Organisers",
                column: "User_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_User_ID1",
                table: "Participants",
                column: "User_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Results_Enrolment_ID1",
                table: "Results",
                column: "Enrolment_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Event_ID1",
                table: "Routes",
                column: "Event_ID1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Enrolments");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Organisers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
