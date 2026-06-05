namespace WeddingApp.Models
{
    public class SongPlaylist
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; } = null!;

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;
    }
}