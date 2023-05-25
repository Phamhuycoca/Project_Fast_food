using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fast_food.Models
{
    public partial class Fast_foodContext : DbContext
    {
        public Fast_foodContext()
        {
        }

        public Fast_foodContext(DbContextOptions<Fast_foodContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; } = null!;
        public virtual DbSet<DonHang> DonHangs { get; set; } = null!;
        public virtual DbSet<KhachHang> KhachHangs { get; set; } = null!;
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-3I7FN7I; Database=Fast_food; User Id=sa; Password=1234; TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => e.IdCtdh)
                    .HasName("PK__ChiTietD__A09D308181A3C757");

                entity.ToTable("ChiTietDonHang");

                entity.Property(e => e.IdCtdh).HasColumnName("Id_CTDH");

                entity.Property(e => e.IdDh).HasColumnName("Id_DH");

                entity.Property(e => e.IdSp).HasColumnName("Id_SP");

                entity.HasOne(d => d.IdDhNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.IdDh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietDo__Id_DH__300424B4");

                entity.HasOne(d => d.IdSpNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.IdSp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietDo__Id_SP__30F848ED");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.IdDh)
                    .HasName("PK__DonHang__16EC9FD8A84E0266");

                entity.ToTable("DonHang");

                entity.Property(e => e.IdDh).HasColumnName("Id_DH");

                entity.Property(e => e.DiaChi).HasMaxLength(150);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.IdKh).HasColumnName("Id_KH");

                entity.Property(e => e.NgayTao).HasColumnType("date");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdKhNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.IdKh)
                    .HasConstraintName("FK__DonHang__Id_KH__2D27B809");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.IdKh)
                    .HasName("PK__KhachHan__16ECD6E7E42524F3");

                entity.ToTable("KhachHang");

                entity.Property(e => e.IdKh).HasColumnName("Id_KH");

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.Property(e => e.HinhAnh).IsUnicode(false);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.Pass).IsUnicode(false);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UseName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LoaiSanPham>(entity =>
            {
                entity.HasKey(e => e.IdLoai)
                    .HasName("PK__LoaiSanP__134A7BEB4425897A");

                entity.ToTable("LoaiSanPham");

                entity.Property(e => e.IdLoai).HasColumnName("Id_Loai");

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.TenLoai).HasMaxLength(50);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.IdSp)
                    .HasName("PK__SanPham__16EC11E526DAFBF6");

                entity.ToTable("SanPham");

                entity.Property(e => e.IdSp).HasColumnName("Id_SP");

                entity.Property(e => e.HinhAnh).IsUnicode(false);

                entity.Property(e => e.IdLoai).HasColumnName("Id_Loai");

                entity.Property(e => e.TenSp)
                    .HasMaxLength(50)
                    .HasColumnName("TenSP");

                entity.HasOne(d => d.IdLoaiNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.IdLoai)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SanPham__Id_Loai__2A4B4B5E");
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.IdTk)
                    .HasName("PK__TaiKhoan__16EC19C3C9A56621");

                entity.ToTable("TaiKhoan");

                entity.Property(e => e.IdTk).HasColumnName("Id_TK");

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.Property(e => e.HinhAnh).IsUnicode(false);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Pass).IsUnicode(false);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UseName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
