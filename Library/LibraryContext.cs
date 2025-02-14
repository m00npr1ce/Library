using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Library;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Author_pkey");

            entity.ToTable("Author");

            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Book_pkey");

            entity.ToTable("Book");

            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("Book_Author_fkey");

            entity.HasOne(d => d.GenreNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.Genre)
                .HasConstraintName("Book_Genre_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Genre_pkey");

            entity.ToTable("Genre");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public void AddDataToDatabase()
    {
        using var context = new LibraryContext();

        // Проверка на существующие данные
        if (!context.Genres.Any() && !context.Authors.Any() && !context.Books.Any())
        {
            // Добавляем жанры
            var genre1 = new Genre { Name = "Fiction", Description = "Fictional books" };
            var genre2 = new Genre { Name = "Non-Fiction", Description = "Non-fictional books" };
            context.Genres.Add(genre1);
            context.Genres.Add(genre2);

            // Добавляем авторов
            var author1 = new Author { FirstName = "John", LastName = "Doe", Country = "USA" };
            var author2 = new Author { FirstName = "Jane", LastName = "Smith", Country = "UK" };
            context.Authors.Add(author1);
            context.Authors.Add(author2);

            context.SaveChanges();

            // Добавляем книги
            var book1 = new Book { Title = "Book One", Isbn = "111111", Author = author1.Id, Genre = genre1.Id };
            var book2 = new Book { Title = "Book Two", Isbn = "222222", Author = author2.Id, Genre = genre2.Id };
            context.Books.Add(book1);
            context.Books.Add(book2);

            // Сохраняем изменения
            context.SaveChanges();
        }
    }

}

