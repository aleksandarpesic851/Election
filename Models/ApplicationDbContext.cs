using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<ProvinceModel> Provinces { get; set; }
        public virtual DbSet<ElectionDistrictModel> Districts { get; set; }
        public virtual DbSet<ElectionCenterModel> ElectionCenters { get; set; }
        public virtual DbSet<UserModel> Accounts { get; set; }
        public virtual DbSet<PartyModel> Parties { get; set; }
        public virtual DbSet<ElectionDateModel> ElectionDates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProvinceModel>(entity =>
            {
                entity.ToTable("province");

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });
            builder.Entity<ElectionDistrictModel>(entity =>
            {
                entity.ToTable("district");

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.province)
                    .HasColumnName("province")
                    .HasMaxLength(255);
            });
            builder.Entity<ElectionCenterModel>(entity =>
            {
                entity.ToTable("election_center");

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.district)
                    .HasColumnName("district")
                    .HasMaxLength(255);
            });
            builder.Entity<UserModel>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.userid)
                    .IsRequired()
                    .HasColumnName("userid")
                    .IsUnicode(true)
                    .HasMaxLength(255);

                entity.Property(e => e.username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(255);

                entity.Property(e => e.role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasMaxLength(255);

                entity.Property(e => e.province)
                    .HasColumnName("province")
                    .HasMaxLength(255);

                entity.Property(e => e.district)
                    .HasColumnName("district")
                    .HasMaxLength(255);

                entity.Property(e => e.election_center)
                    .HasColumnName("election_center")
                    .HasMaxLength(255);
            });
            builder.Entity<PartyModel>(entity =>
            {
                entity.ToTable("party");

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.candidate)
                    .HasColumnName("candidate")
                    .HasMaxLength(255);

                entity.Property(e => e.logo)
                    .HasColumnName("logo")
                    .HasMaxLength(500);
            });

            builder.Entity<ElectionDateModel>(entity =>
            {
                entity.ToTable("election_date");

                entity.Property(e => e.start_time)
                    .HasColumnName("start_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.end_time)
                    .HasColumnName("end_time")
                    .HasColumnType("datetime");
            });
        }
    }
}
