using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voyago.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCoreDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookingReference = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ProductType = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemTitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Destination = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuestsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 24, nullable: false),
                    SpecialRequests = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Region = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Tagline = table.Column<string>(type: "TEXT", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    BestSeason = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    RecommendedDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    Featured = table.Column<bool>(type: "INTEGER", nullable: false),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightType = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Airline = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    OriginCity = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    OriginCode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    DestinationCity = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    DestinationCode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Duration = table.Column<long>(type: "INTEGER", nullable: false),
                    Aircraft = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    CabinClass = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Baggage = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CarbonOffsetIncluded = table.Column<bool>(type: "INTEGER", nullable: false),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightOffers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    DestinationId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DestinationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    Stars = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<decimal>(type: "TEXT", precision: 3, scale: 2, nullable: false),
                    PricePerNight = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hotels_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DestinationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    DurationDays = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupSizeMax = table.Column<int>(type: "INTEGER", nullable: false),
                    TravelStyle = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    PricePerPerson = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    AvailableSlots = table.Column<int>(type: "INTEGER", nullable: false),
                    Featured = table.Column<bool>(type: "INTEGER", nullable: false),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPackages_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HotelRoomTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HotelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Size = table.Column<decimal>(type: "TEXT", precision: 8, scale: 2, nullable: false),
                    BedType = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRoomTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelRoomTypes_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageInclusions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TourPackageId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsIncluded = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageInclusions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageInclusions_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageItineraryDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TourPackageId = table.Column<int>(type: "INTEGER", nullable: false),
                    DayNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageItineraryDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageItineraryDays_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    DestinationId = table.Column<int>(type: "INTEGER", nullable: true),
                    TourPackageId = table.Column<int>(type: "INTEGER", nullable: true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 3000, nullable: false),
                    VerifiedBooking = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModerationStatus = table.Column<string>(type: "TEXT", maxLength: 24, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingReference",
                table: "Bookings",
                column: "BookingReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatedAtUtc",
                table: "Bookings",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_Status",
                table: "Bookings",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Country_Region",
                table: "Destinations",
                columns: new[] { "Country", "Region" });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Name",
                table: "Destinations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Published_Featured",
                table: "Destinations",
                columns: new[] { "Published", "Featured" });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_DestinationId",
                table: "Favorites",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_DestinationId",
                table: "Favorites",
                columns: new[] { "UserId", "DestinationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightOffers_FlightType",
                table: "FlightOffers",
                column: "FlightType");

            migrationBuilder.CreateIndex(
                name: "IX_FlightOffers_OriginCode_DestinationCode_Published",
                table: "FlightOffers",
                columns: new[] { "OriginCode", "DestinationCode", "Published" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelRoomTypes_HotelId",
                table: "HotelRoomTypes",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_DestinationId_Published",
                table: "Hotels",
                columns: new[] { "DestinationId", "Published" });

            migrationBuilder.CreateIndex(
                name: "IX_PackageInclusions_TourPackageId_DisplayOrder",
                table: "PackageInclusions",
                columns: new[] { "TourPackageId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PackageItineraryDays_TourPackageId_DayNumber",
                table: "PackageItineraryDays",
                columns: new[] { "TourPackageId", "DayNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DestinationId",
                table: "Reviews",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ModerationStatus_CreatedAtUtc",
                table: "Reviews",
                columns: new[] { "ModerationStatus", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TourPackageId",
                table: "Reviews",
                column: "TourPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPackages_DestinationId_Published",
                table: "TourPackages",
                columns: new[] { "DestinationId", "Published" });

            migrationBuilder.CreateIndex(
                name: "IX_TourPackages_Published_Featured",
                table: "TourPackages",
                columns: new[] { "Published", "Featured" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "FlightOffers");

            migrationBuilder.DropTable(
                name: "HotelRoomTypes");

            migrationBuilder.DropTable(
                name: "PackageInclusions");

            migrationBuilder.DropTable(
                name: "PackageItineraryDays");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "TourPackages");

            migrationBuilder.DropTable(
                name: "Destinations");
        }
    }
}
