using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.Asset.Spell;

namespace RPGOnline.Application.Interfaces
{
    public interface ISpell
    {
        Task<ICollection<GetSpellSimplifiedResponse>> GetSpellsForCharacter(int uId);
        Task<(ICollection<GetSpellResponse>, int pageCount)> GetSpells(SearchAssetRequest searchSpellRequest, int userId, CancellationToken cancellationToken);
        Task<GetSpellResponse> PostSpell(PostSpellRequest postSpellRequest);
    }
}
