namespace WeddingApp.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public SongGenre Genre { get; set; } = null!;

        public ICollection<SongPlaylist> SongPlaylists { get; set; } = new List<SongPlaylist>();
    }
}