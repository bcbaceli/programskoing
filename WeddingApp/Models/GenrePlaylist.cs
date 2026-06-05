namespace WeddingApp.Models
{
    public class GenrePlaylist
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public SongGenre Genre { get; set; } = null!;

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;
    }
}