using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LudoSnakeAndLadder.Databases.Models;

public partial class SnakeDbContext : DbContext
{
    public SnakeDbContext()
    {
    }

    public SnakeDbContext(DbContextOptions<SnakeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GamePlayRecord> GamePlayRecords { get; set; }

    public virtual DbSet<GameRecord> GameRecords { get; set; }

    public virtual DbSet<PlayerCharacter> PlayerCharacters { get; set; }

    public virtual DbSet<SnakeBoard> SnakeBoards { get; set; }

   /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = .;Initial Catalog = SnakeLadder ;User ID =sa; Password = sasa@123;TrustServerCertificate  = True");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GamePlayRecord>(entity =>
        {
            entity.HasKey(e => e.MoveId).HasName("PK__GamePlay__A931A41CB5062409");

            entity.ToTable("GamePlayRecord");

            entity.Property(e => e.GameCode)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.MoveType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PlayerId)
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.HasOne(d => d.GameCodeNavigation).WithMany(p => p.GamePlayRecords)
                .HasPrincipalKey(p => p.Gamecode)
                .HasForeignKey(d => d.GameCode)
                .HasConstraintName("FK__GamePlayR__GameC__66603565");

            entity.HasOne(d => d.NewPositionNavigation).WithMany(p => p.GamePlayRecordNewPositionNavigations)
                .HasForeignKey(d => d.NewPosition)
                .HasConstraintName("FK__GamePlayR__NewPo__656C112C");

            entity.HasOne(d => d.OldPositionNavigation).WithMany(p => p.GamePlayRecordOldPositionNavigations)
                .HasForeignKey(d => d.OldPosition)
                .HasConstraintName("FK__GamePlayR__OldPo__6477ECF3");

            entity.HasOne(d => d.Player).WithMany(p => p.GamePlayRecords)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("FK__GamePlayR__Playe__6383C8BA");
        });

        modelBuilder.Entity<GameRecord>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__GameReco__2AB897FD3AE35458");

            entity.ToTable("GameRecord");

            entity.HasIndex(e => e.Gamecode, "UQ__GameReco__CD4096F14D901FB0").IsUnique();

            entity.Property(e => e.FirstWinnerPlayerUid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("FirstWinnerPlayerUID");
            entity.Property(e => e.Gamecode)
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.HasOne(d => d.FirstWinnerPlayerU).WithMany(p => p.GameRecords)
                .HasForeignKey(d => d.FirstWinnerPlayerUid)
                .HasConstraintName("FK__GameRecor__First__5FB337D6");
        });

        modelBuilder.Entity<PlayerCharacter>(entity =>
        {
            entity.HasKey(e => e.PlayerUid).HasName("PK__PlayerCh__E017C004A03CCE1D");

            entity.ToTable("PlayerCharacter");

            entity.HasIndex(e => e.CharacterColor, "UQ__PlayerCh__A5C7E09F852695D0").IsUnique();

            entity.Property(e => e.PlayerUid)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PlayerUID");
            entity.Property(e => e.CharacterColor)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CharacterName)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.PlayerCurrentPositionNavigation).WithMany(p => p.PlayerCharacters)
                .HasForeignKey(d => d.PlayerCurrentPosition)
                .HasConstraintName("FK__PlayerCha__Playe__5BE2A6F2");
        });

        modelBuilder.Entity<SnakeBoard>(entity =>
        {
            entity.HasKey(e => e.CellNum).HasName("PK__SnakeBoa__75EEB90FAF85EE2D");

            entity.ToTable("SnakeBoard");

            entity.Property(e => e.CellNum).ValueGeneratedNever();
            entity.Property(e => e.CellType).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
