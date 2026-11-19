using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using library.Models;

namespace library.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCopy> BookCopies { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__86516BCF6D59AFFF");

            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("author_name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__490D1AE136D49900");

            entity.HasIndex(e => e.CoverImage, "UQ__Books__3C4E7E06E03B55FB").IsUnique();

            entity.HasIndex(e => e.Isbn, "UQ__Books__99F9D0A4BF92C198").IsUnique();

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CoverImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("cover_image");
            entity.Property(e => e.Descriptions)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descriptions");
            entity.Property(e => e.Edition).HasColumnName("edition");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("isbn");
            entity.Property(e => e.Languages)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Chinese")
                .HasColumnName("languages");
            entity.Property(e => e.PublishedDate).HasColumnName("published_date");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "AssociateBookAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_auth_author"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_auth_book"),
                    j =>
                    {
                        j.HasKey("BookId", "AuthorId").HasName("PK__Associat__A1680C5D74674E1A");
                        j.ToTable("Associate_Book_Author");
                        j.IndexerProperty<int>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<short>("AuthorId").HasColumnName("author_id");
                    });

            entity.HasMany(d => d.Categories).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "AssociateBookCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_cat_category"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_cat_book"),
                    j =>
                    {
                        j.HasKey("BookId", "CategoryId").HasName("PK__Associat__1459F47AB6BC03A1");
                        j.ToTable("Associate_Book_Category");
                        j.IndexerProperty<int>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<short>("CategoryId").HasColumnName("category_id");
                    });

            entity.HasMany(d => d.Publishers).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "AssociateBookPublisher",
                    r => r.HasOne<Publisher>().WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_pub_publisher"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_pub_book"),
                    j =>
                    {
                        j.HasKey("BookId", "PublisherId").HasName("PK__Associat__8A2B25C893C6EAF7");
                        j.ToTable("Associate_Book_Publisher");
                        j.IndexerProperty<int>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<short>("PublisherId").HasColumnName("publisher_id");
                    });
        });

        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__BookCopi__3C21D2D22E7B0453");

            entity.Property(e => e.CopyId).HasColumnName("copy_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CopyCondition)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Good")
                .HasColumnName("copy_condition");
            entity.Property(e => e.ShelfLocation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("shelf_location");
            entity.Property(e => e.Stat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Available")
                .HasColumnName("stat");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("fk_bookcopy_book");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__D54EE9B44B8B7293");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.LoanId).HasName("PK__Loans__A1F795548D0690DE");

            entity.Property(e => e.LoanId).HasColumnName("loan_id");
            entity.Property(e => e.BorrowedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("borrowed_at");
            entity.Property(e => e.CopyId).HasColumnName("copy_id");
            entity.Property(e => e.DueAt).HasColumnName("due_at");
            entity.Property(e => e.ProcessedBy).HasColumnName("processed_by");
            entity.Property(e => e.RenewalCount).HasColumnName("renewal_count");
            entity.Property(e => e.ReturnedAt).HasColumnName("returned_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Copy).WithMany(p => p.Loans)
                .HasForeignKey(d => d.CopyId)
                .HasConstraintName("fk_loan_copy");

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.LoanProcessedByNavigations)
                .HasForeignKey(d => d.ProcessedBy)
                .HasConstraintName("fk_loan_librarian");

            entity.HasOne(d => d.User).WithMany(p => p.LoanUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_loan_user");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__Policies__47DA3F03DD0AB1E4");

            entity.Property(e => e.PolicyId).HasColumnName("policy_id");
            entity.Property(e => e.FinePerDay)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("fine_per_day");
            entity.Property(e => e.LoanDays).HasColumnName("loan_days");
            entity.Property(e => e.MaxLoans).HasColumnName("max_loans");
            entity.Property(e => e.MaxRenewals).HasColumnName("max_renewals");
            entity.Property(e => e.ReservationHoldDays).HasColumnName("reservation_hold_days");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Policies)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_policy_role");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__3263F29D98608CA9");

            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.PublisherName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("publisher_name");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__31384C292C43F0F2");

            entity.Property(e => e.ReservationId).HasColumnName("reservation_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.FullfilledCopyId).HasColumnName("fullfilled_copy_id");
            entity.Property(e => e.ReservedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("reserved_at");
            entity.Property(e => e.Stat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PENDING")
                .HasColumnName("stat");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("fk_reservation_book");

            entity.HasOne(d => d.FullfilledCopy).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.FullfilledCopyId)
                .HasConstraintName("fk_reservation_copy");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_reservation_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC3F334604");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FAEC68A0A");

            entity.HasIndex(e => e.UserName, "UQ__Users__7C9273C405601A98").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Stat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active")
                .HasColumnName("stat");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
