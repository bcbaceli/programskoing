namespace WeddingApp.Models
{
    public class SongGenre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Song> Songs { get; set; } = new List<Song>();
        public ICollection<GenrePlaylist> GenrePlaylists { get; set; } = new List<GenrePlaylist>();
    }
}