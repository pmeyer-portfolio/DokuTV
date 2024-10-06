namespace Domain.Entities
{
    public class Playlist
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Video> Videos { get; set; } = new List<Video>();
    }
}
