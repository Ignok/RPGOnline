using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Character;
using RPGOnline.Application.DTOs.Responses.Asset.Character;
using RPGOnline.Application.DTOs.Responses.Asset.Character.Character;
using RPGOnline.Application.DTOs.Responses.Character;

namespace RPGOnline.Application.Interfaces
{
    public interface ICharacter
    {
        Task<CharacterResponse> GetCharacter(int characterId);

        Task<(ICollection<CharacterResponse>, int pageCount)> GetCharacters(string type, SearchAssetRequest searchAssetRequest, int userId, CancellationToken cancellationToken);

        Task<MotivationResponse> GetRandomMotivation();

        Task<CharacteristicsResponse> GetRandomCharacteristics();

        Task<AttributesResponse> GetRandomAttributes();

        Task<CharacterSimplifiedResponse> PostCharacter(PostCharacterRequest postCharacterRequest);
    }
}
