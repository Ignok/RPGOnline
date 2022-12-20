using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Race;
using RPGOnline.Application.DTOs.Responses.Asset.Race;

namespace RPGOnline.Application.Interfaces
{
    public interface IRace
    {
        Task<ICollection<GetRaceSimplifiedResponse>> GetRacesForCharacter(int uId, GetAssetForCharacterRequest getRacesForCharacterRequest);
        Task<(ICollection<GetRaceResponse>, int pageCount)> GetRaces(SearchAssetRequest searchRaceRequest, CancellationToken cancellationToken);
        Task<GetRaceResponse> PostRace(PostRaceRequest postRaceRequest);
    }
}
