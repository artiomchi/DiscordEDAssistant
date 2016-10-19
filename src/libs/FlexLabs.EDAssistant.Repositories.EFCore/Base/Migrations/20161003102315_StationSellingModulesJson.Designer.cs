using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FlexLabs.EDAssistant.Repositories.EFCore.Base;

namespace FlexLabs.EDAssistant.Repositories.EFCore.Base.Migrations
{
    [DbContext(typeof(EDAssistantDataContext))]
    [Migration("20161003102315_StationSellingModulesJson")]
    partial class StationSellingModulesJson
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Commodities_Category", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("ID");

                    b.ToTable("Commodities_Categories","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Commodity", b =>
                {
                    b.Property<int>("ID");

                    b.Property<int>("AveragePrice");

                    b.Property<int>("CategoryID");

                    b.Property<bool>("IsRare");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Commodities","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Module", b =>
                {
                    b.Property<int>("ID");

                    b.Property<int>("CategoryID");

                    b.Property<byte>("Class");

                    b.Property<string>("FullName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("GroupID");

                    b.Property<float>("Mass");

                    b.Property<byte?>("MissileType");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<float>("Power");

                    b.Property<int?>("Price");

                    b.Property<byte?>("Rating");

                    b.Property<string>("Ship")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<byte?>("WeaponMode");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("GroupID");

                    b.ToTable("Modules","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Modules_Category", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("ID");

                    b.ToTable("Modules_Categories","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Modules_Group", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("ID");

                    b.ToTable("Modules_Groups","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_StarSystem", b =>
                {
                    b.Property<int>("ID");

                    b.Property<bool>("IsPopulated");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("ID");

                    b.ToTable("StarSystems","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Station", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Allegiance")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int?>("DistanceToStar");

                    b.Property<string>("Faction")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Government")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("HasBlackmarket");

                    b.Property<bool>("HasCommodities");

                    b.Property<bool>("HasDocking");

                    b.Property<bool>("HasMarket");

                    b.Property<bool>("HasOutfitting");

                    b.Property<bool>("HasRearm");

                    b.Property<bool>("HasRefuel");

                    b.Property<bool>("HasRepair");

                    b.Property<bool>("HasShipyard");

                    b.Property<bool>("IsPlanetary");

                    b.Property<DateTime?>("MarketUpdatedAt");

                    b.Property<byte?>("MaxLandingPadSize");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<DateTime?>("OutfittingUpdatedAt");

                    b.Property<DateTime?>("ShipyardUpdatedAt");

                    b.Property<string>("State")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("SystemID");

                    b.Property<int?>("TypeID");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("ID");

                    b.HasIndex("TypeID");

                    b.ToTable("Stations","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Stations_SellingModule", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ModuleID");

                    b.Property<int>("StationID");

                    b.HasKey("PK");

                    b.HasIndex("StationID");

                    b.ToTable("Stations_SellingModules","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Stations_SellingShip", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ship")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("StationID");

                    b.HasKey("PK");

                    b.HasIndex("StationID");

                    b.ToTable("Stations_SellingShips","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Stations_Type", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("ID");

                    b.ToTable("Stations_Types","eddb");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.KosSimpleRule", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AuthorID");

                    b.Property<string>("Rule")
                        .IsRequired();

                    b.Property<long>("ServerID");

                    b.Property<string>("UserName");

                    b.HasKey("PK");

                    b.ToTable("KosSimpleRules");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.KosUserRule", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AuthorID");

                    b.Property<string>("Rule")
                        .IsRequired();

                    b.Property<long>("ServerID");

                    b.Property<long>("UserID");

                    b.HasKey("PK");

                    b.ToTable("KosUserRules");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Server", b =>
                {
                    b.Property<long>("ID");

                    b.Property<string>("CommandPrefix")
                        .HasAnnotation("MaxLength", 5);

                    b.Property<string>("WelcomeMessage");

                    b.HasKey("ID");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Upload_Eddb_Commodity", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AveragePrice");

                    b.Property<int>("CategoryID");

                    b.Property<string>("CategoryName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("ID");

                    b.Property<bool>("IsRare");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("PK");

                    b.ToTable("Eddb_Commodities","upload");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Upload_Eddb_Module", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("CategoryName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<byte>("Class");

                    b.Property<string>("FullName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("GroupID");

                    b.Property<string>("GroupName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("ID");

                    b.Property<float>("Mass");

                    b.Property<byte?>("MissileType");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<float>("Power");

                    b.Property<int?>("Price");

                    b.Property<byte?>("Rating");

                    b.Property<string>("Ship");

                    b.Property<byte?>("WeaponMode");

                    b.HasKey("PK");

                    b.ToTable("Eddb_Modules","upload");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Upload_Eddb_StarSystem", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ID");

                    b.Property<bool>("IsPopulated");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("PK");

                    b.ToTable("Eddb_StarSystems","upload");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Upload_Eddb_Station", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Allegiance")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<int?>("DistanceToStar");

                    b.Property<string>("Faction")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("Government")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<bool>("HasBlackmarket");

                    b.Property<bool>("HasCommodities");

                    b.Property<bool>("HasDocking");

                    b.Property<bool>("HasMarket");

                    b.Property<bool>("HasOutfitting");

                    b.Property<bool>("HasRearm");

                    b.Property<bool>("HasRefuel");

                    b.Property<bool>("HasRepair");

                    b.Property<bool>("HasShipyard");

                    b.Property<int>("ID");

                    b.Property<bool>("IsPlanetary");

                    b.Property<DateTime?>("MarketUpdatedAt");

                    b.Property<byte?>("MaxLandingPadSize");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<DateTime?>("OutfittingUpdatedAt");

                    b.Property<string>("SellingModulesJson");

                    b.Property<string>("SellingShipsJson");

                    b.Property<DateTime?>("ShipyardUpdatedAt");

                    b.Property<string>("State")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<int>("SystemID");

                    b.Property<int?>("TypeID");

                    b.Property<string>("TypeName")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("PK");

                    b.ToTable("Eddb_Stations","upload");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Upload_Eddb_Stations_SellingModule", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ModuleID");

                    b.Property<int>("StationID");

                    b.HasKey("PK");

                    b.ToTable("Eddb_Stations_SellingModules","upload");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Upload_Eddb_Stations_SellingShip", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ship")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("StationID");

                    b.HasKey("PK");

                    b.ToTable("Eddb_Stations_SellingShips","upload");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Commodity", b =>
                {
                    b.HasOne("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Commodities_Category", "Category")
                        .WithMany("Modules")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Module", b =>
                {
                    b.HasOne("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Modules_Category", "Category")
                        .WithMany("Modules")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Modules_Group", "Group")
                        .WithMany("Modules")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Station", b =>
                {
                    b.HasOne("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Stations_Type", "Type")
                        .WithMany("Stations")
                        .HasForeignKey("TypeID");
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Stations_SellingModule", b =>
                {
                    b.HasOne("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Station", "Station")
                        .WithMany("SellingModules")
                        .HasForeignKey("StationID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Stations_SellingShip", b =>
                {
                    b.HasOne("FlexLabs.EDAssistant.Repositories.EFCore.Base.Eddb_Station", "Station")
                        .WithMany("SellingShips")
                        .HasForeignKey("StationID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
