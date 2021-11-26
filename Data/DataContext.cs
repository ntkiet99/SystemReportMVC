using System.Data.Entity;
using SystemReportMVC.Models;

namespace SystemReportMVC.Data
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<DonVi> DonVis { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<NguoiDungQuyen> NguoiDungQuyens { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<QuyenMenu> QuyenMenus { get; set; }
        public virtual DbSet<TrangThai> TrangThais { get; set; }
        public DataContext() : base("name=DataContextConnectionString")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DonVi>().Ignore(e => e.RowNum);
            modelBuilder.Entity<DonVi>().HasOptional(d => d.DonViCha)
                    .WithMany(p => p.DonViCons)
                    .HasForeignKey(d => d.DonViChaId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Menu>().HasOptional(d => d.MenuCha)
                    .WithMany(p => p.MenuCons)
                    .HasForeignKey(d => d.MenuChaId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quyen>().Ignore(e => e.Menus);
            modelBuilder.Entity<Menu>().Ignore(e => e.Active);
            modelBuilder.Entity<Quyen>().Ignore(e => e.MenuIds);
            modelBuilder.Entity<NguoiDung>().HasOptional(d => d.DonVi)
                   .WithMany(p => p.NguoiDung)
                   .HasForeignKey(d => d.DonViId)
                   .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDungQuyen>().HasOptional(d => d.NguoiDung)
                    .WithMany(p => p.NguoiDungQuyen)
                    .HasForeignKey(d => d.NguoiDungId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDungQuyen>().HasOptional(d => d.Quyen)
                .WithMany(p => p.NguoiDungQuyen)
                .HasForeignKey(d => d.QuyenId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuyenMenu>().HasOptional(d => d.Menu)
                .WithMany(p => p.QuyenMenu)
                .HasForeignKey(d => d.MenuId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuyenMenu>().HasOptional(d => d.Quyen)
                .WithMany(p => p.QuyenMenu)
                .HasForeignKey(d => d.QuyenId)
                .WillCascadeOnDelete(false);
        }
    }
}
