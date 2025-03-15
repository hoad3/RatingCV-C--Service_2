using Microsoft.EntityFrameworkCore;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;


namespace RatingCV.Data;
public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<cv_ungvien> cv_ungvien { get; set; }
    public DbSet<thong_tin_chi_tiet_ungvien> thong_tin_chi_tiet_ungvien { get; set; }
    
    
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
    }
}