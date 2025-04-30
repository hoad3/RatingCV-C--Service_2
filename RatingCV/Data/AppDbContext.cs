using Microsoft.EntityFrameworkCore;
using RatingCV.Model;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.du_an;
using RatingCV.Model.github;
using RatingCV.Model.session;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;


namespace RatingCV.Data;
public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<cv_ungvien> cv_ungvien { get; set; }
    public DbSet<thong_tin_chi_tiet_ungvien> thong_tin_chi_tiet_ungvien { get; set; }
    
    public DbSet<du_an> du_an { get; set; }
    
    public DbSet<github_link> github_link { get; set; }
    public DbSet<rating_cv> rating_cv { get; set; }
    
    public DbSet<session> session { get; set; }
    
    public DbSet<danh_gia_theo_tieu_chi> danh_gia_theo_tieu_chi { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<cv_ungvien>()
            .ToTable("cv_ungvien")
            .HasKey(c => c.ungvienid);
        
        
        modelBuilder.Entity<thong_tin_chi_tiet_ungvien>()
            .ToTable("thong_tin_chi_tiet_ungvien")
            .HasKey(t => t.ungvienid);
        
        modelBuilder.Entity<thong_tin_chi_tiet_ungvien>()
            .ToTable("thong_tin_chi_tiet_ungvien")
            .Property(c => c.id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<cv_ungvien>()
            .HasOne<thong_tin_chi_tiet_ungvien>()
            .WithOne()
            .HasForeignKey<thong_tin_chi_tiet_ungvien>(t => t.ungvienid)
            .OnDelete(DeleteBehavior.Cascade); // Xóa ứng viên sẽ xóa thông tin chi tiết

        modelBuilder.Entity<du_an>()
            .ToTable("du_an")
            .HasKey(d => d.du_an_id);

        modelBuilder.Entity<du_an>()
            .ToTable("du_an")
            .Property(d => d.du_an_id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<cv_ungvien>()
            .HasMany<du_an>()
            .WithOne()
            .HasForeignKey(d => d.userid)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Project>().HasNoKey();
        
        modelBuilder.Entity<github_link>()
            .ToTable("github")
            .Property(g => g.id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<github_link>()
            .ToTable("github")
            .HasKey(g => g.id);
        
        modelBuilder.Entity<cv_ungvien>()
            .HasMany(c => c.github_links)
            .WithOne(g => g.cv_ungvien)
            .HasForeignKey(g => g.userid)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<rating_cv>()
            .ToTable("rating_cv")
            .Property(r => r.id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<rating_cv>()
            .ToTable("rating_cv")
            .HasKey(r => r.id);
        
        modelBuilder.Entity<session>()
            .ToTable("session")
            .Property(s => s.session_id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<session>()
            .ToTable("session")
            .HasKey(s => s.session_id);
        
        modelBuilder.Entity<danh_gia_theo_tieu_chi>()
            .ToTable("danh_gia_theo_tieu_chi")
            .HasKey(r => r.id_danh_gia);
        modelBuilder.Entity<danh_gia_theo_tieu_chi>()
            .ToTable("danh_gia_theo_tieu_chi")
            .Property(d => d.id_danh_gia)
            .ValueGeneratedOnAdd();
    }
}