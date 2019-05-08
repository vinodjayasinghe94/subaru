using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Subaru.Presistance.Models
{
    public partial class SubaruConnectDevContext : DbContext
    {
        public SubaruConnectDevContext()
        {
        }

        public SubaruConnectDevContext(DbContextOptions<SubaruConnectDevContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdgroupApps> AdgroupApps { get; set; }
        public virtual DbSet<AdgroupInfo> AdgroupInfo { get; set; }
        public virtual DbSet<ApplicationInfo> ApplicationInfo { get; set; }
        public virtual DbSet<AuditCriticalAlertInfo> AuditCriticalAlertInfo { get; set; }
        public virtual DbSet<CriticalAlertInfo> CriticalAlertInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SubaruConnectDev;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AdgroupApps>(entity =>
            {
                entity.HasKey(e => e.AdGroupAppId);

                entity.ToTable("ADGroupApps");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.AdGroup)
                    .WithMany(p => p.AdgroupApps)
                    .HasForeignKey(d => d.AdGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ADGroupApps_ADGroupInfo");

                entity.HasOne(d => d.App)
                    .WithMany(p => p.AdgroupApps)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ADGroupApps_ApplicationInfo");
            });

            modelBuilder.Entity<AdgroupInfo>(entity =>
            {
                entity.HasKey(e => e.AdgroupId);

                entity.ToTable("ADGroupInfo");

                entity.Property(e => e.AdgroupId).HasColumnName("ADGroupId");

                entity.Property(e => e.AdgroupName)
                    .IsRequired()
                    .HasColumnName("ADGroupName")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ApplicationInfo>(entity =>
            {
                entity.HasKey(e => e.AppId);

                entity.Property(e => e.AppAuthType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AppIconPath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.AppName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValueSql("(N'Application Name')");

                entity.Property(e => e.AppUrl)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AuditCriticalAlertInfo>(entity =>
            {
                entity.HasKey(e => e.AuditId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Alert)
                    .WithMany(p => p.AuditCriticalAlertInfo)
                    .HasForeignKey(d => d.AlertId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuditCriticalAlertInfo_CriticalAlertInfo");
            });

            modelBuilder.Entity<CriticalAlertInfo>(entity =>
            {
                entity.HasKey(e => e.AlertId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.EndDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)/(1))/(2000))");

                entity.Property(e => e.IsVisible)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StartDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)/(1))/(2000))");
            });
        }
    }
}
