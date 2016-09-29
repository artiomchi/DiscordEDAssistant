using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FlexLabs.DiscordEDAssistant.Repositories.EFCompact.Base;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.Base.Migrations
{
    [DbContext(typeof(EDAssistantDataContext))]
    [Migration("20160929190457_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-alpha1-22167");

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCompact.Base.Server", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommandPrefix");

                    b.Property<long>("ServerID");

                    b.HasKey("ID");

                    b.ToTable("Servers");
                });
        }
    }
}
