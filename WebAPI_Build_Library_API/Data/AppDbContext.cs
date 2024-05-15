using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebAPI_Build_Library_API.Models;
using WebAPI_Build_Library_API.Models.Domain;

namespace WebAPI_Build_Library_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Book_Author> Book_Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book_Author>()
                .HasOne(y => y.Book)
                .WithMany(z => z.Book_Authors)
                .HasForeignKey(x => x.BookId);
            builder.Entity<Book_Author>()
                .HasOne(y => y.Book)
                .WithMany(z => z.Book_Authors)
                .HasForeignKey(x => x.BookId);
            builder.Entity<Image>()
                .ToTable("MyImages"); // Cấu hình tên bảng cho entity Image là "MyImages"
        }

    }
}
