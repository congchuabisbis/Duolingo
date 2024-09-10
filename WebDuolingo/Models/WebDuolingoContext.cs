using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebDuolingo.Models;

public partial class WebDuolingoContext : DbContext
{
    public WebDuolingoContext()
    {
    }

    public WebDuolingoContext(DbContextOptions<WebDuolingoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Listen> Listens { get; set; }

    public virtual DbSet<LogMistake> LogMistakes { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SQL6031.site4now.net;Initial Catalog=db_aa5410_khoale30092003;User Id=db_aa5410_khoale30092003_admin;Password=Khoale@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.IdAns).HasName("PK__Answer__51900A8C2CFC522D");

            entity.ToTable("Answer");

            entity.Property(e => e.IdAns).HasColumnName("Id_Ans");
            entity.Property(e => e.IdQues).HasColumnName("Id_Ques");
            entity.Property(e => e.ImageAns)
                .HasMaxLength(450)
                .IsUnicode(false)
                .HasColumnName("Image_Ans");
            entity.Property(e => e.NameAns)
                .HasMaxLength(450)
                .IsUnicode(false)
                .HasColumnName("Name_Ans");

            entity.HasOne(d => d.IdQuesNavigation).WithMany(p => p.Answers)
                .HasForeignKey(d => d.IdQues)
                .HasConstraintName("FK_Ans_Ques");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Image");

            entity.Property(e => e.FileImg)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("File_img");
            entity.Property(e => e.IdImg).HasColumnName("Id_Img");
            entity.Property(e => e.IdQues).HasColumnName("Id_Ques");

            entity.HasOne(d => d.IdQuesNavigation).WithMany()
                .HasForeignKey(d => d.IdQues)
                .HasConstraintName("FK_Img_Ques");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.IdLevel).HasName("PK__Level__E2D8B6539C6EDDCC");

            entity.ToTable("Level");

            entity.Property(e => e.IdLevel)
                .ValueGeneratedNever()
                .HasColumnName("Id_level");
            entity.Property(e => e.NameLevel)
                .HasMaxLength(100)
                .HasColumnName("Name_level");
            entity.Property(e => e.NoteLevel)
                .HasMaxLength(450)
                .IsUnicode(false)
                .HasColumnName("Note_level");
        });

        modelBuilder.Entity<Listen>(entity =>
        {
            entity.HasKey(e => e.IdListen).HasName("PK__Listen__CD05D8940D371225");

            entity.ToTable("Listen");

            entity.Property(e => e.IdListen).HasColumnName("Id_listen");
            entity.Property(e => e.FileListen)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("File_listen");
            entity.Property(e => e.IdQues).HasColumnName("Id_Ques");

            entity.HasOne(d => d.IdQuesNavigation).WithMany(p => p.Listens)
                .HasForeignKey(d => d.IdQues)
                .HasConstraintName("FK_Lis_Ques");
        });

        modelBuilder.Entity<LogMistake>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdQues });

            entity.ToTable("LogMistake");

            entity.Property(e => e.IdUser).HasColumnName("Id_User");
            entity.Property(e => e.IdQues).HasColumnName("Id_Ques");
            entity.Property(e => e.ContQues).HasColumnName("Cont_Ques");
            entity.Property(e => e.DatetimeQues)
                .HasColumnType("datetime")
                .HasColumnName("Datetime_Ques");

            entity.HasOne(d => d.IdQuesNavigation).WithMany(p => p.LogMistakes)
                .HasForeignKey(d => d.IdQues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_Ques");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.LogMistakes)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_user");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.IdQues).HasName("PK__Question__9BF5EDA7CF520B5C");

            entity.ToTable("Question");

            entity.Property(e => e.IdQues).HasColumnName("ID_Ques");
            entity.Property(e => e.CreateBy).HasMaxLength(450);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IdType).HasColumnName("Id_Type");
            entity.Property(e => e.NameQues)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Name_Ques");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.CorrectAnsNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CorrectAns)
                .HasConstraintName("FK__Question__Correc__14270015");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK_Ques_user");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("FK_Ques_Type");

            entity.HasOne(d => d.LevelNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.Level)
                .HasConstraintName("FK_Ques_level");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("PK__Question__1A20A3D5064A6D21");

            entity.ToTable("QuestionType");

            entity.Property(e => e.IdType).HasColumnName("Id_Type");
            entity.Property(e => e.Name)
                .HasMaxLength(450)
                .IsUnicode(false);
            entity.Property(e => e.Note)
                .HasMaxLength(450)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.HasKey(e => e.IdStr).HasName("PK__Story__2D27940B29BCD161");

            entity.ToTable("Story");

            entity.Property(e => e.IdStr)
                .ValueGeneratedNever()
                .HasColumnName("Id_str");
            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.CreateBy).HasMaxLength(450);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Stories)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK_Str_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
