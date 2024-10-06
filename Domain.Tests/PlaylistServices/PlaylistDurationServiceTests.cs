using Domain.Entities;
using Domain.PlaylistServices;
using Moq;
using Serilog;

namespace Domain.Tests.PlaylistServices
{
    public class PlaylistDurationServiceTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly PlaylistDurationService _playlistDurationService;

        public PlaylistDurationServiceTests()
        {
            _loggerMock = new Mock<ILogger>();
            _playlistDurationService = new PlaylistDurationService(_loggerMock.Object);
        }

        [Fact]
        public void CalculateTotalDuration_ValidDurations_ReturnsCorrectTotalDuration()
        {
            // Arrange
            var playlist = new Playlist
            {
                Videos = new List<Video>
                {
                    new Video { Id = "1", Duration = "PT10M" },
                    new Video { Id = "2", Duration = "PT5M" }
                }
            };

            // Act
            var result = _playlistDurationService.CalculateTotalDuration(playlist);

            // Assert
            var expectedDuration = TimeSpan.FromMinutes(15);
            Assert.Equal(expectedDuration, result);
            _loggerMock.Verify(x => x.Warning(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        [Fact]
        public void CalculateTotalDuration_InvalidDuration_LogsWarning()
        {
            // Arrange
            var playlist = new Playlist
            {
                Videos = new List<Video>
                {
                    new Video { Id = "1", Duration = "PT10M" },
                    new Video { Id = "2", Duration = "InvalidFormat" }
                }
            };

            // Act
            var result = _playlistDurationService.CalculateTotalDuration(playlist);

            // Assert
            var expectedDuration = TimeSpan.FromMinutes(10);
            Assert.Equal(expectedDuration, result);

            _loggerMock.Verify(x => x.Warning(
                "Ungültiges Video-Dauerformat für Video ID: {VideoId}. Dauer: {Duration}. Fehler: {ErrorMessage}",
                "2", "InvalidFormat", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void CalculateTotalDuration_EmptyPlaylist_ReturnsZeroDuration()
        {
            // Arrange
            var playlist = new Playlist
            {
                Videos = new List<Video>()
            };

            // Act
            var result = _playlistDurationService.CalculateTotalDuration(playlist);

            // Assert
            Assert.Equal(TimeSpan.Zero, result);
            _loggerMock.Verify(x => x.Warning(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        [Fact]
        public void CalculateTotalDuration_AllInvalidDurations_LogsWarningsForEachVideo()
        {
            // Arrange
            var playlist = new Playlist
            {
                Videos = new List<Video>
        {
            new Video { Id = "1", Duration = "InvalidFormat1" },
            new Video { Id = "2", Duration = "InvalidFormat2" }
        }
            };

            // Act
            var result = _playlistDurationService.CalculateTotalDuration(playlist);

            // Assert
            Assert.Equal(TimeSpan.Zero, result);

            _loggerMock.Verify(x => x.Warning(
                "Ungültiges Video-Dauerformat für Video ID: {VideoId}. Dauer: {Duration}. Fehler: {ErrorMessage}",
                "1", "InvalidFormat1", It.IsAny<string>()), Times.Once);

            _loggerMock.Verify(x => x.Warning(
                "Ungültiges Video-Dauerformat für Video ID: {VideoId}. Dauer: {Duration}. Fehler: {ErrorMessage}",
                "2", "InvalidFormat2", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void CalculateTotalDuration_PartiallyInvalidDurations_ReturnsCorrectTotalDurationAndLogsWarnings()
        {
            // Arrange
            var playlist = new Playlist
            {
                Videos = new List<Video>
        {
            new Video { Id = "1", Duration = "PT20M" },
            new Video { Id = "2", Duration = "InvalidFormat" },
            new Video { Id = "3", Duration = "PT15M" }
        }
            };

            // Act
            var result = _playlistDurationService.CalculateTotalDuration(playlist);

            // Assert
            var expectedDuration = TimeSpan.FromMinutes(35);
            Assert.Equal(expectedDuration, result);

            _loggerMock.Verify(x => x.Warning(
                "Ungültiges Video-Dauerformat für Video ID: {VideoId}. Dauer: {Duration}. Fehler: {ErrorMessage}",
                "2", "InvalidFormat", It.IsAny<string>()), Times.Once);
        }

    }
}
