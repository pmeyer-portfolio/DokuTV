using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IPlaylistDurationService
    {
        TimeSpan CalculateTotalDuration(Playlist playlist);
    }
}
