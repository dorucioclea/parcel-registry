﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParcelRegistry.Projections.Legacy;

namespace ParcelRegistry.Projections.Legacy.Migrations
{
    [DbContext(typeof(LegacyContext))]
    [Migration("20190122083952_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Position");

                    b.HasKey("Name")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ProjectionStates","ParcelRegistryLegacy");
                });

            modelBuilder.Entity("ParcelRegistry.Projections.Legacy.ParcelDetail.ParcelDetail", b =>
                {
                    b.Property<Guid>("ParcelId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Complete");

                    b.Property<string>("OsloId");

                    b.Property<bool>("Removed");

                    b.Property<string>("StatusAsString")
                        .HasColumnName("Status");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnName("VersionTimestamp");

                    b.HasKey("ParcelId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("Complete");

                    b.HasIndex("OsloId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Removed");

                    b.ToTable("ParcelDetails","ParcelRegistryLegacy");
                });

            modelBuilder.Entity("ParcelRegistry.Projections.Legacy.ParcelDetail.ParcelDetailAddress", b =>
                {
                    b.Property<Guid>("ParcelId");

                    b.Property<Guid>("AddressId");

                    b.HasKey("ParcelId", "AddressId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ParcelAddresses","ParcelRegistryLegacy");
                });

            modelBuilder.Entity("ParcelRegistry.Projections.Legacy.ParcelDetail.ParcelDetailAddress", b =>
                {
                    b.HasOne("ParcelRegistry.Projections.Legacy.ParcelDetail.ParcelDetail")
                        .WithMany("Addresses")
                        .HasForeignKey("ParcelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
