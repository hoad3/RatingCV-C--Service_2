using Microsoft.EntityFrameworkCore;
using RatingCV.Model.cv_ungvien;


namespace RatingCV.Data;
public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<cv_ungvien> cv_ungvien { get; set; }
    
    
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<cv_ungvien>()
            .ToTable("cv_ungvien")
            .HasKey(c => c.ungvienid);
    }
}