using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Profession;
using RPGOnline.Application.DTOs.Responses.Asset.Profession;

namespace RPGOnline.Application.Interfaces
{
    public interface IProfession
    {
        Task<ICollection<GetProfessionSimplifiedResponse>> GetProfessionsForCharacter(int uId, GetAssetForCharacterRequest getProfessionRequest);
        Task<(ICollection<GetProfessionResponse>, int pageCount)> GetProfessions(SearchAssetRequest searchProfessionRequest, int userId, CancellationToken cancellationToken);
        Task<GetProfessionResponse> PostProfession(PostProfessionRequest postProfessionRequest);
    }
}
