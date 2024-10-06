using Domain.Abstractions;
using Domain.Entities;
using Serilog;
using System.Xml;

namespace Domain.PlaylistServices
{
    public class PlaylistDurationService : IPlaylistDurationService
    {
        private readonly ILogger _logger;
        public PlaylistDurationService(ILogger logger)
        {
            _logger = logger;
        }
        public TimeSpan CalculateTotalDuration(Playlist playlist)
        {
            var totalDuration = TimeSpan.Zero;

            foreach (var video in playlist.Videos)
            {
                try
                {
                    var videoDuration = XmlConvert.ToTimeSpan(video.Duration);
                    totalDuration += videoDuration;
                }
                catch (FormatException ex)
                {
                    _logger.Warning("Ungültiges Video-Dauerformat für Video ID: {VideoId}. Dauer: {Duration}. Fehler: {ErrorMessage}",
                               video.Id, video.Duration, ex.Message);
                }

            }

            return totalDuration;
        }
    }
}
