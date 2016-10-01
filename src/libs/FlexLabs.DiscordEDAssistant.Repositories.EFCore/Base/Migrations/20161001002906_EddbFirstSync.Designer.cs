using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Migrations
{
    [DbContext(typeof(EDAssistantDataContext))]
    [Migration("20161001002906_EddbFirstSync")]
    partial class EddbFirstSync
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Eddb_Module", b =>
                {
                    b.Property<int>("ID");

                    b.Property<int>("CategoryID");

                    b.Property<byte>("Class");

                    b.Property<int>("GroupID");

                    b.Property<float>("Mass");

                    b.Property<byte?>("MissileType");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<float>("Power");

                    b.Property<int?>("Price");

                    b.Property<char>("Rating");

                    b.Property<string>("Ship")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<byte?>("WeaponMode");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("GroupID");

                    b.ToTable("Modules","eddb");
                });

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Eddb_Modules_Category", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("ID");

                    b.ToTable("Modules_Categories","eddb");
                });

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Eddb_Modules_Group", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("ID");

                    b.ToTable("Modules_Groups","eddb");
                });

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Eddb_System", b =>
                {
                    b.Property<int>("ID");

                    b.Property<bool>("IsPopulated");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("ID");

                    b.ToTable("Systems","eddb");
                });

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Server", b =>
                {
                    b.Property<long>("ID");

                    b.Property<string>("CommandPrefix")
                        .HasAnnotation("MaxLength", 5);

                    b.Property<string>("WelcomeMessage");

                    b.HasKey("ID");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Upload_Eddb_Module", b =>
                {
                    b.Property<int>("PK")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("CategoryName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<byte>("Class");

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

                    b.Property<char>("Rating");

                    b.Property<string>("Ship");

                    b.Property<byte?>("WeaponMode");

                    b.HasKey("PK");

                    b.ToTable("Eddb_Modules","upload");
                });

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Upload_Eddb_System", b =>
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

                    b.ToTable("Eddb_Systems","upload");
                });

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Eddb_Module", b =>
                {
                    b.HasOne("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Eddb_Modules_Category", "Category")
                        .WithMany("Modules")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Eddb_Modules_Group", "Group")
                        .WithMany("Modules")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
