using Microsoft.EntityFrameworkCore;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Infraestructure.Persistence.Context
{
    public class PlaylistContext:DbContext
    {
        public PlaylistContext(DbContextOptions<PlaylistContext> options) : base(options) { }

        #region Models
        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User Configuration

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(b => b.UserId);
                entity.Property(b => b.UserId).ValueGeneratedOnAdd();
                entity.Property(b => b.Name).IsRequired().HasMaxLength(50);
                entity.Property(b => b.LastName).IsRequired().HasMaxLength(50);
                entity.Property(b => b.Email).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Password).IsRequired();
                entity.HasMany(b => b.Playlists)
                    .WithOne(b => b.User)
                    .HasForeignKey(b => b.UserId);
            });

            #endregion

            #region Playlist Configuration

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasKey(b => b.PlaylistId);
                entity.Property(b => b.Name).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Description).HasMaxLength(255);
                entity.HasOne(b => b.User)
                    .WithMany(b => b.Playlists)
                    .HasForeignKey(b => b.UserId);
            });

            #endregion


            #region Song Configuration

            modelBuilder.Entity<Song>(entity => 
            {
                entity.HasKey(b => b.SongId);
                entity.Property(b => b.Artist).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Genre).IsRequired().HasMaxLength(50);
            });

            #endregion

            #region Relationship Many to Many Playlist ↔ Song

            modelBuilder.Entity<Playlist>()
                .HasMany(b => b.Songs)
                .WithMany(b => b.Playlists)
                .UsingEntity(c => c.ToTable("PlaylistSongs"));

            #endregion
        }
    }
}
