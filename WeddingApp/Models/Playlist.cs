namespace WeddingApp.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int BandId { get; set; }
        public Band Band { get; set; } = null!;

        public ICollection<GenrePlaylist> GenrePlaylists { get; set; } = new List<GenrePlaylist>();
        public ICollection<SongPlaylist> SongPlaylists { get; set; } = new List<SongPlaylist>();
    }
}