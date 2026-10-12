using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Товары> Товары { get; set; }
        public DbSet<Категории> Категории { get; set; }
        public DbSet<Поставщики> Поставщики { get; set; }
        public DbSet<Производители> Производители { get; set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              
                optionsBuilder.UseSqlServer(
                    @"Server=dbsrv\VIP2025;Database=sme12.10.tckh;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Категории>().HasKey(c => c.ИдКатегории);
            modelBuilder.Entity<Поставщики>().HasKey(p => p.ИдПоставщика);
            modelBuilder.Entity<Производители>().HasKey(p => p.ИдПроизводителя);
            modelBuilder.Entity<Товары>().HasKey(t => t.ИдТовары);
            modelBuilder.Entity<Товары>(entity =>
            {
                entity.HasKey(e => e.ИдТовары);

                entity.HasOne(d => d.ВкКатегорииNavigation)
                    .WithMany(p => p.Товары)
                    .HasForeignKey(d => d.ВкКатегории)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ВкПоставщикиNavigation)
                    .WithMany(p => p.Товары)
                    .HasForeignKey(d => d.ВкПоставщики)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ВкПроизводителиNavigation)
                    .WithMany(p => p.Товары)
                    .HasForeignKey(d => d.ВкПроизводители)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

        }

    }
}