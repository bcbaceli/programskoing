using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;

namespace WeddingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<PartnerCategory> PartnerCategories => Set<PartnerCategory>();
        public DbSet<Partner> Partners => Set<Partner>();
        public DbSet<WeddingTemplate> WeddingTemplates => Set<WeddingTemplate>();
        public DbSet<Wedding> Weddings => Set<Wedding>();
        public DbSet<SongGenre> SongGenres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongPlaylist> SongPlaylists { get; set; }
        public DbSet<GenrePlaylist> GenrePlaylists { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Pastry> Pastries { get; set; }
        public DbSet<Florist> Florists { get; set; }
        public DbSet<SpecialRequest> SpecialRequests { get; set; }
        public DbSet<FoodMenu> FoodMenus { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<WeddingCeremony> WeddingCeremonies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WeddingCeremony>()
            .HasOne(x => x.Wedding)
            .WithMany()
            .HasForeignKey(x => x.WeddingId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeddingCeremony>()
                .HasOne(x => x.Restaurant)
                .WithMany(x => x.WeddingCeremonies)
                .HasForeignKey(x => x.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WeddingCeremony>()
                .HasOne(x => x.Florist)
                .WithMany(x => x.WeddingCeremonies)
                .HasForeignKey(x => x.FloristId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WeddingCeremony>()
                .HasOne(x => x.Band)
                .WithMany(x => x.WeddingCeremonies)
                .HasForeignKey(x => x.BandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}