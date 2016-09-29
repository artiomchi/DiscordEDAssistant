using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Migrations
{
    [DbContext(typeof(EDAssistantDataContext))]
    partial class EDAssistantDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base.Server", b =>
                {
                    b.Property<long>("ID");

                    b.Property<string>("CommandPrefix")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 5);

                    b.HasKey("ID");

                    b.ToTable("Servers");
                });
        }
    }
}
