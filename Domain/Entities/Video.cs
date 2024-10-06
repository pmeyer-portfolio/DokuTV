namespace Domain.Entities
{
    public class Video
    {
        // Identifikation des Videos
        public string Id { get; set; }

        // Allgemeine Informationen
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }

        // Thumbnail-Informationen
        public string ThumbnailUrl { get; set; }

        // Kanalinformationen
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }

        // Kategorie und Lizenz
        public string CategoryId { get; set; }
        public string License { get; set; }

        // Sprachinformationen
        public string DefaultLanguage { get; set; }
        public string DefaultAudioLanguage { get; set; }

        // Inhaltliche Details
        public string Duration { get; set; }
        public bool LicensedContent { get; set; }
        public string SourceUrl { get; set; }
    }
}