using Microsoft.EntityFrameworkCore;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.du_an;
using RatingCV.Model.github;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;


namespace RatingCV.Data;
public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<cv_ungvien> cv_ungvien { get; set; }
    public DbSet<thong_tin_chi_tiet_ungvien> thong_tin_chi_tiet_ungvien { get; set; }
    
    public DbSet<du_an> du_an { get; set; }
    
    public DbSet<github_link> github_link { get; set; }
    
    
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
            .HasMany<github_link>()
            .WithOne()
            .HasForeignKey(g => g.userid)
            .OnDelete(DeleteBehavior.Cascade);
     
    }
}