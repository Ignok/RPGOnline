using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Race;
using RPGOnline.Application.DTOs.Responses.Asset.Race;

namespace RPGOnline.Application.Interfaces
{
    public interface IRace
    {
        Task<ICollection<GetRaceSimplifiedResponse>> GetRacesForCharacter(int uId, GetAssetForCharacterRequest getRaceRequest);
        Task<(ICollection<GetRaceResponse>, int pageCount)> GetRaces(SearchAssetRequest searchRaceRequest, int userId, CancellationToken cancellationToken);
        Task<GetRaceResponse> PostRace(PostRaceRequest postRaceRequest);
    }
}
